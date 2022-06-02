# Glutinum.Iconify

Bindings for [iconify](https://iconify.design/) ecosystem.

## Usage

⚠️ Only the offline API using React is supported right now

If you need, to use the online API or another framework (Vue, WebComponent, etc.) supported by Iconify please open issue for discussion.

### Core API

#### Custom icons

If needed you can create a custom icons like that:

```fs
// Creation of a custom icon
let triangleRightIcon =
    IconifyIcon(
        body = "<path d=\"M7 6v12l10-6z\" fill=\"currentColor\"/>",
        width = 26.,
        height = 26.
    )

// Example of consuming it
Icon [
    icon.icon triangleRightIcon
]
```

```f#
// Creation of a custom icon
let triangleRightIcon =
    IconifyIcon(
        body = "<path d=\"M7 6v12l10-6z\" fill=\"currentColor\"/>",
        width = 26.,
        height = 26.
    )

// Example of consuming it
Icon [
    icon.icon triangleRightIcon
]
```

### React

#### Installation

```
# using nuget
dotnet add package Glutinum.Feliz.Iconify

# or with paket
paket add Glutinum.Feliz.Iconify --project /path/to/project.fsproj
```

You also need to install fuse.js package.

```
# using Femto
dotnet femto --resolve

# using NPM
npm install @iconify/react

# using yarn
yarn add @iconify/react
```

Before being able to consume icons, you also need to install the specific binding for the icon set you want to use.

<!-- Begin:binding_reference_table -->
| Nuget                   | Npm      |
|-------------------------|----------|
| Glutinum.IconifyIcons.* | Generated |
<!-- End:binding_reference_table -->

#### Offline

```fs
open Feliz

// Access to the icon properties
open Feliz.Iconify
// Access to the Icon React component for Offline usage
open type Feliz.Iconify.Offline.Exports
// Access to the antDesign list of icon
open Glutinum.IconifyIcons.AntDesign

[<ReactComponent>]
let private MyComponent () =
    Icon [
        icon.icon antDesign.antCloud
    ]
```

## Contributing

This section is only for the maintainers and contributors of the project.

### How the bindings are written

| Binding                 | Manual/Generated |
|-------------------------|------------------|
| Glutinum.Iconify        | Manual           |
| Glutinum.Feliz.Iconify  | Manual           |
| Glutinum.IconifyIcons.* | Generated        |

If a binding is generated, it means that you should not edit it manually.
You should use the icon binding generator which will be responsible for
generating the binding code and changelog for you.

In case of manual binding, you can edit it as a normal binding and add entry to the changelog.

### Icon binding generator for `Glutinum.IconifyIcons.*`

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

2. In `Build.fs` add the instruction for your icon set to the `iconifyIconsGeneratorReferences` list.

```fs
let iconifyIconsGeneratorReferences =
    [
        // ...
        {
            IconifyIconPackageName = "akar-icons"
            Exceptions = Map.empty
        }
        // ...
    ]
```

3. Run the generation a first time

    `.\build.cmd RefreshIcons`

    This will initialize a new project and generate the binding.

4. Check that the binding compiles

    `dotnet build ./packages/Glutinum.IconifyIcons.Mdi/`

6. If it compiles then everything is good and the binding is ready to be published.

#### Generator exceptions rules

Sometimes, you will need to fix or improve the default rule of the generator.

In order, to do that you can pass a `Map` of rules.

- `Skip`: Don't generate an instruction for this icon
- `ForceName`: Force the generated name for this icon

For example, in the case of `@iconify-icons/mdi` some icons are included twice under different names that result in the same generated name.

Using the exception rules allow us to only output the icon once.

```fs
{
    IconifyIconPackageName = "mdi"
    Exceptions =
        Map.ofList [
            // Packages contains "1-2-3" and "123" has equivalent icons
            "1-2-3", Skip
            // Packages contains "a-b-c" and "abc" has equivalent icons
            "a-b-c", Skip
            // Packages contains "a-b-c-off" and "abc-off" has equivalent icons
            "a-b-c-off", Skip
        ]
}
```
