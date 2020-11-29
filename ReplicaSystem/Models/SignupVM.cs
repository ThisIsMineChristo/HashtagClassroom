using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReplicaSystem.Models
{
    public class SignupVM
    {
        [Display(Name = "Firsrt Name")]
        public string FName { get; set; }


        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Contact Number")]
        public string CellNum { get; set; }



        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        // [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}