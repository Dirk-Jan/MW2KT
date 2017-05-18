using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MW2KT_ASP.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
