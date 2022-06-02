module Demo.Hook

open Feliz
open Browser.Dom
open Fable.Core.JsInterop
open Feliz.Iconify
open Glutinum.Iconify

open type Feliz.Iconify.Offline.Exports

// Workaround to have React-refresh working
// I need to open an issue on react-refresh to see if they can improve the detection
emitJsStatement () "import React from \"react\""

importSideEffects "./index.scss"

let triangleRightIcon =
    IconifyIcon(
        body = "<path d=\"M7 6v12l10-6z\" fill=\"currentColor\"/>",
        width = 26.,
        height = 26.
    )

[<ReactComponent>]
let private Component () =
    Html.div [
        prop.className "wrapper"

        prop.children [
            Icon [
                icon.icon triangleRightIcon
            ]
        ]
    ]

ReactDOM.render(
    Component ()
    ,
    document.getElementById("root")
)
