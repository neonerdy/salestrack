using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public class StatusController : Controller
    {
        private SalesTrackContext db;
        public StatusController()
        {
            this.db = new SalesTrackContext();
        }

        
        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var statuses = db.Statuses
                .OrderBy(s=>s.StatusName)
                .ToList<Status>();
           
            return View(statuses);
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

            var status = db.Statuses.Find(id);
            return View(status);
        }


        public ActionResult Save(Status status)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                status.ID = Guid.NewGuid();
                db.Statuses.Add(status);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("Add", status);
        }


        public ActionResult Update(Status status)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var original = db.Statuses.Find(status.ID);

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(status);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View("Edit", status);
        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var status = db.Statuses.Find(id);
            db.Statuses.Remove(status);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}