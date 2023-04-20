# EnvMapper

A .NET library to map environment variables to a C# object. This is useful when deploying to containerized environments where environment variables may be injected before starting up.

## Usage

### Load env file

`MapConfiguration()` will automatically search for environment variables and map them to a class decorated with `System.Runtime.Serialization.DataMember` attributes. This supports overriding using the `Name` and `IsRequired` properties of the `DataMemberAttribute`.

```csharp
var config = AddEnvironmentConfiguration.MapConfiguration<TType>();
```

If properties that are marked as required are not found, a `EnvMapperException` will be thrown, which contains a list of missing properties.
