using ASPracticeCore.Areas.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class UserSession : IEntity
    {
        public int Id { get; set; }

        public int UserAccountId { get; set; }
        public string Guid { get; set; }
        public virtual UserAccount UserAccount {get;set;}

    }
}
