using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductsId { get; set; }  
        public int DiscountPercentage { get; set; }  
        public decimal Price { get; set; }
        public int Count { get; set; }

        // for show
        public string ProdName { get; set; }     
        public string Image { get; set; }     
        public string Name { get; set; }     
        //detail for product
        public string Details { get; set; }      
        public string Detail { get; set; }      
        public string Phone { get; set; }      
        public string CountryName { get; set; }      
        public int CountryId { get; set; }      
        public string Address { get; set; }      
        public int OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public bool IsCancel { get; set; }
        public bool IsApporve { get; set; }
        public bool IsDone { get; set; }            
        public bool IsDiscount { get; set; }            
        public decimal NetAmount { get; set; }
        public decimal TotalDiscount { get; set; }
    }
}
