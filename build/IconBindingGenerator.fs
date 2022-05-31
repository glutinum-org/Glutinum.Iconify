module IconBindingGenerator

open System.IO
open System.Text.RegularExpressions
open System.Text
open StringBuilder.Extensions

open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators

let private removeExtension (value : string) =
    value.Replace(".d.ts", "")

let private sanitizeStartWithNumber (value : string) =
    if Regex.IsMatch(value, "^\d") then
        "_" + value
    else
        value

let private replaceHyphenWithUnderscore (input : string) =
    input.Replace("-", "_")

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

let generateBinding (iconPackageFolder : string) =
    let packageName = pascalize iconPackageFolder
    let projectName = $"Glutinum.IconifyIcons.%s{packageName}"
    let packageDestination = "packages" </> projectName

    // If the package doesn't exist, create it
    if not (Directory.Exists packageDestination) then
        Directory.CreateDirectory packageDestination |> ignore

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


//     let safeIconPackageName = camelize iconPackageFolder

//     let sb = new StringBuilder()

//     sb.WriteLine $"""namespace Glutinum.IconifyIcons.%s{packageName}

// open Fable.Core

// [<Erase>]
// type %s{safeIconPackageName} =
// """
//     !! ("node_modules" </> "@iconify-icons" </> iconPackageFolder </> "*.d.ts")
//     |> Seq.iter (fun fileName ->
//         let file = new FileInfo (fileName)

//         let fileName =
//             file.Name
//             |> removeExtension

//         let methodName =
//             fileName
//             |> replaceHyphenWithUnderscore
//             |> sanitizeStartWithNumber
//             |> sanitizeKeyword

//         sb.WriteLine $"""    [<Import("default", "@iconify-icons/mdi/%s{fileName}")>]"""

//         sb.WriteLine $"""    static member inline %s{methodName} : IconifyIcon ="""
//         // sb.WriteLine $"""    let %s{methodName} : IconifyIcon = jsNative"""
//         sb.WriteLine "        jsNative"
//         sb.NewLine()
//     )

//     // Ensure that the destination path exists
//     Directory.CreateDirectory "src/Icons" |> ignore

//     // Write the generated code to the destination file
//     File.WriteAllText($"src/Icons/%s{safeIconPackageName}.fs", sb.ToString())
