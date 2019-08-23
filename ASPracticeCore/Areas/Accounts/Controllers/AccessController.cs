using ASPracticeCore.Areas.Accounts.Models;
using ASPracticeCore.Models;
using ASPracticeCore.Repositories;
using ASPracticeCore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ASPracticeCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class AccessController:Controller
    {
        
        [HttpGet]
        [Route("Accounts/Access/Login")]
        public IActionResult Login(string statusMessage)
        {
            ViewBag.statusMessage = statusMessage;
            return View();
        }

        [HttpPost]
        [Route("Accounts/Access/Login")]
        public IActionResult Login(string email, string password)
        {
           
            AccountRepository repo = new AccountRepository();
            UserAccount account = repo.Authenticate(email, password);
            bool isAuthenticated = account != null;
            IActionResult finalResult = isAuthenticated? RedirectToAction("Index", "Home"):RedirectToAction("Login", new {statusMessage="Incorrect username or password."});

            if (isAuthenticated && HttpContext.Session.Get<int>(Constants.KEY_USERID) == default)
            {
                Util.Log("Setting session data items...");
                HttpContext.Session.Set(Constants.KEY_USERID, account.Id);
                HttpContext.Session.Set(Constants.KEY_USER_NAME, account.Name);
            }

            return finalResult;
        }
        
        [Route("Accounts/Access/Register")]
        public IActionResult Register(UserAccount account, UserAddress address)
        {

            Util.Log("Inside Register()");
            Util.DisplayObjectProperties(account);
            Util.DisplayObjectProperties(address);

            Repository repo = new Repository();
            //add the account, add the address as well
            string addAccountStatus = repo.Add(account);
            
            Util.Log("Add account:", addAccountStatus);

            string addAddressStatus = "UserAddress wasn't added because UserAccount didn't add successfully.";
            //check useraccount add success before adding its address
            if(addAccountStatus == Constants.SUCCESS)
            {
                UserAccount recentlyAddedUser = repo.GetAll<UserAccount>().LastOrDefault();
                address.Id = recentlyAddedUser.Id;//the user's id
                addAddressStatus = repo.Add(address);
                Util.Log("Add address:", addAddressStatus);
            }
           
            
            string successString = Constants.SUCCESS;
            string[] taskStatusMessages = Util.ControllerUtil.GetFinalStatusMessages(successString, addAccountStatus, addAddressStatus);
            
            //taskStatusMessages either contains a single element success string or the whole error collection
            if (taskStatusMessages[0] != successString)
            {
                //then taskStatusMessages is an error collection
                return RedirectToAction("Error", "Home");
            }
            //SUCCESS operation
            return RedirectToAction("Login", new { statusMessage = taskStatusMessages[0] });
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(Constants.KEY_USERID);
            HttpContext.Session.Remove(Constants.KEY_USER_NAME);
            return RedirectToAction("Login");
        }



    }
}
