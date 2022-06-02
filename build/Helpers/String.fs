module String

open System.Text.RegularExpressions

let remove (extension : string) (value : string) =
    value.Replace(extension, "")

let sanitizeIfStartWithNumber (value : string) =
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

let capitalize (input : string) =
    if input.Length > 0 then
        (string input[0]).ToLower() + input.Substring(1)
    else
        input

let camelize (input : string) =
    let word = pascalize input
    if word.Length > 0 then
        (string word[0]).ToLower() + word.Substring(1)
    else
        word

let sanitizeKeyword (input : string) =
    if Set.contains input FSharp.Keywords.keywords then
        input + "_"
    else
        input
