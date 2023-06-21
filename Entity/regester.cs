using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace HouseMangment.Entity
{
    public class regester
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        public int phone { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }

        /// <summary>  
        /// Gets or sets to password address.  
        /// </summary>  
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "rePassword")]
        [Compare("Password")]
        public string rePassword { get; set; }

    }
}