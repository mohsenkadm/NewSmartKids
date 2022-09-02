using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public int AdminNo { get; set; }
        public string Password { get; set; }      
        public string Token { get; set; }      
    }
}
