using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model.ViewModel
{
    public class CertificationViewModelForIndex
    {
        public int ID { get; set; }
        public int? CertID { get; set; }
        public int CertificateNumber { get; set; }
        public DateTime? CertIssueDate { get; set; }
        public DateTime? RecertDate { get; set; }
        public int PersonID { get; set; }

        public int IssueBoard { get; set; }

        public bool? AddToPrintQueues { get; set; }

        public string CertificateAcronym { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public string Acronym { get; set; }
        public string address
        {
            get; set;
        }
    }
}
