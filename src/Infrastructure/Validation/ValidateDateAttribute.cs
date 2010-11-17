using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateDateAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _message = "'{0}' cannot be in the past.";

        public ValidateDateAttribute() : base(_message) { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        public override bool IsValid(object value)
        {
            return (DateTime)value >= GetDate();
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationRangeRule(FormatErrorMessage(metadata.GetDisplayName()),
                GetDate(), DateTime.MaxValue) };
        }

        private static DateTime GetDate()
        {
            return DateTime.Today;
        }
    }
}
