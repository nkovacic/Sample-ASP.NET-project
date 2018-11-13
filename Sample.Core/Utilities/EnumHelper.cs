using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Utilities
{
    public class EnumHelper
    {
        public static T[] ConvertFlagToArray<T>(T enumWithFlags)
        {
            return enumWithFlags
                .ToString()
                .Split(',')
                .Select(flag => (T)Enum.Parse(typeof(T), flag.Trim()))
                .ToArray();
        }

        public static bool TryGetValue<T>(string description, out T value)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    var fieldValue = field.GetValue(null);

                    value = Cast<T>(fieldValue);

                    return fieldValue == null ? true : false;
                }
                else if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    var fieldValue = field.GetValue(null);

                    value = Cast<T>(fieldValue);

                    return fieldValue == null ? true : false;
                }
            }

            var enumValue = Convert.ChangeType(description, Enum.GetUnderlyingType(type));

            if (enumValue != null)
            {
                value = Cast<T>(enumValue);

                return true;
            }

            if (type.IsValueType)
            {
                value = Cast<T>(Activator.CreateInstance(type));

                return true;
            }

            value = default(T);

            return false;
        }

        public static T GetValue<T>(string description)
        {
            var type = typeof(T);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)field.GetValue(null);
                }
                else if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)field.GetValue(null);
                }
            }

            return default(T);
        }

        public static object GetValue(Type type, string description)
        {
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return field.GetValue(null);
                }
                else if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return field.GetValue(null);
                }
            }

            var enumValue = Convert.ChangeType(description, Enum.GetUnderlyingType(type));
            
            if (enumValue != null)
            {
                return enumValue;
            }

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static int GetEnumLength<T>(bool withoutDefault = true)
        {
            var enumNames = Enum.GetNames(typeof(T));

            if (withoutDefault)
            {
                return enumNames.Where(q => !q.Equals("Default", StringComparison.OrdinalIgnoreCase) && !q.Equals("None", StringComparison.OrdinalIgnoreCase)).Count();
            }
            else
            {
                return enumNames.Length;
            }
        }

        private static T Cast<T>(object value)
        {
            if (value is T)
            {
                return (T)value;
            }
            else
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
        }
    }
}
