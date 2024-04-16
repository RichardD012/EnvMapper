using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace EnvMapper
{
    public static class Env
    {
        public static TConfigType MapConfiguration<TConfigType>()
            where TConfigType : class
        {
            var invalidVariables = new List<FieldError>();
            var properties = typeof(TConfigType).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DataMemberAttribute)));
            //var returnOptions = new TConfigType();
            var returnOptions = Activator.CreateInstance<TConfigType>(); // Creates an instance using parameterless constructor

            foreach (var member in properties)
            {
                var dataMemberAttribute = member.GetCustomAttribute<DataMemberAttribute>();
                if (dataMemberAttribute == null)
                {
                    continue;
                }
                var propertyName = dataMemberAttribute.Name;
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    propertyName = member.Name;
                }
#if NET8
                var otherRequiredAttribute = member.GetCustomAttribute<System.Runtime.CompilerServices.RequiredMemberAttribute>();
                var (success, tempValue) = ReadEnvironmentVariable(propertyName, dataMemberAttribute.IsRequired || (otherRequiredAttribute != null));
#else
                // Default code if none of the above frameworks are targeted
                var (success, tempValue) = ReadEnvironmentVariable(propertyName, dataMemberAttribute.IsRequired );

#endif
                if (success)
                {
                    try
                    {
                        var parsedValue = ParseValue(member, tempValue);
                        member.SetValue(returnOptions, parsedValue, null);
                    }
                    catch (FormatException)
                    {
                        invalidVariables.Add(new FieldError(propertyName, FieldErrorType.InvalidFormat));
                    }
                    catch (OverflowException)
                    {
                        invalidVariables.Add(new FieldError(propertyName, FieldErrorType.InvalidFormat));
                    }
                    catch (ArgumentException)
                    {
                        invalidVariables.Add(new FieldError(propertyName, FieldErrorType.InvalidValue));
                    }
                }
                else
                {
                    invalidVariables.Add(new FieldError(propertyName, FieldErrorType.Missing));
                }
            }

            if (invalidVariables.Count > 0)
            {
                throw new EnvMapperException("Unable to process configuration", invalidVariables);
            }

            return returnOptions;
        }

        private static object ParseValue(PropertyInfo property, string value)
        {
            if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                return int.Parse(value);
            }

            if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
            {
                return long.Parse(value);
            }

            if (property.PropertyType == typeof(short) || property.PropertyType == typeof(short?))
            {
                return short.Parse(value);
            }

            if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                return double.Parse(value);
            }

            if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
            {
                return float.Parse(value);
            }
            
            if (IsNullableEnum(property.PropertyType, out var underlyingType) || property.PropertyType.IsEnum)
            {
                var enumType = property.PropertyType;
                if (underlyingType != null)
                {
                    enumType = underlyingType;
                }
                return Enum.Parse(enumType, value);
            }

            return value;
        }
        
        private static bool IsNullableEnum(this Type t, out Type result)
        {
            var u = Nullable.GetUnderlyingType(t);
            result = u;
            return (u != null) && u.IsEnum;
        }

        private static (bool, string) ReadEnvironmentVariable(string variableName, bool required = true)
        {
            var value = Environment.GetEnvironmentVariable(variableName);
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                    return (false, null);
                return (true, null);
            }
            //we don't care about new lines
            value = value.Replace("\r", "").Replace("\n", "").Trim();
            return (true, value);
        }
    }
}