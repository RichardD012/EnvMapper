using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EnvMapper
{
    public class EnvMapperException : Exception
    {
        public IReadOnlyList<FieldError> ErrorProperties { get; }
        public EnvMapperException(string message, IReadOnlyList<FieldError> errorProperties) : base(message)
        {
            ErrorProperties = errorProperties;
        }

        public string GenerateErrorString()
        {
            var message = Environment.NewLine;
            message += "*************** Unable to process configuration! **************" + Environment.NewLine;
            message += "**** The following environment variables must be defined:  ****" + Environment.NewLine;
            message += string.Join(", ", ErrorProperties.Select(x=>$"{x.FieldName} ({x.ErrorType}"));
            message += Environment.NewLine;
            message += "***************************************************************" + Environment.NewLine; ;
            return message;
        }
    }

    public class FieldError
    {
        public string FieldName { get; }
        
        public FieldErrorType ErrorType { get; }

        public FieldError(string name, FieldErrorType type)
        {
            FieldName = name;
            ErrorType = type;
        }
        
    }

    public enum FieldErrorType
    {
        Missing,
        InvalidFormat,
        InvalidValue
    }
}