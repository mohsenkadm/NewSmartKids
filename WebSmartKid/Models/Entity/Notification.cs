using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Entity
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Details { get; set; }
        public string Title { get; set; }
        public DateTime DateInsert { get; set; }  
        public int UserId { get; set; }  
    }
}
