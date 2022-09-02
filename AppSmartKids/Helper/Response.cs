using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AppSmartKid.Helper
{
    public class Response<TEntity>
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public TEntity data { get; set; } 
        public bool Unauthorized { get; set; }
    }
    public class ResponseList<TEntity>
    {
        public bool success { get; set; }
        public string msg { get; set; } 
        public List<TEntity> data { get; set; } 
        public bool Unauthorized { get; set; }
    }
    public class ResponseCollection<TEntity>
    {
        public bool success { get; set; }
        public string msg { get; set; } 
        public ObservableCollection<TEntity> data { get; set; } 
        public bool Unauthorized { get; set; }
    }
}
