using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class Setting
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string SettingName { get; set; }
        
        [Required(ErrorMessage = "Value is required")]
        public string SettingValue { get; set; }
    }
}