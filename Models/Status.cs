using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class Status
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Status name is required")]
        public string StatusName { get; set; }
        public string Possibility { get; set; }

        [Required(ErrorMessage = "Definition is required")]
        public string Definition { get; set; }
    }
}