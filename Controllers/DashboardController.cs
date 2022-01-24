using SalesTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SalesTrack.Controllers
{
    public class DashboardController : Controller
    {


        private SalesTrackContext db;
        private int activeYear;
        public DashboardController()
        {
            this.db = new SalesTrackContext();

            var setting = db.Settings.Where(s => s.SettingName == "Active_Year").SingleOrDefault<Setting>();
            activeYear = Convert.ToInt32(setting.SettingValue);
        }


        public ActionResult Index()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }


            RenderDashboard();

            List<ChartViewModel> quotationsByAccountManagers = QuotationByAccountManager();
            ViewBag.Performances = quotationsByAccountManagers;
            ViewBag.ActiveYear = activeYear;

            CalculateTargetPipeline1();
            CalculateTargetPipeline2();

            return View();
        }

        private void CalculateTargetPipeline1()
        {
            decimal A = 0;
            decimal B = 0;
            decimal C = 0;
            decimal D = 0;

           
           int ACount = db.Pipelines
                .Where(p => p.Status.StatusName == "A1" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();

            int BCount = db.Pipelines
                .Where(p => p.Status.StatusName == "B" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();

            int CCount = db.Pipelines
                .Where(p => p.Status.StatusName == "C" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();

            int DCount = db.Pipelines
                .Where(p => p.Status.StatusName == "D" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();


            if (ACount > 0)
            {
                A = db.Pipelines
                    .Where(p => p.Status.StatusName == "A1" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                    )
                    .Sum(p => p.Quotation);
            }

            if (BCount > 0)
            {
                B = db.Pipelines
                    .Where(p => p.Status.StatusName == "B" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                    )
                    .Sum(p => p.Quotation);
            }

            if (CCount > 0)
            {
                C = db.Pipelines
                    .Where(p => p.Status.StatusName == "C" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                    )
                    .Sum(p => p.Quotation);
            }

            if (DCount > 0)
            {
                D = db.Pipelines
                    .Where(p => p.Status.StatusName == "D" && (
                           (p.OrderMonth.Month >= 1 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 6 && p.OrderMonth.Year == activeYear)
                        )
                    )
                   .Sum(p => p.Quotation);
            }


            var A1 = A;
            var B1 = A1 + B;
            var C1 = B1 + C;
            var D1 = C1 + D;

            var targetSetting1 = db.Settings.Where(s => s.SettingName == "2022_1H_Target").SingleOrDefault<Setting>();
            List<ChartViewModel> targetPipelines = new List<ChartViewModel>();

            targetPipelines.Add(new ChartViewModel { Label = "A0", Data = 0, Actual = 0 });
            targetPipelines.Add(new ChartViewModel { Label = "~A1", Data = A1, Actual = Math.Round(A1 / Convert.ToDecimal(targetSetting1.SettingValue) * 100, 2) });
            targetPipelines.Add(new ChartViewModel { Label = "~B1", Data = B1, Actual = Math.Round(B1 / Convert.ToDecimal(targetSetting1.SettingValue) * 100, 2) });
            targetPipelines.Add(new ChartViewModel { Label = "~C1", Data = C1, Actual = Math.Round(C1 / Convert.ToDecimal(targetSetting1.SettingValue) * 100, 2) });
            targetPipelines.Add(new ChartViewModel { Label = "D", Data = D1, Actual = Math.Round(D1 / Convert.ToDecimal(targetSetting1.SettingValue) * 100, 2) });

            ViewBag.TargetSetting1 = Convert.ToDecimal(targetSetting1.SettingValue).ToString("N0");
            ViewBag.TargetPipelines1 = targetPipelines;
        }



        private void CalculateTargetPipeline2()
        {
            decimal A = 0;
            decimal B = 0;
            decimal C = 0;
            decimal D = 0;

            int ACount = db.Pipelines
                .Where(p => p.Status.StatusName == "A1" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();

            int BCount = db.Pipelines
                .Where(p => p.Status.StatusName == "B" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();

            int CCount = db.Pipelines
                .Where(p => p.Status.StatusName == "C" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();

            int DCount = db.Pipelines
                .Where(p => p.Status.StatusName == "D" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                 ).Count();


            if (ACount > 0)
            {
                A = db.Pipelines
                    .Where(p => p.Status.StatusName == "A1" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                    )
                    .Sum(p => p.Quotation);
            }

            if (BCount > 0)
            {
                B = db.Pipelines
                    .Where(p => p.Status.StatusName == "B" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                    )
                    .Sum(p => p.Quotation);
            }

            if (CCount > 0)
            {
                C = db.Pipelines
                    .Where(p => p.Status.StatusName == "C" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                    )
                    .Sum(p => p.Quotation);
            }

            if (DCount > 0)
            {
                D = db.Pipelines
                    .Where(p => p.Status.StatusName == "D" && (
                           (p.OrderMonth.Month >= 7 && p.OrderMonth.Year == activeYear) && (p.OrderMonth.Month <= 12 && p.OrderMonth.Year == activeYear)
                        )
                    )
                   .Sum(p => p.Quotation);
            }


            var A1 = A;
            var B1 = A1 + B;
            var C1 = B1 + C;
            var D1 = C1 + D;

            var targetSetting2 = db.Settings.Where(s => s.SettingName == "2022_1H_Target").SingleOrDefault<Setting>();
          
            List<ChartViewModel> targetPipelines = new List<ChartViewModel>();

            targetPipelines.Add(new ChartViewModel { Label = "A0", Data = 0, Actual = 0 });
            targetPipelines.Add(new ChartViewModel { Label = "~A1", Data = A1, Actual = Math.Round(A1 / Convert.ToDecimal(targetSetting2.SettingValue) * 100, 2) });
            targetPipelines.Add(new ChartViewModel { Label = "~B1", Data = B1, Actual = Math.Round(B1 / Convert.ToDecimal(targetSetting2.SettingValue) * 100, 2) });
            targetPipelines.Add(new ChartViewModel { Label = "~C1", Data = C1, Actual = Math.Round(C1 / Convert.ToDecimal(targetSetting2.SettingValue) * 100, 2) });
            targetPipelines.Add(new ChartViewModel { Label = "D", Data = D1, Actual = Math.Round(D1 / Convert.ToDecimal(targetSetting2.SettingValue) * 100, 2) });

            ViewBag.TargetSetting2 = Convert.ToDecimal(targetSetting2.SettingValue).ToString("N0");
            ViewBag.TargetPipelines2 = targetPipelines;

        }



        public ActionResult QuotationByMonth()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var quotations = db.Pipelines
                .Include(p=>p.OrderMonth)
                .Where(p=>p.Year == activeYear)
                .GroupBy(p => p.OrderMonthId)
                .Select(cvm => new ChartViewModel
                {
                    Month = cvm.FirstOrDefault().OrderMonth.Month,
                    Label = cvm.FirstOrDefault().OrderMonth.Name,
                    Data = cvm.Sum(x => x.Quotation)
                })
                .OrderBy(p=>p.Month)
                .ToList<ChartViewModel>();


            return Json(quotations, JsonRequestBehavior.AllowGet);
        }


        public ActionResult QuotationByStatus()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var quotations = db.Pipelines
                 .Where(p => p.Year == activeYear)
                .GroupBy(p => p.StatusId)
                .Select(cvm => new ChartViewModel
                {
                    Label = cvm.FirstOrDefault().Status.StatusName,
                    Data = cvm.Sum(x => x.Quotation)
                })
                .ToList<ChartViewModel>();


            return Json(quotations, JsonRequestBehavior.AllowGet);
        }


        public ActionResult QuotationBySegment()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var quotations = db.Pipelines
             .Where(p => p.Year == activeYear)
            .GroupBy(p => p.Segment)
            .Select(cvm => new ChartViewModel
            {
                Label = cvm.FirstOrDefault().Segment,
                Data = cvm.Sum(x => x.Quotation)
            })
            .ToList<ChartViewModel>();


            return Json(quotations, JsonRequestBehavior.AllowGet);
        }



        public ActionResult QuotationByClassification()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var quotations = db.Pipelines
                .Where(p => p.Year == activeYear)
                .GroupBy(p => p.ClassificationId)
                .Select(cvm => new ChartViewModel
                {
                    Label = cvm.FirstOrDefault().Classification.ClassificationCode + " " 
                        + cvm.FirstOrDefault().Classification.ClassificationName,
                    Data = cvm.Sum(x => x.Quotation)
                })
                .ToList<ChartViewModel>();

           return Json(quotations, JsonRequestBehavior.AllowGet);
        }



        private List<ChartViewModel> QuotationByAccountManager()
        {
            var quotations = db.Pipelines
                 .Include(p=>p.AccountManager)
                 .Where(p => p.Year == activeYear)
                 .GroupBy(p => p.AccountManagerId)
                .Select(cvm => new ChartViewModel
                {
                    Label = cvm.FirstOrDefault().AccountManager.EmployeeName,
                    Data = cvm.Sum(x => x.Quotation),
                    Actual = cvm.Sum(x => x.Quotation) / cvm.FirstOrDefault().AccountManager.SalesTarget * 100
                })
                .OrderBy(p=>p.Label)
                .ToList<ChartViewModel>();

            return quotations;
        }


       

        private void RenderDashboard()
        {
            int projectCount = db.Pipelines
                .Where(p=>p.Year == activeYear)
                .Count();
            
            int accountManagerCount = db.Pipelines
                .Where(p => p.Year == activeYear)
                .Select(p=>p.AccountManagerId).Distinct().Count();
            
            decimal quotation = db.Pipelines
                .Where(p => p.Year == activeYear)
                .Sum(p => p.Quotation);

            int customerCount = db.Pipelines
                .Where(p => p.Year == activeYear)
                .Select(p => p.CustomerId).Distinct().Count();

            ViewBag.ProjectCount = projectCount;
            ViewBag.CustomerCount = customerCount;
            ViewBag.AccountManagerCount = accountManagerCount;
            ViewBag.Quotation = quotation;
        }


    }
}