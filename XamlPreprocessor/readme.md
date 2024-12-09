# XAML Preprocessor

A NuGet package that enables conditional compilation in XAML files using XML comments, similar to `#if` preprocessor directives in C# code.

## Usage

Add conditional sections in your XAML using XML comments:

```xml
<!-- directive:true -->
<Button Content="This shows when directive is defined"/>
<!-- end directive:true -->

<!-- directive:false -->
<Button Content="This shows when directive is NOT defined"/>
<!-- end directive:false -->
```

Define your constants through:

- Windows command line cmd terminal: `set "DefineConstants=%DefineConstants%;directive1;directive2"`
- Or MSBuild command line: `/p:DefineConstants="%DefineConstants%;directive1;directive2"`
- Or project `.csproj` file:

```xml
<PropertyGroup>
  <DefineConstants>$(DefineConstants);directive1;directive2</DefineConstants>
</PropertyGroup>
```

## Known Issues

- Currently modifies source XAML files directly (will be fixed in future versions)
