using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public class CustomerController : Controller
    {
        private SalesTrackContext db;
        public CustomerController()
        {
            this.db = new SalesTrackContext();
        }

        
        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customers = db.Customers.ToList<Customer>();
            return View(customers);
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

            var customer = db.Customers.Find(id);
            return View(customer);
        }


        public ActionResult Save(Customer customer)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                customer.ID = Guid.NewGuid();
                db.Customers.Add(customer);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Add", customer);
        }

        public ActionResult Update(Customer customer)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var original = db.Customers.Find(customer.ID);

                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(customer);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View("Edit", customer);
        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}