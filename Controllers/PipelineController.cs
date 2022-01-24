using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPOI.SS.UserModel;
using SalesTrack.Models;

namespace SalesTrack.Controllers
{
    public enum ViewMode
    {
        Add,Search
    }

    public class PipelineController : Controller
    {
        private SalesTrackContext db;
        private int activeYear;
       
        public PipelineController()
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

            string role = Session["Role"].ToString();
            string userId = Session["UserId"].ToString();

            List<Pipeline> pipelines = new List<Pipeline>();

            if (role == "Admin")
            {
                pipelines = db.Pipelines
                   .Include(p => p.AccountManager)
                   .Include(p => p.Customer)
                   .Include(p => p.OrderMonth)
                   .Include(p => p.Classification)
                   .Include(p => p.Status)
                   .OrderByDescending(p => p.CreatedDate)
                   .ToList<Pipeline>();
            } 
            else
            {
                pipelines = db.Pipelines
                    .Include(p => p.AccountManager)
                    .Include(p => p.Customer)
                    .Include(p => p.OrderMonth)
                    .Include(p => p.Classification)
                    .Include(p => p.Status)
                    .Where(p=>p.AccountManagerId == new Guid(userId))
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList<Pipeline>();
            }

            RenderDropDown(ViewMode.Search);
           
            var pvm = new PipelineFilterViewModel();
            pvm.Pipelines = pipelines;

            return View(pvm);
        }


        private void RenderDropDown(ViewMode viewMode)
        {
            string role = Session["Role"].ToString();
            string userId = Session["UserId"].ToString();

            List<SelectListItem> segments = db.Segments.Select(x => new SelectListItem()
            {
                Value = x.SegmentName,
                Text = x.SegmentName
            }).ToList();

            
            List<SelectListItem> accountManagers = db.AccountManagers.OrderBy(am => am.EmployeeName).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.EmployeeName,
            }).ToList();

            if (viewMode == ViewMode.Add)
            {
                var user = accountManagers.Where(x => x.Value == userId).First();
                user.Selected = true;
            } 
         
            else if (viewMode == ViewMode.Search && role == "User")
            {
                var user = accountManagers.Where(x => x.Value == userId).First();
                user.Selected = true;
            }

           
            List<SelectListItem> customers = db.Customers.OrderBy(c => c.CustomerName).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.CustomerName
            }).ToList();


            List<SelectListItem> classifications = db.Classifications.OrderBy(c => c.ClassificationName).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.ClassificationCode + " " + x.ClassificationName
            }).ToList();

            List<SelectListItem> statuses = db.Statuses.OrderBy(s => s.StatusName).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.StatusName
            }).ToList();

            List<SelectListItem> orderMonths = db.OrderMonths.Where(om => om.Year == activeYear)
                .OrderBy(om => om.CreatedDate).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.Name,
            }).ToList();


            segments.Insert(0, new SelectListItem() { Value = string.Empty, Text = "(Select Segment)" });
            accountManagers.Insert(0, new SelectListItem() { Value = "", Text = "(Select Account Manager)" });
            customers.Insert(0, new SelectListItem() { Value = "", Text = "(Select Customer)" });
            classifications.Insert(0, new SelectListItem() { Value = "", Text = "(Select Classification)" });
            statuses.Insert(0, new SelectListItem() { Value = "", Text = "(Select Status)" });
            orderMonths.Insert(0, new SelectListItem() { Value = "", Text = "(Select Order)" });

            ViewBag.Segments = segments;
            ViewBag.AccountManagers = accountManagers;
            ViewBag.Customers = customers;
            ViewBag.Classifications = classifications;
            ViewBag.Statuses = statuses;
            ViewBag.OrderMonths = orderMonths;


        }


        public ActionResult Add()
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            RenderDropDown(ViewMode.Add);
            return View();
        }


        public ActionResult Edit(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pipeline = db.Pipelines.Find(id);
            RenderDropDown(ViewMode.Add);

            return View(pipeline);
        }

        public ActionResult Detail(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pipeline = db.Pipelines
                .Include(p => p.AccountManager)
                .Include(p => p.Customer)
                .Include(p => p.OrderMonth)
                .Include(p => p.Classification)
                .Include(p => p.Status)
                .Where(p => p.ID == id).SingleOrDefault<Pipeline>();

            return View(pipeline);
        }


        private void SanitizeFilter(PipelineFilterViewModel pvm)
        {
            if (pvm.Segment == null)
            {
                pvm.Segment = "";
            }
            if (pvm.AccountManagerId == null)
            {
                pvm.AccountManagerId = "";
            }
            if (pvm.ClassificationId == null)
            {
                pvm.ClassificationId = "";
            }
            if (pvm.StatusId == null)
            {
                pvm.StatusId = "";
            }
            if (pvm.CustomerId == null)
            {
                pvm.CustomerId = "";
            }
            if (pvm.OrderMonthId == null)
            {
                pvm.OrderMonthId = "";
            }
            if (pvm.No == null)
            {
                pvm.No = "";
            }
            if (pvm.ProjectName == null)
            {
                pvm.ProjectName = "";
            }


        }


       
        public ActionResult Filter(PipelineFilterViewModel pvm)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            SanitizeFilter(pvm);

            string role = Session["Role"].ToString();
            string userId = Session["UserId"].ToString();
            List<Pipeline> pipelines = new List<Pipeline>();

            if (role == "Admin")
            {
                pipelines = db.Pipelines
                    .Include(p => p.AccountManager)
                    .Include(p => p.Customer)
                    .Include(p => p.OrderMonth)
                    .Include(p => p.Classification)
                    .Include(p => p.Status)
                    .Where(p => p.Segment.Contains(pvm.Segment)
                       && p.AccountManagerId.ToString().Contains(pvm.AccountManagerId)
                       && p.ClassificationId.ToString().Contains(pvm.ClassificationId)
                       && p.StatusId.ToString().Contains(pvm.StatusId)
                       && p.CustomerId.ToString().Contains(pvm.CustomerId)
                       && p.OrderMonthId.ToString().Contains(pvm.OrderMonthId)
                       && p.No.Contains(pvm.No)
                       && p.ProjectName.Contains(pvm.ProjectName)
                    )
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList<Pipeline>();
            } 
            else
            {
                pipelines = db.Pipelines
                    .Include(p => p.AccountManager)
                    .Include(p => p.Customer)
                    .Include(p => p.OrderMonth)
                    .Include(p => p.Classification)
                    .Include(p => p.Status)
                    .Where(p => p.Segment.Contains(pvm.Segment)
                        && p.AccountManagerId.ToString().Contains(pvm.AccountManagerId)
                        && p.ClassificationId.ToString().Contains(pvm.ClassificationId)
                        && p.StatusId.ToString().Contains(pvm.StatusId)
                        && p.CustomerId.ToString().Contains(pvm.CustomerId)
                        && p.OrderMonthId.ToString().Contains(pvm.OrderMonthId)
                        && p.No.Contains(pvm.No)
                        && p.ProjectName.Contains(pvm.ProjectName)
                    )
                    .Where(p=>p.AccountManagerId == new Guid(userId))
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList<Pipeline>();

            }

            RenderDropDown(ViewMode.Search);

            var viewModel = new PipelineFilterViewModel();
            viewModel.Pipelines = pipelines;

            return View("Index", viewModel);
        }


        public ActionResult Download(PipelineFilterViewModel pvm)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            SanitizeFilter(pvm);
            
            string role = Session["Role"].ToString();
            string userId = Session["UserId"].ToString();
            List<PipelineXlsViewModel> pipelines = new List<PipelineXlsViewModel>();

            if (role == "Admin")
            {
                pipelines = db.Pipelines
                    .Include(p => p.AccountManager)
                    .Include(p => p.Customer)
                    .Include(p => p.OrderMonth)
                    .Include(p => p.Classification)
                    .Include(p => p.Status)
                    .Where(p => p.Segment.Contains(pvm.Segment)
                       && p.AccountManagerId.ToString().Contains(pvm.AccountManagerId)
                       && p.ClassificationId.ToString().Contains(pvm.ClassificationId)
                       && p.StatusId.ToString().Contains(pvm.StatusId)
                       && p.CustomerId.ToString().Contains(pvm.CustomerId)
                       && p.OrderMonthId.ToString().Contains(pvm.OrderMonthId)
                       && p.No.Contains(pvm.No)
                       && p.ProjectName.Contains(pvm.ProjectName)
                     )
                    .Select(p => new PipelineXlsViewModel
                    {
                        No = p.No,
                        Segment = p.Segment,
                        ProjectName = p.ProjectName,
                        AccountManager = p.AccountManager.EmployeeName,
                        Classification = p.Classification.ClassificationName,
                        Status = p.Status.StatusName,
                        CustomerName = p.Customer.CustomerName,
                        OrderMonth = p.OrderMonth.Name,
                        Quotation = p.Quotation,
                        JanRevenue = p.JanRevenue,
                        JanExternalCost = p.JanExternalCost,
                        JanInternalCost = p.JanInternalCost,
                        JanMarginalProfit = p.JanMarginalProfit,
                        FebRevenue = p.FebRevenue,
                        FebExternalCost = p.FebExternalCost,
                        FebInternalCost = p.FebInternalCost,
                        FebMarginalProfit = p.FebMarginalProfit,
                        MarRevenue = p.MarRevenue,
                        MarExternalCost = p.MarExternalCost,
                        MarInternalCost = p.MarInternalCost,
                        MarMarginalProfit = p.MarMarginalProfit,
                        AprRevenue = p.AprRevenue,
                        AprExternalCost = p.AprExternalCost,
                        AprInternalCost = p.AprInternalCost,
                        AprMarginalProfit = p.AprMarginalProfit,
                        MayRevenue = p.MayRevenue,
                        MayExternalCost = p.MayExternalCost,
                        MayInternalCost = p.MayInternalCost,
                        MayMarginalProfit = p.MayMarginalProfit,
                        JunRevenue = p.JunRevenue,
                        JunExternalCost = p.JunExternalCost,
                        JunInternalCost = p.JunInternalCost,
                        JunMarginalProfit = p.JunMarginalProfit,
                        JulRevenue = p.JulRevenue,
                        JulExternalCost = p.JulExternalCost,
                        JulInternalCost = p.JulInternalCost,
                        JulMarginalProfit = p.JulMarginalProfit,
                        AugRevenue = p.AugRevenue,
                        AugExternalCost = p.AugExternalCost,
                        AugInternalCost = p.AugInternalCost,
                        AugMarginalProfit = p.AugMarginalProfit,
                        SeptRevenue = p.SeptRevenue,
                        SeptExternalCost = p.SeptExternalCost,
                        SeptInternalCost = p.SeptInternalCost,
                        SeptMarginalProfit = p.SeptMarginalProfit,
                        OctRevenue = p.OctRevenue,
                        OctExternalCost = p.OctExternalCost,
                        OctInternalCost = p.OctInternalCost,
                        OctMarginalProfit = p.OctMarginalProfit,
                        NovRevenue = p.NovRevenue,
                        NovExternalCost = p.NovExternalCost,
                        NovInternalCost = p.NovInternalCost,
                        NovMarginalProfit = p.NovMarginalProfit,
                        DecRevenue = p.DecRevenue,
                        DecExternalCost = p.DecExternalCost,
                        DecInternalCost = p.DecInternalCost,
                        DecMarginalProfit = p.DecMarginalProfit,
                        TotalRevenue = p.TotalRevenue,
                        TotalExternalCost = p.TotalExternalCost,
                        TotalInternalCost = p.TotalInternalCost,
                        TotalMarginalProfit = p.TotalMarginalProfit
                    })
                    .ToList<PipelineXlsViewModel>();
            }
            else
            {
                pipelines = db.Pipelines
                   .Include(p => p.AccountManager)
                   .Include(p => p.Customer)
                   .Include(p => p.OrderMonth)
                   .Include(p => p.Classification)
                   .Include(p => p.Status)
                   .Where(p => p.Segment.Contains(pvm.Segment)
                      && p.AccountManagerId.ToString().Contains(pvm.AccountManagerId)
                      && p.ClassificationId.ToString().Contains(pvm.ClassificationId)
                      && p.StatusId.ToString().Contains(pvm.StatusId)
                      && p.CustomerId.ToString().Contains(pvm.CustomerId)
                      && p.OrderMonthId.ToString().Contains(pvm.OrderMonthId)
                      && p.No.Contains(pvm.No)
                      && p.ProjectName.Contains(pvm.ProjectName) 
                      && p.AccountManagerId == new Guid(userId)
                    )
                   .Select(p => new PipelineXlsViewModel
                   {
                       No = p.No,
                       Segment = p.Segment,
                       ProjectName = p.ProjectName,
                       AccountManager = p.AccountManager.EmployeeName,
                       Classification = p.Classification.ClassificationName,
                       Status = p.Status.StatusName,
                       CustomerName = p.Customer.CustomerName,
                       OrderMonth = p.OrderMonth.Name,
                       Quotation = p.Quotation,
                       JanRevenue = p.JanRevenue,
                       JanExternalCost = p.JanExternalCost,
                       JanInternalCost = p.JanInternalCost,
                       JanMarginalProfit = p.JanMarginalProfit,
                       FebRevenue = p.FebRevenue,
                       FebExternalCost = p.FebExternalCost,
                       FebInternalCost = p.FebInternalCost,
                       FebMarginalProfit = p.FebMarginalProfit,
                       MarRevenue = p.MarRevenue,
                       MarExternalCost = p.MarExternalCost,
                       MarInternalCost = p.MarInternalCost,
                       MarMarginalProfit = p.MarMarginalProfit,
                       AprRevenue = p.AprRevenue,
                       AprExternalCost = p.AprExternalCost,
                       AprInternalCost = p.AprInternalCost,
                       AprMarginalProfit = p.AprMarginalProfit,
                       MayRevenue = p.MayRevenue,
                       MayExternalCost = p.MayExternalCost,
                       MayInternalCost = p.MayInternalCost,
                       MayMarginalProfit = p.MayMarginalProfit,
                       JunRevenue = p.JunRevenue,
                       JunExternalCost = p.JunExternalCost,
                       JunInternalCost = p.JunInternalCost,
                       JunMarginalProfit = p.JunMarginalProfit,
                       JulRevenue = p.JulRevenue,
                       JulExternalCost = p.JulExternalCost,
                       JulInternalCost = p.JulInternalCost,
                       JulMarginalProfit = p.JulMarginalProfit,
                       AugRevenue = p.AugRevenue,
                       AugExternalCost = p.AugExternalCost,
                       AugInternalCost = p.AugInternalCost,
                       AugMarginalProfit = p.AugMarginalProfit,
                       SeptRevenue = p.SeptRevenue,
                       SeptExternalCost = p.SeptExternalCost,
                       SeptInternalCost = p.SeptInternalCost,
                       SeptMarginalProfit = p.SeptMarginalProfit,
                       OctRevenue = p.OctRevenue,
                       OctExternalCost = p.OctExternalCost,
                       OctInternalCost = p.OctInternalCost,
                       OctMarginalProfit = p.OctMarginalProfit,
                       NovRevenue = p.NovRevenue,
                       NovExternalCost = p.NovExternalCost,
                       NovInternalCost = p.NovInternalCost,
                       NovMarginalProfit = p.NovMarginalProfit,
                       DecRevenue = p.DecRevenue,
                       DecExternalCost = p.DecExternalCost,
                       DecInternalCost = p.DecInternalCost,
                       DecMarginalProfit = p.DecMarginalProfit,
                       TotalRevenue = p.TotalRevenue,
                       TotalExternalCost = p.TotalExternalCost,
                       TotalInternalCost = p.TotalInternalCost,
                       TotalMarginalProfit = p.TotalMarginalProfit
                   })
                   .ToList<PipelineXlsViewModel>();
            }
                

            var dataTable = Extention.ConvertListToDataTable<PipelineXlsViewModel>(pipelines);

            IWorkbook workbook = Utility.WriteExcel(dataTable, "xlsx");
            string excelName = "Pipeline" + DateTime.Now.ToString("yyyyMMddHHmmss");

            var file = Utility.CreateExcelFile(workbook, excelName);
            return File(file.FileContents, file.ContentType, file.FileName);
        }


        private void CalculateTotal(Pipeline pipeline)
        {
            pipeline.JanMarginalProfit = pipeline.JanRevenue - pipeline.JanExternalCost;
            pipeline.FebMarginalProfit = pipeline.FebRevenue - pipeline.FebExternalCost;
            pipeline.MarMarginalProfit = pipeline.MarRevenue - pipeline.MarExternalCost;
            pipeline.AprMarginalProfit = pipeline.AprRevenue - pipeline.AprExternalCost;
            pipeline.MayMarginalProfit = pipeline.MayRevenue - pipeline.MayExternalCost;
            pipeline.JunMarginalProfit = pipeline.JunRevenue - pipeline.JunExternalCost;
            pipeline.JulMarginalProfit = pipeline.JulRevenue - pipeline.JulExternalCost;
            pipeline.AugMarginalProfit = pipeline.AugRevenue - pipeline.AugExternalCost;
            pipeline.SeptMarginalProfit = pipeline.SeptRevenue - pipeline.SeptExternalCost;
            pipeline.OctMarginalProfit = pipeline.OctRevenue - pipeline.OctExternalCost;
            pipeline.NovMarginalProfit = pipeline.NovRevenue - pipeline.NovExternalCost;
            pipeline.DecMarginalProfit = pipeline.DecRevenue - pipeline.DecExternalCost;

            //Total

            pipeline.TotalRevenue = pipeline.JanRevenue + pipeline.FebRevenue + pipeline.MarRevenue + pipeline.AprRevenue
                 + pipeline.MayRevenue + pipeline.JunRevenue + pipeline.JulRevenue + pipeline.AugRevenue
                 + pipeline.SeptRevenue + pipeline.OctRevenue + pipeline.NovRevenue + pipeline.DecRevenue;

            pipeline.TotalExternalCost = pipeline.JanExternalCost + pipeline.FebExternalCost + pipeline.MarExternalCost + pipeline.AprExternalCost
                + pipeline.MayExternalCost + pipeline.JunExternalCost + pipeline.JulExternalCost + pipeline.AugExternalCost
                + pipeline.SeptExternalCost + pipeline.OctExternalCost + pipeline.NovExternalCost + pipeline.DecExternalCost;

            pipeline.TotalInternalCost = pipeline.JanInternalCost + pipeline.FebInternalCost + pipeline.MarInternalCost + pipeline.AprInternalCost
                + pipeline.MayInternalCost + pipeline.JunExternalCost + pipeline.JulInternalCost + pipeline.AugInternalCost
                + pipeline.SeptInternalCost + pipeline.OctInternalCost + pipeline.NovInternalCost + pipeline.DecInternalCost;

            pipeline.TotalMarginalProfit = pipeline.JanMarginalProfit + pipeline.FebMarginalProfit + pipeline.MarMarginalProfit + pipeline.AprMarginalProfit
                + pipeline.MayMarginalProfit + pipeline.JunMarginalProfit + pipeline.JulMarginalProfit + pipeline.AugMarginalProfit
                + pipeline.SeptMarginalProfit + pipeline.OctMarginalProfit + pipeline.NovMarginalProfit + pipeline.DecMarginalProfit;


        }


        private bool ValidatePipeline(Pipeline pipeline)
        {
            bool isValid = true;

            if (pipeline.ProjectName == null)
            {
                ViewBag.ProjectNameErrMsg = "Project name is required";
                isValid = false;
            }
            if (pipeline.No == null)
            {
                ViewBag.NoErrMsg = "No is required";
                isValid = false;
            }
            if (pipeline.Segment == null)
            {
                ViewBag.SegmentErrMsg = "Segment is required";
                isValid = false;
            }
            if (pipeline.AccountManagerId == Guid.Empty)
            {
                ViewBag.AccountManagerErrMsg = "Account manager is required";
                isValid = false;
            }
            if (pipeline.CustomerId == Guid.Empty)
            {
                ViewBag.CustomerErrMsg = "Customer is required";
                isValid = false;
            }
            if (pipeline.ClassificationId == Guid.Empty)
            {
                ViewBag.ClassificationErrMsg = "Classification is required";
                isValid = false;
            }
            if (pipeline.StatusId == Guid.Empty)
            {
                ViewBag.StatusErrMsg = "Status is required";
                isValid = false;
            }
            if (pipeline.OrderMonthId == Guid.Empty)
            {
                ViewBag.OrderMonthErrMsg = "Order month is required";
                isValid = false;
            }
            if (pipeline.Quotation== 0)
            {
                ViewBag.QuotationErrMsg = "Quotation is required";
                isValid = false;
            }

            return isValid;
        }



        public ActionResult Save(Pipeline pipeline)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            bool isValid = ValidatePipeline(pipeline); 

            if (isValid)
            {
                pipeline.ID = Guid.NewGuid();
                pipeline.CreatedDate = DateTime.Now;
                pipeline.Year = DateTime.Now.Year;
                pipeline.Notes = pipeline.Notes == null ? "" : pipeline.Notes;

                CalculateTotal(pipeline);

                db.Pipelines.Add(pipeline);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            

            RenderDropDown(ViewMode.Add);
            return View("Add", pipeline);
            
        }

        public ActionResult Update(Pipeline pipeline)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }


            bool isValid = ValidatePipeline(pipeline);

            if (isValid)
            {
                var original = db.Pipelines.Find(pipeline.ID);

                if (original != null)
                {
                    CalculateTotal(pipeline);
                    pipeline.Notes = pipeline.Notes == null ? "" : pipeline.Notes;

                    db.Entry(original).CurrentValues.SetValues(pipeline);
                    db.SaveChanges();
                }
                return RedirectToAction("Detail", new { id = pipeline.ID });
            }

            RenderDropDown(ViewMode.Add);
            return View("Edit", pipeline);
        }




        public ActionResult Delete(Guid id)
        {
            if (Session.Count == 0 && Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pipeline = db.Pipelines.Find(id);
            db.Pipelines.Remove(pipeline);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}