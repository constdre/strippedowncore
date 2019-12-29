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
            
            ViewData["isError"] = false;
            if (statusMessage != null)
            {
                
                string classifier = statusMessage.Split('_')[0];
                if (classifier.Equals(Constants.FAILED))
                {
                    ViewData["isError"] = true;
                }
                ViewBag.statusMessage = statusMessage.Split('_')[1];

            }

            return View();
        }

        [HttpPost]
        [Route("Accounts/Access/Login")]
        public IActionResult Login(string email, string password)
        {
           
            AccountRepository repo = new AccountRepository();
            UserAccount account = repo.Authenticate(email, password);
            bool isAuthenticated = account != null;
            string failMessage = Constants.FAILED + "_Incorrect username or password.";
            IActionResult finalResult = isAuthenticated? RedirectToAction("Index", "Home"):RedirectToAction("Login", new {statusMessage=failMessage});

            if (isAuthenticated && HttpContext.Session.Get<int>(Constants.KEY_USERID) == default)
            {
                Util.Log("Setting session data items...");
                //Is this 1 session per user?
                HttpContext.Session.Set(Constants.KEY_USERID, account.Id);
                HttpContext.Session.Set(Constants.KEY_USER_NAME, account.Name);
            }

            return finalResult;
        }
        
        [Route("Accounts/Access/Register")]
        public IActionResult Register(UserAccount account)
        {

            Util.DisplayObjectProperties(account);

            RepositoryReflection repo = new RepositoryReflection();
            string addStatus = repo.Add(account);

            if (addStatus != Constants.SUCCESS)
            {
                return RedirectToAction("Error", "Home");
            }

            string statusMessage = Constants.SUCCESS + "_" + account.Email + " added successfully!";
            return RedirectToAction("Login", new { statusMessage });
        }
       
        [Route("Accounts/Access/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(Constants.KEY_USERID);
            HttpContext.Session.Remove(Constants.KEY_USER_NAME);
            return RedirectToAction("Login");
        }



    }
}
