using Sample.Core.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinCombinedLength : MinLengthAttribute
    {
        public new int Length { get; private set; }
        public List<string> OtherFields { get; private set; }

        public MinCombinedLength(int length) : base(length)
        {
            Length = length;
        }

        public MinCombinedLength(int length, params string[] otherFields) : base(length)
        {
            Length = length;

            if (otherFields != null)
            {
                OtherFields = otherFields.ToList();
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var lengthSum = 0;

            if (OtherFields == null)
            {
                return base.IsValid(value, validationContext);
            }

            OtherFields.Add(validationContext.MemberName);

            foreach (var fieldName in OtherFields)
            {
                var property = validationContext.ObjectType.GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    var propertyValue = property.GetValue(validationContext.ObjectInstance, null);

                    if (propertyValue != null)
                    {
                        if (property.PropertyType == typeof(string))
                        {
                            var propertyStringValue = propertyValue as string;

                            if (propertyStringValue != null)
                            {
                                lengthSum += propertyStringValue.Length;
                            }
                        }
                        else if (ExpressionsHelper.IsPropertyNonStringEnumerable(property.PropertyType))
                        {
                            var methodInfo = property.PropertyType.GetMethod("Count");

                            if (methodInfo != null && propertyValue != null)
                            {
                                var arrayLength = methodInfo.Invoke(propertyValue, null) as int?;

                                if (arrayLength.HasValue)
                                {
                                    lengthSum += arrayLength.Value;
                                }
                            }
                            else
                            {
                                var lengthProperty = property.PropertyType.GetProperty("Length");

                                if (lengthProperty != null)
                                {
                                    var arrayLength = lengthProperty.GetValue(propertyValue, null) as int?;

                                    if (arrayLength.HasValue)
                                    {
                                        lengthSum += arrayLength.Value;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Length <= lengthSum)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
    }
}
