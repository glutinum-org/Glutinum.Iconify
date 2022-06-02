module Changelog

open Ionide.KeepAChangelog
open Ionide.KeepAChangelog.Domain
open System.IO
open System.Linq

let findEntry (changelogPath : string) (version : SemVersion.SemanticVersion) =
    if not(File.Exists changelogPath) then
        failwith "Changelog file not found at: %s{changelogPath}"
    else

        let changelogFile =
            FileInfo changelogPath

        match Parser.parseChangeLog changelogFile with
        | Error (formatted, msg) ->
            failwith $"Error parsing Changelog at {changelogFile.FullName}. The error occurred at {msg.Position}.{System.Environment.NewLine}{formatted}"

        | Ok changelog ->
            let changelogEntry =
                changelog.Releases
                |> Seq.tryFind (fun (v, _, _) ->
                    v = version
                )

            match changelogEntry with
            | Some changelogEntry ->
                changelogEntry

            | None ->
                failwith "No version found in Changelog"

let findLatestVersion changelogPath =
    if not(File.Exists changelogPath) then
        failwith "Changelog file not found at: %s{changelogPath}"
    else

        let changelogFile =
            FileInfo changelogPath

        match Parser.parseChangeLog changelogFile with
        | Error (formatted, msg) ->
            failwith $"Error parsing Changelog at {changelogFile.FullName}. The error occurred at {msg.Position}.{System.Environment.NewLine}{formatted}"

        | Ok changelogs ->
            let sortedReleases =
                // have to use LINQ here because List.sortBy* require IComparable, which
                // semver doesn't implement
                changelogs.Releases.OrderByDescending(fun (v, _, _) -> v)

            // Try to extract the first element of the sequence
            // Note: It is possible to not have a version yet
            // that's why we return an Option
            Seq.tryHead sortedReleases
            |> Option.map (fun (v, _, _) -> v)

module Util =

    let allReleaseNotesFor (data: ChangelogData) =
        let section name items =
            match items with
            | [] -> []
            | items -> $"### {name}" :: items @ [ "" ]

        String.concat
            System.Environment.NewLine
            [
                yield! section "Added" data.Added
                yield! section "Changed" data.Changed
                yield! section "Deprecated" data.Deprecated
                yield! section "Removed" data.Removed
                yield! section "Fixed" data.Fixed
                yield! section "Security" data.Security
                for KeyValue(heading, lines) in data.Custom do
                    yield! section heading lines
            ]
