using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetAmount { get; set; }
        // USER INFO
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Details { get; set; }
        public string CountryName { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }

        public bool IsCancel { get; set; }
        public bool IsApporve { get; set; }
        public bool IsDone { get; set; }            
    }
}
