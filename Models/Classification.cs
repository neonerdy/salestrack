using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class Classification
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string ClassificationCode { get; set; }

        [Required(ErrorMessage = "Classification name is required")]
        public string ClassificationName { get; set; }
    }
}