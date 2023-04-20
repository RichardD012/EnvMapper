
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
                where TConfigType : class, new()
        {
            var missingEnvironmentVariables = new List<string>();
            var properties = typeof(TConfigType).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(DataMemberAttribute)));
            var returnOptions = new TConfigType();
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
                var tempValue = ReadEnvironmentVariable(propertyName, ref missingEnvironmentVariables, dataMemberAttribute.IsRequired);
                member.SetValue(returnOptions, tempValue, null);
            }
            if (missingEnvironmentVariables.Count > 0)
            {
                /*var message = Environment.NewLine;
                message += "*************** Unable to process configuration! **************" + Environment.NewLine;
                message += "**** The following environment variables must be defined:  ****" + Environment.NewLine;
                message += string.Join(", ", missingEnvironmentVariables);
                message += Environment.NewLine;
                message += "***************************************************************" + Environment.NewLine;*/
                throw new EnvMapperException("Unable to process configuration - Missing Variables", missingEnvironmentVariables);
                //throw new Exception(message);
            }
            return returnOptions;
        }

        private static string ReadEnvironmentVariable(string variableName, ref List<string> missingEnvironmentVariables, bool required = true)
        {
            var value = Environment.GetEnvironmentVariable(variableName);
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                    missingEnvironmentVariables.Add(variableName);
                return string.Empty;
            }
            else
            {
                //we don't care about new lines
                value = value.Replace("\r", "").Replace("\n", "").Trim();
            }
            return value;
        }
    }
}