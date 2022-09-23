using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSmartKidsXa.Entity
{
    public class Products
    {
        public int ProductsId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public decimal Price { get; set; }   
        public int Count { get; set; }   
        public int CategoriesId { get; set; }
        public int NoOfBuyers { get; set; }
        public int DiscountPercentage { get; set; }     
        public bool IsDiscount { get; set; }     
        public int LikeCount { get; set; }        
        public string SourceLike { get; set; }
        public string CategoriesName { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }

        // for filter                     
        public List<AgeFilter> AgeFilter { get; set; }

    }
    public class AgeFilter
    {
        public int? Id;
    }
}
