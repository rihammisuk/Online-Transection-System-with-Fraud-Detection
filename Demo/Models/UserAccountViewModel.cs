using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class UserAccountViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public Nullable<decimal> Transection_Money { get; set; }
        public string TransectionType { get; set; }
        public string F_P_AccNo { get; set; }
        public Nullable<bool> IsApproved_old { get; set; }
        public Nullable<bool> IsApproved_new { get; set; }
        public Nullable<bool> IsFraud_old { get; set; }
        public Nullable<bool> IsFraud_new { get; set; }
        public string CreditRatingForOne { get; set; }
        public string AccountTypeForOne { get; set; }
        public string TransectionDate { get; set; }
        public string UserIPAddress { get; set; }


    }
}