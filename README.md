# XAML Preprocessor

A preprocessor for WPF XAML files that enables conditional compilation using XML comments, providing functionality similar to C#'s #if preprocessor directives.

## Features

- Use conditional compilation in XAML files
- Compatible with existing DefineConstants from C# projects
- Integrates with MSBuild process
- Supports both true/false conditions

## Usage Example

```xml
<!-- fix_issue_001:true -->
<Button Content="New Implementation"/>
<!-- end fix_issue_001:true -->

<!-- fix_issue_001:false -->
<Button Content="Old Implementation"/>
<!-- end fix_issue_001:false -->
```

## Installation

Install via NuGet Package Manager:

```powershell
Install-Package XamlPreprocessor
```

## Configuration

Define your constants through:

1. MSBuild command line:

```batch
msbuild.exe /p:DefineConstants="fix_issue_001;feature_002"
```

2. Or project file (`.csproj`):

```xml
<PropertyGroup>
  <DefineConstants>$(DefineConstants);fix_issue_001;feature_002</DefineConstants>
</PropertyGroup>
```

## Known Issues & Limitations

- Currently modifies source XAML files directly instead of intercepting the build process (to be fixed)
- Requires .NET Framework 4.7.2 or later
- Only works with WPF projects

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License
