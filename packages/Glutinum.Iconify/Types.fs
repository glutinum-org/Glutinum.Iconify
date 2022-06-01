namespace Glutinum.Iconify

open Fable.Core

[<Global>]
type IconifyIcon
    [<ParamObject; Emit "$0">]
    (
        ?body : string,
        ?left : float,
        ?top : float,
        ?width : float,
        ?height : float,
        ?rotate : float,
        ?hFlip : bool,
        ?vFlip : bool
    )
    =

    /// <summary>
    /// Icon body: &lt;path d="..." /&gt;, required.
    /// </summary>
    member val body : string option = jsNative with get, set

    /// <summary>
    /// Left position of viewBox.
    /// Defaults to 0.
    /// </summary>
    member val left : float option = jsNative with get, set

    /// <summary>
    /// Top position of viewBox.
    /// Defaults to 0.
    /// </summary>
    member val top : float option = jsNative with get, set

    /// <summary>
    /// Width of viewBox.
    /// Defaults to 16.
    /// </summary>
    member val width : float option = jsNative with get, set

    /// <summary>
    /// Height of viewBox.
    /// Defaults to 16.
    /// </summary>
    member val height : float option = jsNative with get, set

    /// <summary>
    /// Number of 90 degrees rotations.
    /// 0 = 0, 1 = 90deg and so on.
    /// Defaults to 0.
    /// When merged (such as alias + icon), result is icon.rotation + alias.rotation.
    /// </summary>
    member val rotate : float option = jsNative with get, set

    /// <summary>
    /// Horizontal flip.
    /// Defaults to false.
    /// When merged, result is icon.hFlip !== alias.hFlip
    /// </summary>
    member val hFlip : bool option = jsNative with get, set

    /// <summary>
    /// Vertical flip. (see hFlip comments)
    /// </summary>
    member val vFlip : bool option = jsNative with get, set
