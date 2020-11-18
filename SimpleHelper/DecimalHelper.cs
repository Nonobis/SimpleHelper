using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHelper
{
    public static class DecimalHelper
    {
        /// <summary>
        /// Verify if decimal number has a value
        /// </summary>
        /// <param name="value">Decimal number</param>
        /// <returns>True if decimal number has a value, or false if doesn't.</returns>
        public static bool HasDecimal(decimal value)
        {
            return (value - Math.Round(value) != 0);
        }

        /// <summary>
        /// This helper is for pagination.
        /// Returns the integer of the decimal if there are no decimal places.
        /// Otherwise, return the integer, incremented 1.
        /// </summary>
        /// <param name="value">Decimal number</param>
        /// <returns>The pagination number.</returns>
        public static int CalculateNumberPage(decimal value)
        {
            if (value - Math.Round(value) != 0)
            {
                value++;
                return GetIntPart(value);
            }
            return GetIntPart(value);
        }

        /// <summary>
        /// Will return the integer part, of a decimal value.
        /// </summary>
        /// <param name="value">Decimal number</param>
        /// <returns></returns>
        public static int GetIntPart(decimal value)
        {
            return (int)Math.Truncate(value);
        }


    }
}
