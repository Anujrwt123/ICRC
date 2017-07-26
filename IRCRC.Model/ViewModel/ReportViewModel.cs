using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model.ViewModel
{
    public class ReportViewModel
    {
        public int CertifiedPersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime RenewalDate { get; set; }
        public int CertificateNumber{ get; set; }
        public string CurrentBoardName{ get; set; }
    }
}
