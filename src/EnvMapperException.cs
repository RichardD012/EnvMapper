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
    }
}