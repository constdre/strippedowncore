using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPracticeCore.Models;

namespace ASPracticeCore.Areas.Accounts.Models
{
    public class UserAddress:EntityBase
    {
       
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

    }
}
