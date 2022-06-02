open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open BlackFox.Fake
open CLI
open BlackFox.CommandLine
open System
open System.Text.RegularExpressions
open System.IO
open IconifyIconsBindingGenerator.Generator


let testsPath = Path.getFullName "tests"
let srcPath = Path.getFullName "src"

let gitOwner = "glutinum-org"
let gitName = "Glutinum.Iconify"

let releasePackage (packageFolder : string) =
    let nugetKey =
        match Environment.environVarOrNone "nuget_key" with
        | Some nugetKey ->
            nugetKey

        | None ->
            failwith "nuget_key environment variable is not set"

    let changelogPath = packageFolder </> "CHANGELOG.md"
    let lastPublishedVersionPath = packageFolder </> "lastPublishedVersion.txt"
    let latestVersion = Changelog.findLatestVersion changelogPath

    // Does the package need to be released?
    let needRelease =
        if not(File.Exists lastPublishedVersionPath) then
            true
        else
            let lastPublishedVersion = File.ReadAllText lastPublishedVersionPath

            // Naive way to detect if we need to publish a new version
            latestVersion.ToString() <> lastPublishedVersion

    if not needRelease then
        printfn $"Package %s{packageFolder} is up to date, skipping release"
    else
        let stdout, _ =
            dotnet "pack -c Release" packageFolder
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
                // Make dotnet nuget not fail if the package and version already exist
                |> CmdLine.appendRaw "--skip-duplicate"
                |> CmdLine.toString

            File.WriteAllText(lastPublishedVersionPath, latestVersion.ToString())

    //        run dotnet args cwd
            ()

let iconifyIconsGeneratorReferences =
    [
        {
            IconifyIconPackageName = "academicons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "akar-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ant-design"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "arcticons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "bi"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "brandico"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "bx"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "bxl"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "bxs"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "bytesize"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "carbon"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "charm"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ci"
            Exceptions =
                Map.ofList [
                    // Packages contains "long-up-left-" and "long-up-left" has equivalent icons
                    "long-up-left-", Skip
                ]
        }
        {
            IconifyIconPackageName = "cib"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "cif"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "cil"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "circle-flags"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "clarity"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "codicon"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "cryptocurrency"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ei"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "emojione"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "emojione-monotone"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "emojione-v1"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "entypo-social"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "eos-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ep"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "eva"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fa-regular"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fa-solid"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fa6-brands"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fa6-regular"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fa6-solid"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fad"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fe"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "feather"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "file-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "flag"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "flagpack"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fluent"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fontisto"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "fxemoji"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "gala"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "geo"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "gg"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "gis"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "gridicons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "grommet-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "healthicons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "heroicons-outline"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "heroicons-solid"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ic"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "icon-park"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "icon-park-outline"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "icon-park-solid"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "icon-park-twotone"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "iconoir"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ion"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "jam"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "line-md"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "logos"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "lucide"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "majesticons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "maki"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "map"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "material-symbols"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "mdi"
            Exceptions =
                Map.ofList [
                    // Packages contains "1-2-3" and "123" has equivalent icons
                    "1-2-3", Skip
                    // Packages contains "a-b-c" and "abc" has equivalent icons
                    "a-b-c", Skip
                    // Packages contains "a-b-c-off" and "abc-off" has equivalent icons
                    "a-b-c-off", Skip
                ]
        }
        {
            IconifyIconPackageName = "mdi-light"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "medical-icon"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "mi"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "nimbus"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "noto"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "noto-v1"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "octicon"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ooui"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "openmoji"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "pepicons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ph"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "pixelarticons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "prime"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "quill"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "radix-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "ri"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "simple-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "system-uicons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "tabler"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "teenyicons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "twemoji"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "typcn"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "uil"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "uim"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "uis"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "uit"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "uiw"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "vscode-icons"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "wi"
            Exceptions = Map.empty
        }
        {
            IconifyIconPackageName = "zondicons"
            Exceptions = Map.empty
        }
    ]

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

    let refreshMdi = BuildTask.create "TestRefreshIcons" [] {
        generateBinding
            {
                IconifyIconPackageName = "ant-design"
                Exceptions = Map.empty
            }
    }

    let refreshIcons = BuildTask.create "RefreshIcons" [] {
        // Generate the bindings
        iconifyIconsGeneratorReferences
        |> Seq.iter generateBinding

        // Refresh the references table in README.md
        refreshReferencesTable iconifyIconsGeneratorReferences
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
        Directory.GetDirectories "packages"
        |> Seq.iteri (fun i path ->
            if i > 2 then
                failwith "Stop"
            else
                releasePackage path
        )
    }

    BuildTask.runOrList ()
    0
