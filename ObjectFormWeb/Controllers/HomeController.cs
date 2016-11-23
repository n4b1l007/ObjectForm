using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObjectFormWeb.Models;

namespace ObjectFormWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var gender = new List<GenderViewModel>
            {
                new GenderViewModel {ID = 1, Name = "Male"},
                new GenderViewModel {ID = 2, Name = "Female"}
            };
            ViewBag.Gender = new SelectList(gender, "ID", "Name");

            ViewBag.Gender2 = new SelectList(gender, "ID", "Name");

            ViewBag.Name = "Nabil";

            ViewBag.Salary = new List<string>()
            {
                "fdsf",
                "sdf"
            };

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}