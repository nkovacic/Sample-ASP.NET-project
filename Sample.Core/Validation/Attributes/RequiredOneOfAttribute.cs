using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Sample.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredOneOfAttribute : ValidationAttribute
    {
        private string[] _requiredOneOfFields;

        public RequiredOneOfAttribute(params string[] requiredOneOfFields)
        {
            _requiredOneOfFields = requiredOneOfFields;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool onePropertyHasValue = value != null;

            if (value != null)
            {
                onePropertyHasValue = true;
            }

            if (_requiredOneOfFields != null && !onePropertyHasValue)
            {
                foreach (var oneOfRequiredFields in _requiredOneOfFields)
                {
                    var fieldPropertyInfo = validationContext.ObjectType.GetProperty(oneOfRequiredFields, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (fieldPropertyInfo != null)
                    {
                        var fieldValue = fieldPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                        if (fieldValue != null)
                        {
                            return ValidationResult.Success;
                        }
                    }
                    else
                    {
                        return new ValidationResult("Property with " + oneOfRequiredFields + " name was not found.");
                    }
                }
            }

            if (onePropertyHasValue)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}