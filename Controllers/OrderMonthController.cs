using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public class OrderMonthController : Controller
    {
        private SalesTrackContext db;
        public OrderMonthController()
        {
            this.db = new SalesTrackContext();
        }

        
        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var orderMonths = db.OrderMonths
                .OrderBy(om=>om.Month)
                .ToList<OrderMonth>();
            
            return View(orderMonths);
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

            var orderMonth = db.OrderMonths.Find(id);
            return View(orderMonth);
        }


        public ActionResult Save(OrderMonth orderMonth)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                orderMonth.ID = Guid.NewGuid();
                orderMonth.CreatedDate = DateTime.Now;
                db.OrderMonths.Add(orderMonth);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Add", orderMonth);
        }

        public ActionResult Update(OrderMonth orderMonth)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var original = db.OrderMonths.Find(orderMonth.ID);

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(orderMonth);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View("Edit", orderMonth);
        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var orderMonth = db.OrderMonths.Find(id);
            db.OrderMonths.Remove(orderMonth);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}