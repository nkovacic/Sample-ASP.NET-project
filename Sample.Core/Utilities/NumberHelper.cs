using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Utilities
{
    public class NumberHelper
    {
        private static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(decimal), typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort), typeof(int), typeof(double)
        };

        public static double RoundDouble(double value, int numberOfDecimals = 0)
        {
            return Math.Round(value, numberOfDecimals);
        }

        public static decimal RoundDecimal(decimal value, int numberOfDecimals = 0)
        {
            return Math.Round(value, numberOfDecimals);
        }

        public static bool IsNumeric(string s)
        {
            float output;

            return float.TryParse(s, out output);
        }

        public static bool IsNumeric(Type type)
        {
            return NumericTypes.Contains(type);
        }

        public static string FormatForMoney(decimal value)
        {
            if (value % 1 == 0)
            {
                return string.Format("{0:#.0}", value);
            }
            else
            {
                return string.Format("{0:#.0,##}", value);
            }
        }
    }
}
