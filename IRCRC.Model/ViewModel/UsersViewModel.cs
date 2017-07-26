using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCRC.Model.ViewModel
{
    public class UsersViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int IsICRCMember { get; set; }
        
        public int BoardID { get; set; }
        public bool Active { get; set; }


        public string BoardName { get; set; }
    }
}
