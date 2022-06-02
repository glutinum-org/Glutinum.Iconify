module IconifyIconsBindingGenerator.Generator

open System.IO
open System.Text.RegularExpressions
open System.Text
open StringBuilder.Extensions
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open CLI

/// <summary>
/// For <c>packages/Glutinum.IconifyIcons.AntDesign/Glutinum.IconifyIcons.AntDesign.fsproj</c>
/// it represents <c>packages/Glutinum.IconifyIcons.AntDesign<c>
/// </summary>
type FSharpProjectFolder = FSharpProjectFolder of string

/// <summary>
/// For <c>packages/Glutinum.IconifyIcons.AntDesign/Glutinum.IconifyIcons.AntDesign.fsproj</c>
/// it represents <c>Glutinum.IconifyIcons.AntDesign<c>
/// </summary>
type FSharpProjectName = FSharpProjectName of string

/// <summary>
/// For <c>Glutinum.IconifyIcons.AntDesign.fsproj</c>
/// it represents <c>AntDesign<c>
/// </summary>
type FSharpPackageName = FSharpPackageName of string

/// <summary>
/// Represents a method name in F#.
///
/// <code lang="fsharp">
/// type antDesign =
///
///     static member inline accountBookFill : IconifyIcon =
///         jsNative
/// </code>
///
/// it represents <c>accountBookFill</c>
/// </summary>
type FSharpMethodName = FSharpMethodName of string

/// <summary>
/// For <c>node_modules/@iconify-icons/ant-design/account-book.js</c>
/// it represents <c>account-book</c>
/// </summary>
type JavaScripModuleName = JavaScripModuleName of string

/// <summary>
/// For <c>node_modules/@iconify-icons/ant-design</c>
/// it represents <c>@iconify-icons/ant-design</c>
/// </summary>
type NpmPackageName = NpmPackageName of string

/// <summary>
/// For <c>node_modules/@iconify-icons/ant-design</c>
/// it represents <c>ant-design</c>
/// </summary>
type IconifyIconPackageName = IconifyIconPackageName of string

type ExceptionRule =
    | Skip
    | ForceName of string

let private writeMethod
    (sb : StringBuilder)
    (NpmPackageName npmPackageName)
    (JavaScripModuleName jsModuleName)
    (FSharpMethodName fsharpMethodName) =

    sb.WriteLine $"""    /// <summary>"""
    sb.WriteLine $"""    /// Binding for <c>%s{npmPackageName}/%s{jsModuleName}</c>"""
    sb.WriteLine $"""    /// </summary>"""
    sb.WriteLine $"""    [<Import("default", "%s{npmPackageName}/%s{jsModuleName}")>]"""
    sb.WriteLine $"""    static member inline %s{fsharpMethodName} : IconifyIcon ="""
    sb.WriteLine "        jsNative"
    sb.NewLine()

module private PackageJson =

    let findPackageVersion (NpmPackageName npmPackageName) =
        let packageJsonContent = File.ReadAllText("package.json")

        let pattern = $"\"%s{npmPackageName}\": \"\^(?<version>.*)\""

        let m = Regex.Match(packageJsonContent, pattern)

        if m.Success then
            m.Groups["version"].Value
        else
            failwithf $"Unable to find the version for '@iconify-icons/%s{npmPackageName}' in package.json"

module private Fsproj =

    let private npmPackageTagVersionPattern = """(?<start_tag><NpmPackage .* Version=")(?<version>[^"]*)(?<end_tag>" .*\/>)"""

    let private projectPath (projectName : string) =
        "packages" </> projectName </> $"%s{projectName}.fsproj"

    let findNpmPackageVersion (FSharpProjectName fsharpProjectName) =

        let fsprojPath = projectPath fsharpProjectName
        let fsprojContent = File.ReadAllText fsprojPath

        let m = Regex.Match(fsprojContent, npmPackageTagVersionPattern)

        if m.Success then
            m.Groups["version"].Value
        else
            failwithf $"Unable to find the version Femto Metadata in %s{fsprojPath}"

    let updateNpmPackageVersion
        (FSharpProjectName fsharpProjectName)
        (npmPackageName : NpmPackageName) =

        let fsprojPath = projectPath fsharpProjectName
        let fsprojContent = File.ReadAllText fsprojPath
        let npmPackageVersion = PackageJson.findPackageVersion npmPackageName

        // Replace the version in the femto metadata
        let newFsprojContent =
            Regex.Replace(
                fsprojContent,
                npmPackageTagVersionPattern,
                fun m ->
                    m.Groups["start_tag"].Value + $"&gt;= %s{npmPackageVersion}" + m.Groups["end_tag"].Value
            )

        // Write the new fsproj content
        File.WriteAllText(fsprojPath, newFsprojContent)

let private replaceNpmPackageName (projectName : string) (npmPackageName : string) =
    let fsprojPath = "packages" </> projectName </> $"%s{projectName}.fsproj"

    let fsprojContent = File.ReadAllText fsprojPath

    // Replace the npm package name in the femto metadata
    let newFsprojContent =
        Regex.Replace(
            fsprojContent,
            "{npmPackageName}",
            npmPackageName
        )

    // Write the new fsproj content
    File.WriteAllText(fsprojPath, newFsprojContent)

let private initBindingIfNeeded
    (FSharpProjectFolder fsharpProjectFolder)
    (FSharpProjectName fsharpProjectName)
    (NpmPackageName npmPackageName) =

    // If the package doesn't exist, create it
    if not (Directory.Exists fsharpProjectFolder) then
        Directory.CreateDirectory fsharpProjectFolder |> ignore

        // Copy the files from the template
        File.WriteAllText(
            fsharpProjectFolder </> "CHANGELOG.md",
            Templates.changelog
        )

        File.WriteAllText(
            fsharpProjectFolder </> $"%s{fsharpProjectName}.fsproj",
            Templates.projectFsproj npmPackageName
        )

        File.WriteAllText(
            fsharpProjectFolder </> "paket.references",
            Templates.paketReferences
        )

        // Add the project to the solution
        run dotnet $"sln add %s{fsharpProjectFolder}" cwd
        // Add the project to the test project
        run dotnet $" dotnet add tests/Tests.fsproj reference %s{fsharpProjectFolder}" cwd

let private writeBindingHeader
    (sb : StringBuilder)
    (FSharpProjectName fsharpProjectName)
    (IconifyIconPackageName iconPackageName) =

    // CamelCase version of the icon package
    // ant-design => antDesign
    let safeIconPackageName = String.camelize iconPackageName

    sb.WriteLine $"""namespace %s{fsharpProjectName}

open Fable.Core
open Glutinum.Iconify

[<Erase>]
type %s{safeIconPackageName} =
"""

let private writeMembers
    (sb : StringBuilder)
    (npmPackageName : NpmPackageName)
    (exceptions : Map<string, ExceptionRule>)
     =

    let (NpmPackageName npmPackageNameValue) = npmPackageName


    // Takes all the d.ts files in the icon package folder
    !! ("node_modules" </> npmPackageNameValue </> "*.d.ts")
    |> Seq.iter (fun fileName ->
        let file = FileInfo (fileName)

        // The name of the JavaScript module
        let jsModuleName =
            file.Name
            |> String.remove ".d.ts"

        match Map.tryFind jsModuleName exceptions with
        | Some rule ->
            match rule with
            | Skip ->
                ()
            | ForceName forcedName ->
                writeMethod sb npmPackageName (JavaScripModuleName jsModuleName) (FSharpMethodName forcedName)

        | None ->
            // F# method name for the icon
            // We sanitize the name to avoid invalid characters
            let methodName =
                jsModuleName
                // Note: We can't use camelize here because some packages has the same icons provided with different names
                |> String.camelize
                |> String.sanitizeIfStartWithNumber
                |> String.sanitizeKeyword

            writeMethod sb npmPackageName (JavaScripModuleName jsModuleName) (FSharpMethodName methodName)
    )

let private writeBindingToFile
    (sb : StringBuilder)
    (FSharpProjectFolder fsharpProjectFolder) =

    let generatedFileDestination = fsharpProjectFolder </> "Types.fs"

    // Write the generated code to the destination file
    File.WriteAllText(generatedFileDestination, sb.ToString())

let private addChangelogEntry
    (FSharpProjectFolder fsharpProjectFolder)
    (npmPackageName : NpmPackageName)
    (npmPackageVersion : string) =

    let changelogPath = fsharpProjectFolder </> "CHANGELOG.md"
    let changelogContent = File.ReadAllText changelogPath

    // Compute the next version of the binding
    let nextVersion =
        // If there is a version in the changelog
        // we increment the minor version value
        // Example:
        // - 1.1.0 => 1.2.0
        // - 2.45.0 => 2.46.0
        match Changelog.findLatestVersion changelogPath with
        | Some oldVersion ->
            let nextMinorVersion = oldVersion.Minor.Value + 1
            $"1.%i{nextMinorVersion}.0"

        // If there is not version yet
        // Initialize the version to 1.0.0
        | None ->
            "1.0.0"

    let newChangelogContent =
        let (NpmPackageName npmPackageName) = npmPackageName

        let marker = "## Unreleased"
        let makerIndex = changelogContent.IndexOf(marker)

        let newEntry = Templates.changelogEntry nextVersion npmPackageName npmPackageVersion

        changelogContent.Insert(makerIndex + marker.Length, newEntry)

    // Write the new changelog content
    File.WriteAllText(changelogPath, newChangelogContent)

type IconifyIconsGeneratorConfig =
    {
        IconifyIconPackageName : string
        Exceptions : Map<string, ExceptionRule>
    }

let generateBinding (config : IconifyIconsGeneratorConfig) =

    // PascalCase version of the icon package
    // ant-design => AntDesign
    let fsharpPackageName = FSharpPackageName (String.pascalize config.IconifyIconPackageName)

    // F# project name
    let fsharpProjectName =
        let (FSharpPackageName fsharpPackageName) = fsharpPackageName
        FSharpProjectName ($"Glutinum.IconifyIcons.%s{fsharpPackageName}")

    // Relative path to the destination folder
    let fsharpProjectFolder =
        let (FSharpProjectName fsharpProjectName) = fsharpProjectName
        FSharpProjectFolder ("packages" </> fsharpProjectName)

    let npmPackageName = NpmPackageName $"@iconify-icons/%s{config.IconifyIconPackageName}"

    let iconifyIconPackageName = IconifyIconPackageName config.IconifyIconPackageName

    // 1. Create the project if it doesn't exist
    initBindingIfNeeded fsharpProjectFolder fsharpProjectName npmPackageName

    // 2. Check if the NPM package version in package.json
    // and the femto metadata
    let currentNpmPackageVersionBound =
        Fsproj.findNpmPackageVersion fsharpProjectName

    let currentNpmPackageVersion =
        PackageJson.findPackageVersion npmPackageName

    // If they are different, refresh the binding
    if currentNpmPackageVersionBound <> currentNpmPackageVersion then
        // Update the Femto metadata
        Fsproj.updateNpmPackageVersion fsharpProjectName npmPackageName

        let sb = StringBuilder()

        // Generate the binding content
        writeBindingHeader sb fsharpProjectName iconifyIconPackageName
        writeMembers sb npmPackageName config.Exceptions
        // Write the new binding
        writeBindingToFile sb fsharpProjectFolder
        // Add a changelog entry
        addChangelogEntry fsharpProjectFolder npmPackageName currentNpmPackageVersion

//    // Otherwise, do nothing

let refreshReferencesTable (references : IconifyIconsGeneratorConfig list) =

    let tableHeader = """| Nuget                   | Npm      |
|-------------------------|----------|"""

    let tableRows =
        references
        |> List.map (fun config ->
            // PascalCase version of the icon package
            // ant-design => AntDesign
            let fsharpPackageName = String.pascalize config.IconifyIconPackageName

            let npmPackageName = $"@iconify-icons/%s{config.IconifyIconPackageName}"

            $"| `Glutinum.IconifyIcons.%s{fsharpPackageName}` | `%s{npmPackageName}` |"
        )


    let newReferenceTable =
        [
            "<!-- Begin:binding_reference_table -->"
            tableHeader
            yield! tableRows
            "<!-- End:binding_reference_table -->"
        ]
        |> String.concat "\n"

    let readmePath = "README.md"
    let readmeContent = File.ReadAllText readmePath

    let newReadmeContent =
        Regex.Replace(
            readmeContent,
            "<!-- Begin:binding_reference_table -->(.|\s)*<!-- End:binding_reference_table -->",
            newReferenceTable
        )

    File.WriteAllText(readmePath, newReadmeContent)
