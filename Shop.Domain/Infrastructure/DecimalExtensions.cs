using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public static class DecimalExtensions
    {

        public static string GetRubPrice(this decimal value)
        {
            return $"\x20BD{value:N2}";
        }
    }
}
