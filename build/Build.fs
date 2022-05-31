open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open BlackFox.Fake
open BuildHelpers
open BlackFox.CommandLine
open BlackFox
open System
open System.Text.RegularExpressions
open Fake.Api

let cwd = System.Environment.CurrentDirectory

let demoPath = Path.getFullName "demo"
let srcPath = Path.getFullName "src"

let gitOwner = "glutinum-org"
let gitName = "Glutinum.Feliz.Iconify"

[<EntryPoint>]
let main args =
    BuildTask.setupContextFromArgv args

    let clean = BuildTask.create "Clean" [] {
        [
            demoPath </> "bin"
            demoPath </> "obj"
            srcPath </> "bin"
            srcPath </> "obj"
        ]
        |> Shell.cleanDirs

        !! (Glob.fableJs demoPath)
        ++ (Glob.fableJs srcPath)
        |> Seq.iter Shell.rm
    }

    let npmInstall = BuildTask.create "NpmInstall" [] {
        run npm "install" cwd
    }

    let refreshIcons = BuildTask.create "RefreshIcons" [] {
        IconBindingGenerator.generateBinding "ant-design"
        IconBindingGenerator.generateBinding "arcticons"
        IconBindingGenerator.generateBinding "carbon"
        IconBindingGenerator.generateBinding "eva"
        IconBindingGenerator.generateBinding "fa-regular"
        IconBindingGenerator.generateBinding "fa-solid"
        IconBindingGenerator.generateBinding "fe"
        IconBindingGenerator.generateBinding "feather"
        IconBindingGenerator.generateBinding "fluent"
        IconBindingGenerator.generateBinding "fontisto"
        IconBindingGenerator.generateBinding "grommet-icons"
        IconBindingGenerator.generateBinding "healthicons"
        IconBindingGenerator.generateBinding "heroicons-outline"
        IconBindingGenerator.generateBinding "heroicons-solid"
        IconBindingGenerator.generateBinding "ic"
        IconBindingGenerator.generateBinding "logos"
        IconBindingGenerator.generateBinding "mdi"
        IconBindingGenerator.generateBinding "simple-icons"
        IconBindingGenerator.generateBinding "tabler"
        IconBindingGenerator.generateBinding "vscode-icons"
    }

    let watchDemo = BuildTask.create "WatchDemo" [ npmInstall; clean ] {
        // All for graceful shutdown on Ctrl+C while the processes are running
        Console.CancelKeyPress.AddHandler(fun _ ea ->
            ea.Cancel <- true
            printfn "Received Ctrl+C, shutting down..."
            Environment.Exit(0)
        )

        [
            "vite", npx "vite dev" demoPath
            "fable", dotnet "fable --watch" demoPath
        ]
        |> runParallel
    }

    let release = BuildTask.create "Release" [ ] {
        let nugetKey =
            match Environment.environVarOrNone "nuget_key" with
            | Some nugetKey ->
                nugetKey

            | None ->
                failwith "nuget_key environment variable is not set"

        let githubToken =
            match Environment.environVarOrNone "github_token" with
            | Some githubKey ->
                githubKey

            | None ->
                failwith "github_token environment variable is not set"

        let (stdout, _) =
            dotnet "pack -c Release" srcPath
            |> Proc.runWithCaptureOutputAndRedirect

        let m = Regex.Match(stdout, ".*'(?<nupkg_path>.*\.(?<version>.*\..*\..*)\.nupkg)'")

        if not m.Success then
            failwith "Could not find nupkg file in output"
        else
            let nupkgFile = m.Groups["nupkg_path"].Value
            let version =
                m.Groups["version"].Value
                |> SemVersion.SemanticVersion.Parse

            let (_, _, changelogData) =
                Changelog.findEntry "CHANGELOG.md" version

            let releaseNote =
                Option.map Changelog.Util.allReleaseNotesFor changelogData
                |> Option.defaultValue ""

            let args =
                CmdLine.empty
                |> CmdLine.appendRaw "nuget"
                |> CmdLine.appendRaw "push"
                |> CmdLine.append nupkgFile
                |> CmdLine.appendPrefix "--source" "nuget.org"
                |> CmdLine.appendPrefix "--api-key" nugetKey
                |> CmdLine.toString

            run dotnet args cwd

            let tagVersion = $"v%s{version.ToString()}"

            run git "add ." cwd
            run git $"""commit -m "Release %s{tagVersion}""" cwd
            run git $"push" cwd
            run git $"""tag -a %s{tagVersion} -m "Release %s{tagVersion}" """ cwd
            run git $"push origin %s{tagVersion}" cwd

            GitHub.createClientWithToken githubToken
            |> GitHub.draftNewRelease gitOwner gitName tagVersion (not (String.IsNullOrEmpty version.Prerelease)) [ releaseNote ]
            |> GitHub.publishDraft
            |> Async.RunSynchronously
    }

    BuildTask.runOrList ()
    0
