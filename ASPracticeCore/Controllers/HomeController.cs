using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPracticeCore.Models;
using ASPracticeCore.Repositories;
using ASPracticeCore.Areas.Accounts.Models;
using Newtonsoft.Json;
using ASPracticeCore.Utils;

namespace ASPracticeCore.Controllers
{
    public class HomeController : Controller
    {
        public static bool loggedInMode = true;
        public IActionResult Index(string message)
        {
            if(loggedInMode == true)
            {
                HttpContext.Session.Set(Constants.KEY_USERID, 2);
                HttpContext.Session.Set(Constants.KEY_USER_NAME, "admin" );
            }
            ViewBag.NameUser = HttpContext.Session.Get<string>(Constants.KEY_USER_NAME)??default;
            return View();
        }

        public IActionResult About()
        {
            //ViewData["Message"] = "ASP.NET Core 3.0 and that's it. \n\t-Own persistence mechanism through dynamic generic queries.\n-\tjust Javascript";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
