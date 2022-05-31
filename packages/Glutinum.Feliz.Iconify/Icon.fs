namespace Feliz.Iconify

open Feliz
open Fable.Core
open Fable.Core.JsInterop
open Glutinum.Iconify

[<StringEnum>]
[<RequireQualifiedAccess>]
type IconifyHorizontalIconAlignment =
    | Left
    | Center
    | Right

[<StringEnum>]
[<RequireQualifiedAccess>]
type IconifyVerticalIconAlignment =
    | Top
    | Middle
    | Bottom

[<Erase>]
type icon =

    static member inline icon (icon : string) =
        Interop.svgAttribute "icon" icon

    static member inline icon (icon : IconifyIcon) =
        Interop.svgAttribute "icon" icon

    static member inline color (color : string) =
        Interop.svgAttribute "color" color

    static member inline flip (value : string) =
        Interop.svgAttribute "flip" value

    static member inline align (value : string) =
        Interop.svgAttribute "align" value

    static member inline id (id : string) =
        Interop.svgAttribute "id" id

    static member inline onLoad (func : string -> unit) =
        Interop.svgAttribute "onLoad" func

    static member inline rotate (value : float) =
        Interop.svgAttribute "rotate" value

    static member inline rotate (value : string) =
        Interop.svgAttribute "rotate" value

    static member inline inline_ (value : bool) =
         Interop.svgAttribute "inline" value

    static member inline width (value : float) =
        Interop.svgAttribute "width" value

    static member inline width (value : string) =
        Interop.svgAttribute "width" value

    static member inline height (value : float) =
        Interop.svgAttribute "height" value

    static member inline height (value : string) =
        Interop.svgAttribute "height" value

    static member inline hAlign (value : IconifyHorizontalIconAlignment) =
        Interop.svgAttribute "hAlign" value

    static member inline vAlign (value : IconifyVerticalIconAlignment) =
        Interop.svgAttribute "vAlign" value

    static member inline sclice (value : bool) =
        Interop.svgAttribute "sclice" value

    static member inline hFlip (value : bool) =
        Interop.svgAttribute "hFlip" value

    static member inline vFlip (value : bool) =
        Interop.svgAttribute "vFlip" value

    static member inline ref (ref : IRefValue<Browser.Types.SVGElement>) =
        Interop.svgAttribute "ref" ref
