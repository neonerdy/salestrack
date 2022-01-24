using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class Pipeline
    {
        public Guid ID { get; set; }
        public string ProjectName { get; set; }
        public string Segment { get; set; }
        public string No { get; set; }
        public Guid AccountManagerId { get; set; }
        public AccountManager AccountManager { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid ClassificationId { get; set; }
        public Classification Classification { get; set; }
        public Guid StatusId { get; set; }
        public Status Status { get; set; }
        public int Year { get; set; }
        public Guid OrderMonthId { get; set; }
        public OrderMonth OrderMonth { get; set; }
        public decimal Quotation { get; set; }
        public DateTime CreatedDate { get; set;  }
        public string Notes { get; set; }

        //Jan
        public decimal JanRevenue { get; set;}
        public decimal JanExternalCost { get; set; }
        public decimal JanInternalCost { get; set; }
        public decimal JanMarginalProfit { get; set; }

        //Feb
        public decimal FebRevenue { get; set; }
        public decimal FebExternalCost { get; set; }
        public decimal FebInternalCost { get; set; }
        public decimal FebMarginalProfit { get; set; }

        //Mar
        public decimal MarRevenue { get; set; }
        public decimal MarExternalCost { get; set; }
        public decimal MarInternalCost { get; set; }
        public decimal MarMarginalProfit { get; set; }

        //Apr
        public decimal AprRevenue { get; set; }
        public decimal AprExternalCost { get; set; }
        public decimal AprInternalCost { get; set; }
        public decimal AprMarginalProfit { get; set; }

        //May
        public decimal MayRevenue { get; set; }
        public decimal MayExternalCost { get; set; }
        public decimal MayInternalCost { get; set; }
        public decimal MayMarginalProfit { get; set; }

        //Jun
        public decimal JunRevenue { get; set; }
        public decimal JunExternalCost { get; set; }
        public decimal JunInternalCost { get; set; }
        public decimal JunMarginalProfit { get; set; }

        //Jul
        public decimal JulRevenue { get; set; }
        public decimal JulExternalCost { get; set; }
        public decimal JulInternalCost { get; set; }
        public decimal JulMarginalProfit { get; set; }

        //Aug
        public decimal AugRevenue { get; set; }
        public decimal AugExternalCost { get; set; }
        public decimal AugInternalCost { get; set; }
        public decimal AugMarginalProfit { get; set; }

        //Sep
        public decimal SeptRevenue { get; set; }
        public decimal SeptExternalCost { get; set; }
        public decimal SeptInternalCost { get; set; }
        public decimal SeptMarginalProfit { get; set; }

        //Oct
        public decimal OctRevenue { get; set; }
        public decimal OctExternalCost { get; set; }
        public decimal OctInternalCost { get; set; }
        public decimal OctMarginalProfit { get; set; }

        //Nov
        public decimal NovRevenue { get; set; }
        public decimal NovExternalCost { get; set; }
        public decimal NovInternalCost { get; set; }
        public decimal NovMarginalProfit { get; set; }

        //Dec
        public decimal DecRevenue { get; set; }
        public decimal DecExternalCost { get; set; }
        public decimal DecInternalCost { get; set; }
        public decimal DecMarginalProfit { get; set; }

        //Total
        public decimal TotalRevenue { get; set; }
        public decimal TotalExternalCost { get; set; }
        public decimal TotalInternalCost { get; set; }
        public decimal TotalMarginalProfit { get; set; }



    }
}