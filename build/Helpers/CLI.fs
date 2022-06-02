module CLI

open Fake.Core
open BlackFox

let cwd = System.Environment.CurrentDirectory

let createProcess exe arg dir =
    CreateProcess.fromRawCommandLine exe arg
    |> CreateProcess.withWorkingDirectory dir
    |> CreateProcess.ensureExitCode

let run proc arg dir =
    proc arg dir
    |> Proc.run
    |> ignore

let runParallel processes =
    processes
    |> Proc.Parallel.run
    |> ignore


let npm =
    match PathEnvironment.findExecutable "npm" false with
    | None ->
        failwith "npm was not found in path. Please install it and make sure it's available from your path."
    | Some npm ->
        createProcess npm

let dotnet = createProcess "dotnet"

let npx =
    match PathEnvironment.findExecutable "npx" false with
    | None ->
        failwith "npx was not found in path. Please install it and make sure it's available from your path."
    | Some npx ->
        createProcess npx

let git = createProcess "git"
