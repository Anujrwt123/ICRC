using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCRC.Model.ViewModel
{
   public class CertifiedPersonViewModel
    {
        public int ID { get; set; }
      
        public string LastName { get; set; }
      
        public string FirstName { get; set; }
      
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
      
        public string province { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string SSN { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
     
        public int TestingCompany { get; set; }
        
        public bool EthicalViolation { get; set; }
        
        public Nullable<int> CurrentBoardID { get; set; }
        
        public int OtherBoardID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        //[DisplayName("Board ID")]
        //public Nullable<int> Board_ID { get; set; }

        
        public string FullName { get; set; }
        public string CurrentBoardName { get; set; }

        public string BoardAcronym { get; set; }
    }
}

