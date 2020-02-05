using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPracticeCore.ViewModels;
using ASPracticeCore.Repositories;
using ASPracticeCore.Models;
using ASPracticeCore.Utils;
using Microsoft.AspNetCore.Mvc;
using ASPracticeCore.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ASPracticeCore.Controllers
{
    [Authorize]
    public class ShareableController : Controller
    {
        RepositoryEF repo;
        readonly ApplicationContext _context;
        readonly IConfiguration _config;
        public ShareableController(ApplicationContext context, IConfiguration config)
        {
            //controller does the disposing of the context
            _context = context;
            _config = config;
        }

        public IActionResult ManageShareables(int userId)
        {
            ViewData["userId"] = userId;
            return View();
        }
        [HttpGet]
        
        public IActionResult CreateShareable(int userId, string message)
        {
            //just to mock a signed-in user on page refresh
            if (HomeController.loggedInMode)
            {
                HttpContext.Session.Set(Constants.KEY_USERID, 2);
                HttpContext.Session.Set(Constants.KEY_USER_NAME, "admin");
            }

            //if (!Util.ControllerUtil.IsAuthenticated())
            //{
            //    return Unauthorized();
            //}
            ViewBag.userId = userId;
            ViewData["isError"] = false;
            if (message != null)
            {
                string status = message.Split("_")[0];
                if (status == Constants.FAILED)
                {
                    ViewData["isTrue"] = true;
                }
                //"SUCCESS_Entity is added successfully" - send just the second half
                ViewBag.message = message.Split("_")[1];
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult CreateShareable(AddShareableViewModel shareableVM)
        {
            
            //if (!Util.ControllerUtil.IsAuthenticated())
            //{
            //    return Unauthorized();
            //}
            
            Util.DisplayObjectProperties(shareableVM);
            int activeUserId = HttpContext.Session.Get<int>(Constants.KEY_USERID);
            List<FilePath> files = Util.ControllerUtil.GetFilesFromRequest(Request.Form.Files,_config);
            Shareable shareable = new Shareable
            {
                Title = shareableVM.Title,
                Introduction = shareableVM.Introduction,
                DateTimeStamp = DateTime.Now,
                UserAccountId = activeUserId,
                Files = files
            };

            string message = "";

            repo = new RepositoryEF(_context);
            message = repo.Create(shareable);


            IActionResult returnAction = (message == Constants.SUCCESS) ? RedirectToAction("ManageProfile", "User", new { userId = activeUserId }) :
                RedirectToAction("CreateShareable", new { userId = activeUserId, message = message });

            return returnAction;
        }
        
        public async Task<IActionResult> GetUserShareables(int userId)
        {
            //test this
            RepositoryReflection repo = new RepositoryReflection();
            var shareables = await repo.GetItemsOfOwner<Shareable>("useraccount", "shareable", userId);
            Util.Log("Shareables size: ", shareables.Count());
            return Json(shareables);
        }

    }
}