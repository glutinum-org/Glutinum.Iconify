module Demo.Hook

open Feliz
open Browser.Dom
open Fable.Core.JsInterop
open Feliz.Iconify
open Glutinum.IconifyIcons.Mdi
open Glutinum.IconifyIcons.AntDesign

open type Feliz.Iconify.Offline.Exports

// Workaround to have React-refresh working
// I need to open an issue on react-refresh to see if they can improve the detection
emitJsStatement () "import React from \"react\""

importSideEffects "./index.scss"


[<ReactComponent>]
let private Component () =
    let isLeftPanelVisible, setLeftPanelVisibility = React.useState true

    Html.div [
        prop.className "wrapper"

        prop.children [
            Icon [
                icon.icon antDesign.antCloud
            ]
        ]
    ]

ReactDOM.render(
    Component ()
    ,
    document.getElementById("root")
)
