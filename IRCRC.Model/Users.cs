using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    public class Users
    {

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }


        [NotMapped]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public string BoardName { get; set; }
        [Required]
        public int IsICRCMember { get; set; }
        [Required]
        public int BoardID { get; set; }

        public bool Active { get; set; }
        public string Email { get; set; }

    }
}
