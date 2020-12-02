using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReplicaSystem.Models
{
    public class LoginVM
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public int UserId { get; set; }

        public string Password { get; set; }

        public string ReturnURL { get; set; }
    }
}