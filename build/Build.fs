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
open IconBindingGenerator

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
        generateBinding "ant-design" Map.empty
        // generateBinding "arcticons" Map.empty
        // generateBinding "carbon" Map.empty
        // generateBinding "eva" Map.empty
        // generateBinding "fa-regular" Map.empty
        // generateBinding "fa-solid" Map.empty
        // generateBinding "fe" Map.empty
        // generateBinding "feather" Map.empty
        // generateBinding "fluent" Map.empty
        // generateBinding "fontisto" Map.empty
        // generateBinding "grommet-icons" Map.empty
        // generateBinding "healthicons" Map.empty
        // generateBinding "heroicons-outline" Map.empty
        // generateBinding "heroicons-solid" Map.empty
        // generateBinding "ic" Map.empty
        // generateBinding "logos" Map.empty

        generateBinding
            "mdi"
            (
                Map.ofList [
                    // Packages contains "1-2-3" and "123" has equivalent icons
                    "1-2-3", Skip
                    // Packages contains "a-b-c" and "abc" has equivalent icons
                    "a-b-c", Skip
                    // Packages contains "a-b-c-off" and "abc-off" has equivalent icons
                    "a-b-c-off", Skip
                ]
            )

        // generateBinding "simple-icons" Map.empty
        // generateBinding "tabler" Map.empty
        // generateBinding "vscode-icons" Map.empty
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

            let args =
                CmdLine.empty
                |> CmdLine.appendRaw "nuget"
                |> CmdLine.appendRaw "push"
                |> CmdLine.append nupkgFile
                |> CmdLine.appendPrefix "--source" "nuget.org"
                |> CmdLine.appendPrefix "--api-key" nugetKey
                |> CmdLine.toString

            run dotnet args cwd
    }

    BuildTask.runOrList ()
    0
