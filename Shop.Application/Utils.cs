using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application
{
    public static class Utils
    {
        public static decimal GetDecimal(string value)
        {
            if (value.Contains("."))
            {
                value = value.Replace(".", ",");
            }

            return decimal.Parse(value);
        }
    }
}
