using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCRC.Model.ViewModel
{
    public class ReciprocitiesViewModel
    {
        public int ID { get; set; }
        

        public Nullable<int> OriginatingBoard { get; set; }
        
        public Nullable<int> RequestedBoard { get; set; }
        
        public Nullable<int> ICRCCertID { get; set; }
        
        public Nullable<DateTime> DateofEntry { get; set; }
        public Nullable<bool> Status { get; set; }
        
        public Nullable<DateTime> ApprovalDate { get; set; }
        
        public Nullable<double> RecprocityFee { get; set; }
        
        public Nullable<int> PaymentType { get; set; }
        
        public string PaymentNumber { get; set; }
        public string Notes { get; set; }
        
        public Nullable<int> PersonID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }

        
        public string OrginiatingBoardName { get; set; }
        
        public string RequestedBoardName { get; set; }
        
        public string CertificationAcronym { get; set; }
        
        public string PaymentTypeName { get; set; }
        public string FirstName { get; set; }
    
        public string LastName { get; set; }
      
        public string MiddleName { get; set; }
    }
}
