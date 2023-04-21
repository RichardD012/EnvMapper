using System.Runtime.Serialization;

namespace EnvMapper.Tests;

[DataContract]
public class FieldModel
{
    [DataMember(Name = "One", IsRequired = true)]
    public string? FieldOne { get; set; }
    [DataMember]
    public string? FieldTwo { get; set; }
}

[DataContract]
public class IntModel
{
    [DataMember]
    public int? IntField { get; set; }
}

[DataContract]
public class DoubleModel
{
    [DataMember]
    public double? DoubleField { get; set; }
}

[DataContract]
public class EnumModel
{
    [DataMember]
    public TestEnum? EnumField { get; set; }
    
    [DataMember(Name="Other")]
    public TestEnum EnumField2 { get; set; }
}

public enum TestEnum
{
    ValidValue
}
