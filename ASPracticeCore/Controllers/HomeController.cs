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
        public IActionResult Index(string message)
        {
            ViewBag.NameUser = HttpContext.Session.Get<string>(Constants.KEY_USER_NAME)??default;
            //message = activeUserEmail ?? "Guest";
            ViewBag.message = message;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "ASP.NET Core 2.1 and that's it. Own persistence mechanism through dynamic generic queries.";
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
