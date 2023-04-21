using System;
using Xunit;

namespace EnvMapper.Tests;

public class EnvMapperTests
{
    [Fact]
    public void LoadTest()
    {
        Environment.SetEnvironmentVariable("One", "Test");
        var test = EnvMapper.Env.MapConfiguration<FieldModel>();
        Assert.Equal(test.FieldOne, Environment.GetEnvironmentVariable("One"));
        Assert.Null(test.FieldTwo);
    }

    [Fact]
    public void LoadAllTest()
    {
        Environment.SetEnvironmentVariable("One", "Test1");
        Environment.SetEnvironmentVariable("FieldTwo", "Test2");
        var test = EnvMapper.Env.MapConfiguration<FieldModel>();
        Assert.Equal(test.FieldOne, Environment.GetEnvironmentVariable("One"));
        Assert.Equal(test.FieldTwo, Environment.GetEnvironmentVariable("FieldTwo"));
    }

    [Fact]
    public void MissingFieldTest()
    {
        Environment.SetEnvironmentVariable("One", null);
        Environment.SetEnvironmentVariable("FieldTwo", null);
        Assert.Throws<EnvMapperException>(() => EnvMapper.Env.MapConfiguration<FieldModel>());
    }

    [Fact]
    public void LoadIntTest()
    {
        Environment.SetEnvironmentVariable("IntField", "1");
        var test = EnvMapper.Env.MapConfiguration<IntModel>();
        Assert.Equal(test.IntField, 1);
    }

    [Fact]
    public void LoadInvalidIntTest()
    {
        Environment.SetEnvironmentVariable("IntField", "Number");
        Assert.Throws<EnvMapperException>(() => EnvMapper.Env.MapConfiguration<IntModel>());
    }

    [Fact]
    public void LoadDoubleTest()
    {
        Environment.SetEnvironmentVariable("DoubleField", "1.5");
        var test = EnvMapper.Env.MapConfiguration<DoubleModel>();
        Assert.Equal(test.DoubleField, 1.5d);
    }

    [Fact]
    public void LoadInvalidDoubleTest()
    {
        Environment.SetEnvironmentVariable("DoubleField", "Number");
        Assert.Throws<EnvMapperException>(() => EnvMapper.Env.MapConfiguration<DoubleModel>());
    }

    //Test Short, Float, Long

    [Fact]
    public void LoadEnumTest()
    {
        Environment.SetEnvironmentVariable("EnumField", "ValidValue");
        Environment.SetEnvironmentVariable("Other", "ValidValue");
        var test = EnvMapper.Env.MapConfiguration<EnumModel>();
        Assert.Equal(test.EnumField, TestEnum.ValidValue);
        Assert.Equal(test.EnumField2, TestEnum.ValidValue);
    }

    [Fact]
    public void LoadInvalidEnumTest()
    {
        Environment.SetEnvironmentVariable("EnumField", "InvalidValue");
        Environment.SetEnvironmentVariable("Other", "InvalidValue");
        Assert.Throws<EnvMapperException>(() => EnvMapper.Env.MapConfiguration<EnumModel>());
    }
}