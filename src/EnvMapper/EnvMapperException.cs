using System;
using System.Collections;
using System.Collections.Generic;

namespace EnvMapper
{
    public class EnvMapperException : Exception
    {
        public IReadOnlyList<string> MissingProperties { get; private set; }
        public EnvMapperException(string message, IReadOnlyList<string> missingProperties) : base(message)
        {
            MissingProperties = missingProperties;
        }

        public string GenerateErrorString()
        {
            var message = Environment.NewLine;
            message += "*************** Unable to process configuration! **************" + Environment.NewLine;
            message += "**** The following environment variables must be defined:  ****" + Environment.NewLine;
            message += string.Join(", ", MissingProperties);
            message += Environment.NewLine;
            message += "***************************************************************" + Environment.NewLine; ;
            return message;
        }
    }
}