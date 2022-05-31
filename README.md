# Glutinum.Feliz.ReactResizeDetector

Binding for [react-resize-detector](https://www.npmjs.com/package/react-resize-detector)

## Usage

### React hook

```fs
open Feliz

open type Feliz.ReactResizeDetector.Exports

[<ReactComponent>]
let CustomComponent () =
    let resizeDetector = useResizeDetector()

    Html.div [
        prop.ref resizeDetector.ref

        prop.text $"{resizeDetector.width}x{resizeDetector.height}"
    ]
```

If you need to pass some options to the hooks:

```fs
open Feliz
open Feliz.ReactResizeDetector

open type Feliz.ReactResizeDetector.Exports

[<ReactComponent>]
let CustomComponent () =
    let resizeDetector =
        useResizeDetector(
            FunctionProps(
                handleWidth = false,
                refreshMode = RefreshMode.Throttle,
                refreshRate = 500
            )
        )

    Html.div [
        prop.ref resizeDetector.ref

        prop.text $"{resizeDetector.width}x{resizeDetector.height}"
    ]
```

### Child Function Pattern

```fs
open Feliz
open ReactResizeDetector

open type ReactResizeDetector.Exports

reactResizeDetector [
    reactResizeDetector.handleWidth
    reactResizeDetector.handleHeight

    reactResizeDetector.children
        (fun props ->
            Html.div $"Width: {props.width}, Height: {props.height}"
        )
]
```
