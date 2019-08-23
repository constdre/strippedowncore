using ASPracticeCore.Models;
using ASPracticeCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class UserController:Controller 
    {
        public async Task<IActionResult> GetUserShareables(int id)
        {
            //test this
            Repository repo = new Repository();
            var shareables = await repo.GetItemsOfOwner<Shareable>("useraccount", "shareable", id);
            return Json(shareables);
        }
        [Route("Accounts/User/ManageProfile")]
        public IActionResult ManageProfile(int id)
        {
            ViewBag.UserId = id;
            return View();
        }
    }
}
