using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class ChartViewModel
    {
        public int Month { get; set; }
        public string Label { get; set; }
        public decimal Data { get; set; }
        public decimal Actual { get; set; }
    }
}