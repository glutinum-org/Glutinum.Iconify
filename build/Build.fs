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

let testsPath = Path.getFullName "tests"
let srcPath = Path.getFullName "src"

let gitOwner = "glutinum-org"
let gitName = "Glutinum.Feliz.Iconify"

[<EntryPoint>]
let main args =
    BuildTask.setupContextFromArgv args

    let clean = BuildTask.create "Clean" [] {
        [
            testsPath </> "bin"
            testsPath </> "obj"
            srcPath </> "bin"
            srcPath </> "obj"
        ]
        |> Shell.cleanDirs

        !! (Glob.fableJs testsPath)
        ++ (Glob.fableJs srcPath)
        |> Seq.iter Shell.rm
    }

    let npmInstall = BuildTask.create "NpmInstall" [] {
        run npm "install" cwd
    }

    let refreshIcons = BuildTask.create "RefreshIcons" [] {
        generateBinding "academicons" Map.empty
        generateBinding "akar-icons" Map.empty
        generateBinding "ant-design" Map.empty
        generateBinding "arcticons" Map.empty
        generateBinding "bi" Map.empty
        generateBinding "brandico" Map.empty
        generateBinding "bx" Map.empty
        generateBinding "bxl" Map.empty
        generateBinding "bxs" Map.empty
        generateBinding "bytesize" Map.empty
        generateBinding "carbon" Map.empty
        generateBinding "charm" Map.empty
        generateBinding "ci" Map.empty
        generateBinding "cib" Map.empty
        generateBinding "cif" Map.empty
        generateBinding "cil" Map.empty
        generateBinding "circle-flags" Map.empty
        generateBinding "clarity" Map.empty
        generateBinding "codicon" Map.empty
        generateBinding "cryptocurrency" Map.empty
        generateBinding "ei" Map.empty
        generateBinding "emojione" Map.empty
        generateBinding "emojione-monotone" Map.empty
        generateBinding "emojione-v1" Map.empty
        generateBinding "entypo-social" Map.empty
        generateBinding "eos-icons" Map.empty
        generateBinding "ep" Map.empty
        generateBinding "eva" Map.empty
        generateBinding "fa-regular" Map.empty
        generateBinding "fa-solid" Map.empty
        generateBinding "fa6-brands" Map.empty
        generateBinding "fa6-regular" Map.empty
        generateBinding "fa6-solid" Map.empty
        generateBinding "fad" Map.empty
        generateBinding "fe" Map.empty
        generateBinding "feather" Map.empty
        generateBinding "file-icons" Map.empty
        generateBinding "flag" Map.empty
        generateBinding "flagpack" Map.empty
        generateBinding "fluent" Map.empty
        generateBinding "fontisto" Map.empty
        generateBinding "fxemoji" Map.empty
        generateBinding "gala" Map.empty
        generateBinding "geo" Map.empty
        generateBinding "gg" Map.empty
        generateBinding "gis" Map.empty
        generateBinding "gridicons" Map.empty
        generateBinding "grommet-icons" Map.empty
        generateBinding "healthicons" Map.empty
        generateBinding "heroicons-outline" Map.empty
        generateBinding "heroicons-solid" Map.empty
        generateBinding "ic" Map.empty
        generateBinding "icon-park" Map.empty
        generateBinding "icon-park-outline" Map.empty
        generateBinding "icon-park-solid" Map.empty
        generateBinding "icon-park-twotone" Map.empty
        generateBinding "iconoir" Map.empty
        generateBinding "ion" Map.empty
        generateBinding "jam" Map.empty
        generateBinding "line-md" Map.empty
        generateBinding "logos" Map.empty
        generateBinding "lucide" Map.empty
        generateBinding "majesticons" Map.empty
        generateBinding "maki" Map.empty
        generateBinding "map" Map.empty
        generateBinding "material-symbols" Map.empty

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

        generateBinding "mdi-light" Map.empty
        generateBinding "medical-icon" Map.empty
        generateBinding "mi" Map.empty
        generateBinding "nimbus" Map.empty
        generateBinding "noto" Map.empty
        generateBinding "noto-v1" Map.empty
        generateBinding "octicon" Map.empty
        generateBinding "ooui" Map.empty
        generateBinding "openmoji" Map.empty
        generateBinding "pepicons" Map.empty
        generateBinding "ph" Map.empty
        generateBinding "pixelarticons" Map.empty
        generateBinding "prime" Map.empty
        generateBinding "quill" Map.empty
        generateBinding "radix-icons" Map.empty
        generateBinding "ri" Map.empty
        generateBinding "simple-icons" Map.empty
        generateBinding "system-uicons" Map.empty
        generateBinding "tabler" Map.empty
        generateBinding "teenyicons" Map.empty
        generateBinding "twemoji" Map.empty
        generateBinding "typcn" Map.empty
        generateBinding "uil" Map.empty
        generateBinding "uim" Map.empty
        generateBinding "uis" Map.empty
        generateBinding "uit" Map.empty
        generateBinding "uiw" Map.empty
        generateBinding "vscode-icons" Map.empty
        generateBinding "wi" Map.empty
        generateBinding "zondicons" Map.empty
    }

    let watchDemo = BuildTask.create "Watch" [ npmInstall; clean ] {
        // All for graceful shutdown on Ctrl+C while the processes are running
        Console.CancelKeyPress.AddHandler(fun _ ea ->
            ea.Cancel <- true
            printfn "Received Ctrl+C, shutting down..."
            Environment.Exit(0)
        )

        [
            "vite", npx "vite dev" testsPath
            "fable", dotnet "fable --watch" testsPath
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
