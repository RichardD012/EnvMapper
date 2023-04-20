# EnvMapper

[![EnvMapper Build](https://github.com/RichardD012/EnvMapper/actions/workflows/main-build.yml/badge.svg)](https://github.com/RichardD012/EnvMapper/actions/workflows/main-build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![NuGet version](https://badge.fury.io/nu/EnvMapper.svg)](https://www.nuget.org/packages/EnvMapper)

A .NET library to map environment variables to a C# object. This is useful when deploying to containerized environments where environment variables may be injected before starting up.

## Installation

Available on [NuGet](https://www.nuget.org/packages/EnvMapper/)

Visual Studio:

```powershell
PM> Install-Package EnvMapper
```

.NET Core CLI:

```bash
dotnet add package EnvMapper
```

## Usage

### Load env file

`MapConfiguration()` will automatically search for environment variables and map them to a class decorated with `System.Runtime.Serialization.DataMember` attributes. This supports overriding using the `Name` and `IsRequired` properties of the `DataMemberAttribute`.

```csharp
var config = EnvMapper.Env.MapConfiguration<TType>();
```

If properties that are marked as required are not found, a `EnvMapperException` will be thrown, which contains a list of missing properties.

## Example

Given the following example class:

```csharp
[DataContract]
public class TestConfiguration
{
    [DataMember(Name="Foo", IsRequired = true)]
    public string? Foo { get; set; }

    [DataMember]
    public string? Bar { get; set; }

    [DataMember(Name="Override")]
    public string? OtherField { get; set; }
}
```

The following can be used to access the variables that are present as environment variables:

```csharp
var config = EnvMapper.Env.MapConfiguration<TestConfiguration>();
Console.WriteLine($"Foo={config.Foo}");//This must be present
Console.WriteLine($"Bar={config.Bar}");//This may be present
Console.WriteLine($"Otherfield={config.OtherField}");//This may be present
```

Note: in the above example the variables that will be checked are `Foo`, `Bar`, and `Override`. If there is an environment variable `OtherField`, the reader will not check that as the DataMember signified the variable should be "Override".
