using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApi.Core;

namespace WebApi.Controllers
{
    public class SearchController : Controller
    {
        private ISystemConfig config = Ioc.GetConfig();
        public IActionResult Search()
        {
            ViewBag.UserName = config.Authorization.UserName;
            ViewBag.Password = config.Authorization.Password;
            return View();
        }
    }
}
