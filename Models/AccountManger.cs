using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SalesTrack.Models
{
    public class AccountManager
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Employee name is required")] 
        public string EmployeeName { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Sales target is required")]
        public decimal SalesTarget { get; set; }

    }
}