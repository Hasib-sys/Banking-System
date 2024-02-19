using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankSoftware.Models
{
    public class ProfileModel
    {
        [Key]
        public int Profile_ID { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
    }
}