# XAML Preprocessor

A preprocessor for WPF XAML files that enables conditional compilation using XML comments, providing functionality similar to C#'s `#if...#endif` preprocessor directives.

## Features

- Use conditional compilation in XAML files
- Compatible with existing DefineConstants from C# projects
- Integrates with MSBuild process
- Supports both true/false conditions
- Preserves source files (processes intermediate copies)
- Full UTF-8 with BOM support for XAML compatibility

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

## How It Works

1. During build, creates preprocessed copies of XAML files in the intermediate directory
2. Applies XSLT transformation based on defined constants
3. Uses preprocessed copies for compilation while preserving source files
4. Handles proper UTF-8 encoding with BOM for XAML compatibility

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License
