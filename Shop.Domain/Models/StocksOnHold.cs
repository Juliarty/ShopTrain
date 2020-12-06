using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain.Models
{
    public class StocksOnHold
    {
        public int Id{ get; set; }
        public string SessionId{ get; set; }
        public int StockId{ get; set; }
        public Stock Stock{ get; set; }
        public DateTime ExpiryTime{ get; set; }
        public int Qty{ get; set; }
    }
}
