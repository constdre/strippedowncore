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

    public class ShareableController : Controller
    {

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

        [HttpGet]
        public IActionResult CreateShareable()
        {
            return UserShareables();
        }
        public IActionResult ManageShareable()
        {
            return UserShareables();
        }
        public IActionResult UserShareables()
        {
            return View("Views/Shareable/UserShareables.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateShareableAsync(AddShareableViewModel shareableVM)
        {


            //Util.DisplayObjectProperties(shareableVM);
            //string savePath = _env.ContentRootPath + _config.GetValue<string>("ImageSavePath");

            //Save selected images to filesystem
            string savePath = _config.GetValue<string>("ImageSavePath");
            List<FilePath> files = await Util.ControllerUtil.SaveFilesToDisk(Request.Form.Files, savePath);

            int activeUserId = Convert.ToInt32(HttpContext.User.Identity.Name);
            Shareable shareable = new Shareable
            {
                Title = shareableVM.Title,
                Introduction = shareableVM.Introduction,
                DateTimeStamp = DateTime.Now,
                UserAccountId = activeUserId,
                Paragraphs = shareableVM.Paragraphs,
                FilePaths = files
            };

            //save entity to DB with EF repo (there are multiple repo types)
            var repo = new RepositoryEF(_context);
            string status = repo.Create(shareable);

            if (status == Constants.SUCCESS)
            {
                status += "_" + shareable.Title + " added successfully!";
            }

            return Json(status);

            //React "home", navigates to correct path with the address url
            // return RedirectToAction("UserShareables", new { statusMessage = status });
        }

        public async Task<IActionResult> UpdateShareableAsync(Shareable update)
        {
            int activeUserId = Convert.ToInt32(HttpContext.User.Identity.Name);
            update.UserAccountId = activeUserId;
            update.DateTimeStamp = DateTime.Now;

            var repo = new RepositoryShareable();
            string status = await repo.UpdateSpecific(_context,update);
            return Json(status);

            // return RedirectToAction("UserShareables", new { status = new {message = status} });

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
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            //var shareables = _context.GetEntitySet<Shareable>().Where(s => s.UserAccountId == userId).Include(s=>s.Paragraphs).Include(s=>s.FilePaths).ToList();
            //var shareables = _context.GetEntitySet<Shareable>().Where(s => s.UserAccountId == userId).ToList();
            string[] dummyFilePath = new string[] { "/images/kobe.jpeg" };
            var shareables = _context.GetEntitySet<Shareable>().Where(s => s.UserAccountId == userId).Select(shareable => new
            {
                shareable.Id,
                shareable.Title,
                shareable.Introduction,
                shareable.DateTimeStamp,
                shareable.UserAccountId,
                // Paragraphs = shareable.Paragraphs,
                // FilePaths = shareable.FilePaths

                //more specific:
                Paragraphs = shareable.Paragraphs.Select(p => new
                {
                    p.Text
                }),
                FilePaths = shareable.FilePaths.Select(f => new
                {
                    f.Path
                })

            }).ToList();

            Util.Log("How many shareables of user - ", shareables.Count);
            return Json(shareables);
        }

    }
}