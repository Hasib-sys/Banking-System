using BankSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankSoftware.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private readonly ProfileRepo repo;

        public HomeController()
        {
            repo = new ProfileRepo(); 
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProfileModel emp)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = User.Identity.Name;

                var count = repo.Add(emp, loggedInUser);
                if (count > 0)
                {
                    ViewBag.Okay = "Data Added";
                }
            }
            return RedirectToAction("Index", "Home");
          
        }

        public ActionResult GetAll()
        {
            var loggedInUser = User.Identity.Name;
            var data = repo.GetAllData(loggedInUser);
            return View(data);
        }



        public ActionResult Delete(int id)
        {
            var data = repo.DeleteData(id);
            return RedirectToAction("GetAll");
        }


    }
}