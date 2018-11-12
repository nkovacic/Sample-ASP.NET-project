using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LengthRangeAttribute : ValidationAttribute
    {
        private int[] RangePairs { get; set; }

        public bool AllowEmptyStrings { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }

        public LengthRangeAttribute()
        {
            AllowEmptyStrings = true;
            MaxLength = int.MaxValue;
            MinLength = 0;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (!(value is string))
                {
                    throw new InvalidCastException("Property should be string type");
                }

                var stringValue = (string)value;

                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    if (stringValue.Length < MinLength)
                    {
                        return new ValidationResult($"Value has to be longer than {MinLength} characters");
                    }

                    if (stringValue.Length > MaxLength)
                    {
                        return new ValidationResult($"Value has to be smaller than {MaxLength} characters");
                    }
                }
                else if (!AllowEmptyStrings)
                {
                    return new ValidationResult("Value cannot be empty");
                }
            }

            return ValidationResult.Success;
        }
    }
}
