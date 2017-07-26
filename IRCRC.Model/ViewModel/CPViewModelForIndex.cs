using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model.ViewModel
{
    public class CPViewModelForIndex
    {
        public int ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CredentialType { get; set; }
        public Nullable<int> CurrentBoardID { get; set; }
        public string BoardAcronym { get; set; }
        public int? CertID { get; set; }
        public int? certificateNo { get; set; }
        public string Name{ get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? RecertDate { get; set; }
        }
}
