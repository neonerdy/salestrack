using SalesTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesTrack.Controllers
{
    public class LoginController : Controller
    {

        private SalesTrackContext db;
        public LoginController()
        {
            this.db = new SalesTrackContext();
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SignIn(AccountManager accountManager)
        {
            var user = db.AccountManagers.Where(am => am.Email == accountManager.Email
                && am.Password == accountManager.Password).SingleOrDefault<AccountManager>();

            if (user != null)
            {
                Session["UserId"] = user.ID;
                Session["UserName"] = user.EmployeeName;
                Session["Department"] = user.Department;
                Session["Role"] = user.Role;

                return RedirectToAction("Index", "Pipeline");
            } 
            else
            {
                ViewBag.Message = "User name or password is incorrect";
                return View("Index");
            }
            
        }

        public ActionResult Logout()
        {
            Session.Contents.RemoveAll();
            return RedirectToAction("Index");
        }

    }
}