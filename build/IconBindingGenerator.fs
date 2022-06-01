module IconBindingGenerator

open System.IO
open System.Text.RegularExpressions
open System.Text
open StringBuilder.Extensions
open BuildHelpers
open System.Security.Cryptography
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open System.Collections.Generic

let private removeExtension (value : string) =
    value.Replace(".d.ts", "")

let private sanitizeStartWithNumber (value : string) =
    if Regex.IsMatch(value, "^\d") then
        "_" + value
    else
        value

let pascalize (input : string) =
    Regex.Replace(
        input,
        "(?:^|_|-| +)(.)",
        fun m -> m.Groups[1].Value.ToUpper()
    )

let private capitalize (input : string) =
    if input.Length > 0 then
        (string input[0]).ToLower() + input.Substring(1)
    else
        input

let private camelize (input : string) =
    let word = pascalize input
    if word.Length > 0 then
        (string word[0]).ToLower() + word.Substring(1)
    else
        word

let private sanitizeKeyword (input : string) =
    if Set.contains input FSharp.Keywords.keywords then
        input + "_"
    else
        input

type ExceptionRule =
    | Skip
    | ForceName of string

let private writeMethod (sb : StringBuilder) (packageName : string) (moduleName : string) (methodName : string)=

    sb.WriteLine $"""    /// <summary>"""
    sb.WriteLine $"""    /// Binding for <c>@iconify-icons/%s{packageName}/%s{moduleName}</c>"""
    sb.WriteLine $"""    /// </summary>"""
    sb.WriteLine $"""    [<Import("default", "@iconify-icons/%s{packageName}/%s{moduleName}")>]"""
    sb.WriteLine $"""    static member inline %s{methodName} : IconifyIcon ="""
    sb.WriteLine "        jsNative"
    sb.NewLine()

let generateBinding (iconPackageFolder : string) (exceptions : Map<string, ExceptionRule>)=
    // PascalCase version of the icon package
    // ant-design => AntDesign
    let packageName = pascalize iconPackageFolder

    // F# project name
    let projectName = $"Glutinum.IconifyIcons.%s{packageName}"

    // Relative path to the destination folder
    let packageDestination = "packages" </> projectName
    let generatedFileDestination = packageDestination </> "Types.fs"

    // CamelCase version of the icon package
    // ant-desing => antDesign
    let safeIconPackageName = camelize iconPackageFolder

    // If the package doesn't exist, create it
    if not (Directory.Exists packageDestination) then
        Directory.CreateDirectory packageDestination |> ignore

        // Copy the files from the template
        File.Copy(
            __SOURCE_DIRECTORY__ </> "./templates/CHANGELOG.md",
            packageDestination </> "CHANGELOG.md"
        )

        File.Copy(
            __SOURCE_DIRECTORY__ </> "./templates/Project.fsproj",
            packageDestination </> $"%s{projectName}.fsproj"
        )

        File.Copy(
            __SOURCE_DIRECTORY__ </> "./templates/paket.references",
            packageDestination </> "paket.references"
        )

        // Add the project to the solution
        run dotnet $"sln add %s{packageDestination}" cwd
        // Add the project to the test project
        run dotnet $" dotnet add tests/Tests.fsproj reference %s{packageDestination}" cwd

    let sb = new StringBuilder()

    sb.WriteLine $"""namespace %s{projectName}

open Fable.Core
open Glutinum.Iconify

[<Erase>]
type %s{safeIconPackageName} =
"""

    // Takes all the d.ts files in the icon package folder
    !! ("node_modules" </> "@iconify-icons" </> iconPackageFolder </> "*.d.ts")
    |> Seq.iter (fun fileName ->
        let file = new FileInfo (fileName)

        // The name of the JavaScript module
        let moduleName =
            file.Name
            |> removeExtension

        match Map.tryFind moduleName exceptions with
        | Some rule ->
            match rule with
            | Skip ->
                ()
            | ForceName forcedName ->
                writeMethod sb iconPackageFolder moduleName forcedName

        | None ->
            // F# method name for the icon
            // We sanitize the name to avoid invalid characters
            let methodName =
                moduleName
                // Note: We can't use camelize here because some packages has the same icons provided with different names
                |> camelize
                |> sanitizeStartWithNumber
                |> sanitizeKeyword

            writeMethod sb iconPackageFolder moduleName methodName
    )

    // Write the generated code to the destination file
    File.WriteAllText(generatedFileDestination, sb.ToString())
