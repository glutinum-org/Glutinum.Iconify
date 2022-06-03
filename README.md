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
| `Glutinum.IconifyIcons.Academicons` | `@iconify-icons/academicons` |
| `Glutinum.IconifyIcons.AkarIcons` | `@iconify-icons/akar-icons` |
| `Glutinum.IconifyIcons.AntDesign` | `@iconify-icons/ant-design` |
| `Glutinum.IconifyIcons.Arcticons` | `@iconify-icons/arcticons` |
| `Glutinum.IconifyIcons.Bi` | `@iconify-icons/bi` |
| `Glutinum.IconifyIcons.Brandico` | `@iconify-icons/brandico` |
| `Glutinum.IconifyIcons.Bx` | `@iconify-icons/bx` |
| `Glutinum.IconifyIcons.Bxl` | `@iconify-icons/bxl` |
| `Glutinum.IconifyIcons.Bxs` | `@iconify-icons/bxs` |
| `Glutinum.IconifyIcons.Bytesize` | `@iconify-icons/bytesize` |
| `Glutinum.IconifyIcons.Carbon` | `@iconify-icons/carbon` |
| `Glutinum.IconifyIcons.Charm` | `@iconify-icons/charm` |
| `Glutinum.IconifyIcons.Ci` | `@iconify-icons/ci` |
| `Glutinum.IconifyIcons.Cib` | `@iconify-icons/cib` |
| `Glutinum.IconifyIcons.Cif` | `@iconify-icons/cif` |
| `Glutinum.IconifyIcons.Cil` | `@iconify-icons/cil` |
| `Glutinum.IconifyIcons.CircleFlags` | `@iconify-icons/circle-flags` |
| `Glutinum.IconifyIcons.Clarity` | `@iconify-icons/clarity` |
| `Glutinum.IconifyIcons.Codicon` | `@iconify-icons/codicon` |
| `Glutinum.IconifyIcons.Cryptocurrency` | `@iconify-icons/cryptocurrency` |
| `Glutinum.IconifyIcons.Ei` | `@iconify-icons/ei` |
| `Glutinum.IconifyIcons.Emojione` | `@iconify-icons/emojione` |
| `Glutinum.IconifyIcons.EmojioneMonotone` | `@iconify-icons/emojione-monotone` |
| `Glutinum.IconifyIcons.EmojioneV1` | `@iconify-icons/emojione-v1` |
| `Glutinum.IconifyIcons.EntypoSocial` | `@iconify-icons/entypo-social` |
| `Glutinum.IconifyIcons.EosIcons` | `@iconify-icons/eos-icons` |
| `Glutinum.IconifyIcons.Ep` | `@iconify-icons/ep` |
| `Glutinum.IconifyIcons.Eva` | `@iconify-icons/eva` |
| `Glutinum.IconifyIcons.FaRegular` | `@iconify-icons/fa-regular` |
| `Glutinum.IconifyIcons.FaSolid` | `@iconify-icons/fa-solid` |
| `Glutinum.IconifyIcons.Fa6Brands` | `@iconify-icons/fa6-brands` |
| `Glutinum.IconifyIcons.Fa6Regular` | `@iconify-icons/fa6-regular` |
| `Glutinum.IconifyIcons.Fa6Solid` | `@iconify-icons/fa6-solid` |
| `Glutinum.IconifyIcons.Fad` | `@iconify-icons/fad` |
| `Glutinum.IconifyIcons.Fe` | `@iconify-icons/fe` |
| `Glutinum.IconifyIcons.Feather` | `@iconify-icons/feather` |
| `Glutinum.IconifyIcons.FileIcons` | `@iconify-icons/file-icons` |
| `Glutinum.IconifyIcons.Flag` | `@iconify-icons/flag` |
| `Glutinum.IconifyIcons.Flagpack` | `@iconify-icons/flagpack` |
| `Glutinum.IconifyIcons.Fluent` | `@iconify-icons/fluent` |
| `Glutinum.IconifyIcons.Fontisto` | `@iconify-icons/fontisto` |
| `Glutinum.IconifyIcons.Fxemoji` | `@iconify-icons/fxemoji` |
| `Glutinum.IconifyIcons.Gala` | `@iconify-icons/gala` |
| `Glutinum.IconifyIcons.Geo` | `@iconify-icons/geo` |
| `Glutinum.IconifyIcons.Gg` | `@iconify-icons/gg` |
| `Glutinum.IconifyIcons.Gis` | `@iconify-icons/gis` |
| `Glutinum.IconifyIcons.Gridicons` | `@iconify-icons/gridicons` |
| `Glutinum.IconifyIcons.GrommetIcons` | `@iconify-icons/grommet-icons` |
| `Glutinum.IconifyIcons.Healthicons` | `@iconify-icons/healthicons` |
| `Glutinum.IconifyIcons.HeroiconsOutline` | `@iconify-icons/heroicons-outline` |
| `Glutinum.IconifyIcons.HeroiconsSolid` | `@iconify-icons/heroicons-solid` |
| `Glutinum.IconifyIcons.Ic` | `@iconify-icons/ic` |
| `Glutinum.IconifyIcons.IconPark` | `@iconify-icons/icon-park` |
| `Glutinum.IconifyIcons.IconParkOutline` | `@iconify-icons/icon-park-outline` |
| `Glutinum.IconifyIcons.IconParkSolid` | `@iconify-icons/icon-park-solid` |
| `Glutinum.IconifyIcons.IconParkTwotone` | `@iconify-icons/icon-park-twotone` |
| `Glutinum.IconifyIcons.Iconoir` | `@iconify-icons/iconoir` |
| `Glutinum.IconifyIcons.Ion` | `@iconify-icons/ion` |
| `Glutinum.IconifyIcons.Jam` | `@iconify-icons/jam` |
| `Glutinum.IconifyIcons.LineMd` | `@iconify-icons/line-md` |
| `Glutinum.IconifyIcons.Logos` | `@iconify-icons/logos` |
| `Glutinum.IconifyIcons.Lucide` | `@iconify-icons/lucide` |
| `Glutinum.IconifyIcons.Majesticons` | `@iconify-icons/majesticons` |
| `Glutinum.IconifyIcons.Maki` | `@iconify-icons/maki` |
| `Glutinum.IconifyIcons.Map` | `@iconify-icons/map` |
| `Glutinum.IconifyIcons.MaterialSymbols` | `@iconify-icons/material-symbols` |
| `Glutinum.IconifyIcons.Mdi` | `@iconify-icons/mdi` |
| `Glutinum.IconifyIcons.MdiLight` | `@iconify-icons/mdi-light` |
| `Glutinum.IconifyIcons.MedicalIcon` | `@iconify-icons/medical-icon` |
| `Glutinum.IconifyIcons.Mi` | `@iconify-icons/mi` |
| `Glutinum.IconifyIcons.Nimbus` | `@iconify-icons/nimbus` |
| `Glutinum.IconifyIcons.Noto` | `@iconify-icons/noto` |
| `Glutinum.IconifyIcons.NotoV1` | `@iconify-icons/noto-v1` |
| `Glutinum.IconifyIcons.Octicon` | `@iconify-icons/octicon` |
| `Glutinum.IconifyIcons.Ooui` | `@iconify-icons/ooui` |
| `Glutinum.IconifyIcons.Openmoji` | `@iconify-icons/openmoji` |
| `Glutinum.IconifyIcons.Pepicons` | `@iconify-icons/pepicons` |
| `Glutinum.IconifyIcons.Ph` | `@iconify-icons/ph` |
| `Glutinum.IconifyIcons.Pixelarticons` | `@iconify-icons/pixelarticons` |
| `Glutinum.IconifyIcons.Prime` | `@iconify-icons/prime` |
| `Glutinum.IconifyIcons.Quill` | `@iconify-icons/quill` |
| `Glutinum.IconifyIcons.RadixIcons` | `@iconify-icons/radix-icons` |
| `Glutinum.IconifyIcons.Ri` | `@iconify-icons/ri` |
| `Glutinum.IconifyIcons.SimpleIcons` | `@iconify-icons/simple-icons` |
| `Glutinum.IconifyIcons.SystemUicons` | `@iconify-icons/system-uicons` |
| `Glutinum.IconifyIcons.Tabler` | `@iconify-icons/tabler` |
| `Glutinum.IconifyIcons.Teenyicons` | `@iconify-icons/teenyicons` |
| `Glutinum.IconifyIcons.Twemoji` | `@iconify-icons/twemoji` |
| `Glutinum.IconifyIcons.Typcn` | `@iconify-icons/typcn` |
| `Glutinum.IconifyIcons.Uil` | `@iconify-icons/uil` |
| `Glutinum.IconifyIcons.Uim` | `@iconify-icons/uim` |
| `Glutinum.IconifyIcons.Uis` | `@iconify-icons/uis` |
| `Glutinum.IconifyIcons.Uit` | `@iconify-icons/uit` |
| `Glutinum.IconifyIcons.Uiw` | `@iconify-icons/uiw` |
| `Glutinum.IconifyIcons.VscodeIcons` | `@iconify-icons/vscode-icons` |
| `Glutinum.IconifyIcons.Wi` | `@iconify-icons/wi` |
| `Glutinum.IconifyIcons.Zondicons` | `@iconify-icons/zondicons` |
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
