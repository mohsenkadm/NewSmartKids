using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class Items
    {
        public int ItemsId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public decimal Price { get; set; }
        public int AgeId { get; set; }
        public int CategoriesId { get; set; }
        public int NoOfBuyers { get; set; }
        public bool IsDiscount { get; set; }
        public string TypeDiscount { get; set; }
        public int LikeCount { get; set; }        
        public string SourceLike { get; set; }
    }
}
