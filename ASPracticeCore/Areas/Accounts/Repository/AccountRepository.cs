using ASPracticeCore.Areas.Accounts.Models;
using ASPracticeCore.Models;
using ASPracticeCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Repositories
{
    public class AccountRepository : RepositoryReflection
    {
        /// <summary>
        /// Child class or Repository for Area:Account-specific domain operations
        /// 
        /// </summary>
        public AccountRepository() : base()
        {

     
        }


        public UserAccount Authenticate(string email, string password)
        {
            //through reflection:
            UserAccount account = GetFiltered<UserAccount>(new { email, password }).FirstOrDefault();
            return account; 
        }



    }
}
