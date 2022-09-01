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
        public bool IsCancel { get; set; }
        public bool IsApporve { get; set; }
        public bool IsDone { get; set; }
        public bool IsDiscount { get; set; }
        public string TypeDiscount { get; set; }
    }
}
