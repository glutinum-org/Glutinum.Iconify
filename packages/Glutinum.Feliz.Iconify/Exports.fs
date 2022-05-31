namespace Feliz.Iconify

open Feliz
open Fable.Core
open Fable.Core.JsInterop

module Offline =

    [<Erase>]
    type Exports =

        static member inline Icon (properties : #ISvgAttribute  list) =
            Interop.reactApi.createElement(import "Icon" "@iconify/react/dist/offline", createObj !!properties)

        // InlineIcon



        // /**
        // * Add collection to storage, allowing to call icons by name
        // *
        // * @param data Icon set
        // * @param prefix Optional prefix to add to icon names, true (default) if prefix from icon set should be used.
        // */
        // export declare function addCollection(data: IconifyJSON, prefix?: string | boolean): void;

        // /**
        // * Add icon to storage, allowing to call it by name
        // *
        // * @param name
        // * @param data
        // */
        // export declare function addIcon(name: string, data: IconifyIcon): void;
