using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Packed = 1,
        Shipped = 2,
        Done = 3
    }
}
