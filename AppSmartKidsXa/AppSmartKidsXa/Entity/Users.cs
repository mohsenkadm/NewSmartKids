using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSmartKidsXa.Entity
{
    public class Users
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }    
        public string Details { get; set; }    
        public string Token { get; set; }       
        public int CountryId { get; set; }       
        public string Address { get; set; }       
    }
}
