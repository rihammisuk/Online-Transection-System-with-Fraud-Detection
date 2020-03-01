using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class MoneyRequestViewModel
    {

        public int ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string RequestID { get; set; }
        public string RequestBalance { get; set; }
        public string RequestDate { get; set; }
    }
}