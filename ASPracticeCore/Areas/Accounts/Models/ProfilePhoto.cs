using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Areas.Accounts.Models
{
    public class ProfilePhoto:ASPracticeCore.Models.Photo
    {
        //the foreignkey for its User owner
        public int UserId { get; set; }
    }
}
