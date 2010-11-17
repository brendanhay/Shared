using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class CompareDatesAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _message = "{0} must be at least {1} days ahead of {2}.";

        private readonly string _confirmProperty;

        public CompareDatesAttribute(string confirmProperty)
            : base(_message)
        {
            _confirmProperty = confirmProperty;
            DaysAhead = 1;
        }

        public int DaysAhead { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var property = context.ObjectType.GetProperty(_confirmProperty);
            var attribute = property.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();

            var confirmName = attribute == null ? _confirmProperty : ((DisplayAttribute)attribute).Name;
            var confirmValue = property.GetValue(context.ObjectInstance, null);

            return (DateTime)value >= ((DateTime)confirmValue).AddDays(DaysAhead)
                ? null
                : new ValidationResult(string.Format(_message, context.DisplayName, DaysAhead, confirmName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,
            ControllerContext context)
        {
            return new[] { new ModelClientValidationEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), 
                _confirmProperty) };
        }
    }
}
