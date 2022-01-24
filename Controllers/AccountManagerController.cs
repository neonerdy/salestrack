using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public class AccountManagerController : Controller
    {
        private SalesTrackContext db;
        public AccountManagerController()
        {
            this.db = new SalesTrackContext();
        }

        
        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var accountManagers = db.AccountManagers
                .OrderBy(am=>am.EmployeeName)
                .ToList<AccountManager>();
            
            return View(accountManagers);
        }


        private void RenderDropDown()
        {
            List<SelectListItem> roles = db.Roles.Select(x => new SelectListItem()
            {
                Value = x.RoleName,
                Text = x.RoleName,
            }).ToList();

            roles.Insert(0, new SelectListItem() { Value = "", Text = "(Select Role)" });
            ViewBag.Roles = roles;
        }


        public ActionResult Add()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            RenderDropDown();
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var accountManager = db.AccountManagers.Find(id);
            RenderDropDown();

            return View(accountManager);
        }


        public ActionResult Save(AccountManager accountManager)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                accountManager.ID = Guid.NewGuid();
                db.AccountManagers.Add(accountManager);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            RenderDropDown();
            return View("Add", accountManager);
        }


        public ActionResult Update(AccountManager accountManager)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var original = db.AccountManagers.Find(accountManager.ID);

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(accountManager);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            RenderDropDown();
            return View("Edit", accountManager);

        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var accountManager = db.AccountManagers.Find(id);
            db.AccountManagers.Remove(accountManager);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}