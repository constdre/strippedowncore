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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ASPracticeCore.DAL;

namespace ASPracticeCore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public static readonly bool loggedInMode = false;
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            //await Util.ControllerUtil.MockLoginAsync(2, "admin", _context);
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
