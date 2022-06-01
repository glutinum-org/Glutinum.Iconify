# Glutinum.Iconify

Bindings for [iconify](https://iconify.design/) ecosystem.

## Usage

## Contributing

### Icon binding generator

The icon bindings are done using a generator.

For each `.d.ts` files, in the npm package, it will generate a `static member` that has been sanitized will the following rules:

- camelize
- Prefix with `_` if the icon name starts with a number
- Suffix with `_` if the icon name is an F# keyword

If the binding destination doesn't exist, the generator will **scaffold a new** binding project otherwise it will just **update** the `Type.fs` file.

Example:

```bash
node_modules
└───@iconify-icons
    └───ant-design
        ├───account-book-fill.d.ts
        ├───account-book-fill.js
        ├───account-book-filled.d.ts
        └───account-book-filled.js
```

gives

```fs
namespace Glutinum.IconifyIcons.AntDesign

open Fable.Core
open Glutinum.Iconify

[<Erase>]
type antDesign =

    [<Import("default", "@iconify-icons/antDesign/account-book-fill")>]
    static member inline accountBookFill : IconifyIcon =
        jsNative

    [<Import("default", "@iconify-icons/antDesign/account-book-filled")>]
    static member inline accountBookFilled : IconifyIcon =
        jsNative
```

#### Add new icon sets

1. Install the npm package:

    `npm i -D @iconify-icons/mdi`

2. In `Build.fs` add the instruction for your icon set to he `RefreshIcons` task.

```fs
    let refreshIcons = BuildTask.create "RefreshIcons" [] {
        // ...
        generateBinding "mdi" Map.empty
        // ...
    }
```

3. Run the generation a first time

    `.\build.cmd RefreshIcons`

    This will initialize a new project and generate the binding.

4. Check that the binding compiles

    `dotnet build ./packages/Glutinum.IconifyIcons.Mdi/`

6. If it compiles then everything is good and the binding is ready to be published.

#### Generator exceptions rules

Sometimes, you will need to fix or improve the default rule of the generator.

In order, to archieve that you can pass a `Map` of rules.

- `Skip`: Don't generate an instruction for this icon
- `ForceName`: Force the generated name for this icon

For example, in the case of `@iconify-icons/mdi` some icons are included twice under different names that result in the same generated name.

Using the exception rules allow us to only output the icon once.

```fs
    let refreshIcons = BuildTask.create "RefreshIcons" [] {
        generateBinding
            "mdi"
            (
                Map.ofList [
                    // Packages contains "1-2-3" and "123" has equivalent icons
                    "1-2-3", Skip
                    // Packages contains "a-b-c" and "abc" has equivalent icons
                    "a-b-c", Skip
                    // Packages contains "a-b-c-off" and "abc-off" has equivalent icons
                    "a-b-c-off", Skip
                ]
            )
    }
```
