module IconifyIconsBindingGenerator.Templates

let changelog =
    """# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased
"""

let changelogEntry
    (fsharpPackageVersion : string)
    (npmPackageName : string)
    (npmPackageVersion : string) =
    $"""## %s{fsharpPackageVersion}

### Changed

* Updated binding to %s{npmPackageName}@%s{npmPackageVersion}
"""

let paketReferences =
    """FSharp.Core
Fable.Core

Ionide.KeepAChangelog.Tasks
Microsoft.SourceLink.GitHub
"""

let projectFsproj (npmPackageName : string) =
    $"""<?xml version="1.0" encoding="utf-8"?>
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <NpmDependencies>
      <NpmPackage Name="%s{npmPackageName}" Version="0.0.0" ResolutionStrategy="Max" />
    </NpmDependencies>
  </PropertyGroup>
  <ItemGroup>
    <Compile Include="Types.fs" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Glutinum.Iconify\Glutinum.Iconify.fsproj" />
  </ItemGroup>
  <Import Project="..\..\.paket\Paket.Restore.targets" />
</Project>
"""
