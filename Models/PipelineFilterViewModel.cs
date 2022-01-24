using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class PipelineFilterViewModel
    {

        public string Segment { get; set; }
        public string AccountManagerId { get; set; }
        public string ClassificationId { get; set; }
        public string StatusId { get; set; }
        public string  CustomerId { get; set; }
        public string  OrderMonthId { get; set; }
        public string No { get; set; }
        public string ProjectName { get; set; }
        public List<Pipeline> Pipelines { get; set; }

    }
}