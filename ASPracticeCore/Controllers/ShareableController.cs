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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

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
            _config = config;
            _provider = provider;
            //controller does the disposing of the context
            _context = context;
        }

        public IActionResult UserShareables()
        {
            return View("Views/Shareable/UserShareables.cshtml");
        }
        public IActionResult ManageShareable()
        {
            return UserShareables();
        }
        [HttpGet]
        public IActionResult CreateShareable()
        {
            return UserShareables();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShareable(AddShareableViewModel shareableVM)
        {
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

        }

        public async Task<IActionResult> UpdateShareable(Shareable update)
        {
            string status = Constants.SUCCESS + "_" + update.Title + " was updated successfully!";

            try
            {
                int activeUserId = Convert.ToInt32(HttpContext.User.Identity.Name);
                update.UserAccountId = activeUserId;
                update.DateTimeStamp = DateTime.Now;

                var repo = new RepositoryShareable();
                await repo.UpdateExcludeImages(_context, update);

            }
            catch (Exception)
            {
                status = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
            }

            return Json(status);

        }

        public async Task<IActionResult> DeleteShareable(Shareable shareable)
        {

            Util.Log($"id: {shareable.Id}", $"title: {shareable.Title}");
            var repo = new RepositoryEF(_context);
            string status = await repo.Delete<Shareable>(shareable.Id);

            if (status == Constants.SUCCESS)
            {
                status = StatusGenerator.getSuccessMessage($"{shareable.Title} was removed from your posts");
            }
            return Json(status);
        }

        public async Task<IActionResult> GetUserShareables()
        {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            RepositoryReflection repo = new RepositoryReflection();
            var shareables = await repo.GetItemsOfOwnerAsync<Shareable>("useraccount", "shareable", userId);

            //get child properties:
            foreach (var shareable in shareables)
            {
                shareable.Paragraphs = await repo.GetItemsOfOwnerAsync<Paragraph>("shareable", "paragraph", shareable.Id) as List<Paragraph>;
                shareable.FilePaths = await repo.GetItemsOfOwnerAsync<FilePath>("shareable", "filepath", shareable.Id) as List<FilePath>;
            }

            return Json(shareables);
        }
        public IActionResult GetShareablesOfUser()
        {
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            var shareables = _context.GetEntitySet<Shareable>().Where(s => s.UserAccountId == userId).Include(s => s.Paragraphs).Include(s => s.FilePaths).ToList();
            return Json(shareables);
        }

    }
}