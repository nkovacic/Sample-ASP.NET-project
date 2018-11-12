using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MultiRangeAttribute : ValidationAttribute
    {
        private double[] RangePairs { get; set; }

        public MultiRangeAttribute(params double[] rangePairs)
        {
            RangePairs = rangePairs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var numericValue = Convert.ToDouble(value);

                if (RangePairs.Length % 2 != 0)
                {
                    throw new ArgumentException("Ranges should be specified in pairs (min-max).");
                }

                var isInRange = false;

                for (int i = 0; i < RangePairs.Length; i += 2)
                {
                    if (numericValue >= RangePairs[i] &&  numericValue <= RangePairs[i + 1])
                    {
                        isInRange = true;

                        break;
                    }
                }

                if (!isInRange)
                {
                    return new ValidationResult("Number should be between ranges: " + string.Join(", ", 
                        RangePairs.Select(q => q.ToString()).ToArray()));
                }
            }

            return ValidationResult.Success;
        }
    }
}
