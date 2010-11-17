using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _message = "'{0}' must be at least {1} characters long.";

        public ValidatePasswordLengthAttribute()
            : base(_message)
        {
            Length = 6;
        }

        public int Length { get; set; }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                name, Length);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;

            return (valueAsString != null && valueAsString.Length >= Length);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            return new[] { new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), 
                Length, int.MaxValue) };
        }
    }
}
