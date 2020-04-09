using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspAPI.ViewModels
{
    public class BatchesVM
    {
        public string id { get; set; }
        public DateTime start_Date { get; set; }
        public DateTime end_Date { get; set; }
    }
}