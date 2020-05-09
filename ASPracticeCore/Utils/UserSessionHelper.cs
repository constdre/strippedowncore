using ASPracticeCore.Areas.Accounts.Models;
using ASPracticeCore.DAL;
using ASPracticeCore.Models;
using ASPracticeCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Utils
{
    public class UserSessionHelper
    {
        public static void CreateUserSession(ApplicationContext context, int userId, string guid)
        {
            context.Database.OpenConnection();
            try
            {
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.UserSession ON");
                var efRepo = new RepositoryEF(context);
                //save new session
                context.GetEntitySet<UserSession>()
                    .Add(new UserSession()
                    {
                        UserAccountId = userId,
                        Guid = guid
                    });
                context.SaveChanges();
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.UserSession OFF");

            }
            finally
            {
                context.Database.CloseConnection();
                Util.Log("Created session with key ", guid);
            }

        }

        public static void RemoveUserSession(ApplicationContext context, string guid)
        {
            var sessionToDelete = Find(guid, context);
            context.GetEntitySet<UserSession>().Remove(sessionToDelete);
            context.SaveChanges();
        }

        public static int GetActiveUserId(string guid, ApplicationContext context)
        {
            int userId = GetActiveUserId(guid, context);
            return context.GetEntitySet<UserSession>().Find(userId).UserAccountId;
        }
        public static bool IsSessionValid(string guid, ApplicationContext context)
        {
            return Find(guid, context) == null ? false : true;
        }

        public static UserSession Find(string guid, ApplicationContext context)
        {
            int userId = GetActiveUserId(guid, context);
            return context.GetEntitySet<UserSession>().Find(userId);
        }
    }
}
