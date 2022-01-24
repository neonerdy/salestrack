using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class OrderMonth
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Year is required")]
        public int Year { get; set; }
        
        [Required(ErrorMessage = "Month is required")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}