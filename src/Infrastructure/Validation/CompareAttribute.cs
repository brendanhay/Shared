using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class CompareAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _message = "'{0}' and '{1}' do not match.";

        private readonly string _confirmProperty;
        private readonly object _typeId = new object();

        public CompareAttribute(string confirmProperty)
            : base(_message)
        {
            _confirmProperty = confirmProperty;
        }

        public override object TypeId
        {
            get { return _typeId; }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name,
                _confirmProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var confirmValue = context.ObjectType.GetProperty(_confirmProperty)
                .GetValue(context.ObjectInstance, null);

            return Equals(value, confirmValue)
                ? null
                : new ValidationResult(FormatErrorMessage(context.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            return new[] { new ModelClientValidationEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), 
                _confirmProperty) };
        }
    }
}
