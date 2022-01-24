using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public class SettingController : Controller
    {

        private SalesTrackContext db;
        public SettingController()
        {
            this.db = new SalesTrackContext();
        }

        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var settings = db.Settings
                .OrderBy(s=>s.SettingName)
                .ToList<Setting>();
            
            return View(settings);
        }

        public ActionResult Add()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public ActionResult Edit(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var setting = db.Settings.Find(id);
            return View(setting);
        }


        public ActionResult Save(Setting setting)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                setting.ID = Guid.NewGuid();
                db.Settings.Add(setting);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Add", setting);
        }

        public ActionResult Update(Setting setting)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var original = db.Settings.Find(setting.ID);

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(setting);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View("Edit", setting);
        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var setting = db.Settings.Find(id);
            db.Settings.Remove(setting);
            db.SaveChanges();

            return RedirectToAction("Index");
        }




    }
}