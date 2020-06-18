using ASPracticeCore.Areas.Accounts.Models;
using ASPracticeCore.DAL;
using ASPracticeCore.Models;
using ASPracticeCore.Repositories;
using ASPracticeCore.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ASPracticeCore.Areas.Accounts.Controllers
{

    [AllowAnonymous]
    [Area("Accounts")]
    public class AccessController : Controller
    {
        readonly ApplicationContext _context;

        public AccessController(ApplicationContext context)
        {
            _context = context;
        }

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
        public async Task<IActionResult> Login(string email, string password)
        {

            AccountRepository repo = new AccountRepository();
            //check its existence in the database
            UserAccount account = repo.Authenticate(email, password);

            if (account == null)
            {
                //no match, return to login with message
                string failMessage = Constants.FAILED + "_Incorrect username or password.";
                return RedirectToAction("Login", new { statusMessage = failMessage });
            }

            //Match - create auth cookie with unique key in it, save unique key along with user id to the database
            await Util.ControllerUtil.SignInAsync(account.Id, account.Name);
            return RedirectToAction("Index", "Home", new {personName = account.Name});
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


        
        [Authorize]
        [Route("Accounts/Access/Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            //clear the session data 
            //await HttpContext.Session.LoadAsync();
            //HttpContext.Session.Remove(Constants.KEY_USERID);
            //HttpContext.Session.Remove(Constants.KEY_USER_NAME);
            //HttpContext.Session.Clear();

            ////remove the session cookie in the response:
            //Response.Cookies.Delete(Constants.COOKIE_NAME_SESSION);

            //remove the auth cookie
            
            await Util.ControllerUtil.SignOutAsync(); //to delete the auth cookie
            

            return RedirectToAction("Login");
        }



    }
}
