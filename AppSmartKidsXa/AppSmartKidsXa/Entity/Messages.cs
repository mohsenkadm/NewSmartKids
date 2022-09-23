using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSmartKidsXa.Entity
{
    public class Messages
    {          
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime Date { get; set; }
        public string DateShow { get; set; }
        public int UserSenderId { get; set; }
        public int UserReciverId { get; set; }
        public bool IsOwner { get; set; }
        public string Name { get; set; }
    }
}
