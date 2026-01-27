using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class UserPromoCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PromoCodeId { get; set; }
        public DateTime UsedDate { get; set; }
        public string PromoCodeName { get; set; }
        public string UserName { get; set; }
    }
}
