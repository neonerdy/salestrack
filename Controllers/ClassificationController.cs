using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public class ClassificationController : Controller
    {
        private SalesTrackContext db;
        public ClassificationController()
        {
            this.db = new SalesTrackContext();
        }

        
        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var classifications = db.Classifications
                .OrderBy(c=>c.ClassificationCode)
                .ToList<Classification>();
            
            return View(classifications);
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

            var classification = db.Classifications.Find(id);
            return View(classification);
        }


        public ActionResult Save(Classification classification)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                classification.ID = Guid.NewGuid();
                db.Classifications.Add(classification);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Add", classification);
        }

        public ActionResult Update(Classification classification)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {

                var original = db.Classifications.Find(classification.ID);

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(classification);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View("Edit", classification);
        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var classification = db.Classifications.Find(id);
            db.Classifications.Remove(classification);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}