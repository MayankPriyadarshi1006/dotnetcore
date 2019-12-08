using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mashi.Areas.Welcome.Controllers
{
    public class WelcomeController : Controller
    {
        //
        // GET: /Welcome/Welcome/

        public ActionResult Index()
        {
            if (Session["CUser"] != null)
            {
                return View();
            }
            else
            {
                Session["returnurl"] = Request.Url.ToString(); return Redirect("~/Home/Index");
            }
        }

    }
}
