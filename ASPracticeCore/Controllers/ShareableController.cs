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
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ASPracticeCore.Controllers
{
    [SessionValidate]
    public class ShareableController : Controller
    {
        RepositoryEF repo;
        readonly ApplicationContext _context;
        readonly IWebHostEnvironment _env;
        readonly IConfiguration _config;
        readonly IDataProtectionProvider _provider;
        public ShareableController(ApplicationContext context, IConfiguration config, IWebHostEnvironment env, IDataProtectionProvider provider)
        {
            //All supplied through DI
            _env = env;
            //controller does the disposing of the context
            _context = context;
            _config = config;
            _provider = provider;
        }

        public IActionResult UserShareables()
        {
            //await Util.ControllerUtil.MockLoginAsync(2, "admin",_context);


            Util.Log("Opening User Shareables!");
            //string imageSavePath = _env.ContentRootPath + _config.GetValue<string>("FileSavePath");
            ViewData["filePath"] = _config.GetValue<string>("ImageSavePath");

            return View();
        }

        public IActionResult ManageShareable()
        {
            return View();
        }
        public IActionResult CreateShareable(string message)
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateShareable(AddShareableViewModel shareableVM)
        {
            
            
            //Util.DisplayObjectProperties(shareableVM);
            //string savePath = _env.ContentRootPath + _config.GetValue<string>("ImageSavePath");

            string savePath = _config.GetValue<string>("ImageSavePath");
            int activeUserId = UserSessionHelper.GetActiveUserId(HttpContext.User.Identity.Name, _context);
            List<FilePath> files = await Util.ControllerUtil.SaveFilesToDisk(Request.Form.Files,savePath);
            Shareable shareable = new Shareable
            {
                Title = shareableVM.Title,
                Introduction = shareableVM.Introduction,
                DateTimeStamp = DateTime.Now,
                UserAccountId = activeUserId,
                Paragraphs = shareableVM.Paragraphs,
                FilePaths = files
            };

            string message = "";

            repo = new RepositoryEF(_context);
            message = repo.Create(shareable);

            
            IActionResult returnAction = (message == Constants.SUCCESS) ? RedirectToAction("UserShareables") :
                RedirectToAction("ManageSharable", new { message });

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
        public IActionResult GetShareablesOfUser()
        {
            int userId = UserSessionHelper.GetActiveUserId(HttpContext.User.Identity.Name, _context); 
            var shareables = _context.GetEntitySet<Shareable>().Where(s => s.UserAccountId == userId).Include(s=>s.Paragraphs).Include(s=>s.FilePaths).ToList();
            //var shareables = _context.GetEntitySet<Shareable>().Where(s => s.UserAccountId == userId).ToList();
            
            Util.Log("Paragraph1 for Shareable1", shareables[0].Paragraphs.ToList()[0].Text);
            Util.Log("How many shareables of user - ",shareables.Count);
            return Json(shareables);
        }

    }
}