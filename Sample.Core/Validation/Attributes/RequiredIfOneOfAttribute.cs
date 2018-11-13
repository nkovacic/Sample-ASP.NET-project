using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Core.Validation
{
    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredIfOneOfAttribute : ValidationAttribute
    {
        public string[] RequiredOneOfFields;
        public string OtherProperty { get; set; }
        public object OtherValue { get; set; }

        public RequiredIfOneOfAttribute(string dependentUpon, object value, params string[] requiredOneOfFields)
        {
            OtherProperty = dependentUpon;
            OtherValue = value;
            RequiredOneOfFields = requiredOneOfFields;
        }

        public RequiredIfOneOfAttribute(string dependentUpon)
        {
            OtherProperty = dependentUpon;
            OtherValue = null;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // the the other property
            var property = validationContext.ObjectType.GetProperty(OtherProperty);

            // check it is not null
            if (property == null)
                return new ValidationResult(string.Format("Unknown property: {0}.", OtherProperty));

            // get the other value
            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                if (otherPropertyValue != null)
                {
                    if (OtherValue == null)
                    {
                        if (value != null)
                        {
                            return ValidationResult.Success;
                        }
                    }
                    else
                    {
                        if (OtherValue.Equals(otherPropertyValue))
                        {
                            var validationResult = IsValidOneOf(value, validationContext);

                            if (validationResult == ValidationResult.Success)
                            {
                                return validationResult;
                            }
                        }
                        else
                        {
                            return ValidationResult.Success;
                        }
                    }
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            // got this far must mean the items don't meet the comparison criteria
            return new ValidationResult(string.Format("Ker ni {0} prazen, tudi {1} ne sme bit prazen.", OtherProperty, validationContext.DisplayName));
        }

        private ValidationResult IsValidOneOf(object value, ValidationContext validationContext)
        {
            bool onePropertyHasValue = false;

            if (value != null)
            {
                onePropertyHasValue = true;
            }

            if (RequiredOneOfFields != null)
            {
                foreach (var oneOfRequiredFields in RequiredOneOfFields)
                {
                    var fieldPropertyInfo = validationContext.ObjectType.GetProperty(oneOfRequiredFields);

                    if (fieldPropertyInfo != null)
                    {
                        var fieldValue = fieldPropertyInfo.GetValue(validationContext.ObjectInstance, null);

                        if (fieldValue != null)
                        {
                            if (!onePropertyHasValue)
                            {
                                onePropertyHasValue = true;
                            }
                            else
                            {
                                return new ValidationResult(ErrorMessage);
                            }
                        }
                    }
                    else
                    {
                        throw new KeyNotFoundException("Property with " + oneOfRequiredFields + " name was not found.");
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