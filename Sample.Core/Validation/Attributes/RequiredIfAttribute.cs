using Sample.Core.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sample.Core.Validation
{
    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        protected string OtherProperty { get; set; }
        protected object[] OtherValues { get; set; }

        public RequiredIfAttribute(string dependentUpon, params object[] values)
        {
            OtherProperty = dependentUpon;
            OtherValues = values;
        }

        public RequiredIfAttribute(string dependentUpon)
        {
            OtherProperty = dependentUpon;
            OtherValues = null;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // the the other property
            var property = validationContext.ObjectType.GetProperty(OtherProperty);

            // check it is not null
            if (property == null)
            {
                return new ValidationResult(String.Format("Unknown property: {0}.", OtherProperty));
            }
                
            // get the other value
            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                if (OtherValues == null || !OtherValues.Any())
                {
                    if (value != null)
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    var anyValid = false;

                    foreach (var otherValue in OtherValues)
                    {
                        if (otherValue.Equals(otherPropertyValue))
                        {
                            anyValid = true;

                            if (value != null)
                            {
                                return ValidationResult.Success;
                            }
                        }
                    }

                    if (!anyValid)
                    {
                        return ValidationResult.Success;
                    }
                }
            }

            // got this far must mean the items don't meet the comparison criteria
            return new ValidationResult(String.Format("Ker ni {0} prazen, tudi {1} ne sme bit prazen.", OtherProperty, validationContext.DisplayName));
        }
    }
}