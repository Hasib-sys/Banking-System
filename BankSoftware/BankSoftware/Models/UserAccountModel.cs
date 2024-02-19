using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankSoftware.Models
{
    public class UserAccountModel
    {

        [Key]
        public int UA_ID { get; set; }
        public string UA_NO { get; set; }
        public string UA_Holder { get; set; }
        public string Balance { get; set; }

        public string SenderAccountId { get; set; }
        public string ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }

        public decimal WithdrawAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public string UtilityUserID { get; set; }

       
        public class UtilityBillPaymentModel
        {
            [Required(ErrorMessage = "Account Number is required")]
            public string AccountNumber { get; set; }

            [Required(ErrorMessage = "Bill Type is required")]
            public string BillType { get; set; }


            [Required(ErrorMessage = "Bill Amount is required")]
            [DataType(DataType.Currency)]
            public decimal BillAmount { get; set; }


            public string UtilityUserID { get; set; }


            public IEnumerable<SelectListItem> BillTypeList { get; set; }
        }





    }
}