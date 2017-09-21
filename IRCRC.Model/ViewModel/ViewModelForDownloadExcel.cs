using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCRC.Model.ViewModel
{
   public class ViewModelForDownloadExcel
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
        public Nullable<int> CurrentBoardID { get; set; }
        public string BoardAcronym { get; set; }
        public Nullable<int> CertID_ICAADC { get; set; }
        public Nullable<long> CertificateNo_ICAADC { get; set; }
        public DateTime? StartDate_ICAADC { get; set; }
        public DateTime? RecertDate_ICAADC { get; set; }
        public Nullable<int> CertID_ICADC { get; set; }
        public Nullable<long> CertificateNo_ICADC { get; set; }
        public DateTime? StartDate_ICADC { get; set; }
        public DateTime? RecertDate_ICADC { get; set; }
        public Nullable<int> CertID_ICCDP { get; set; }
        public Nullable<long> CertificateNo_ICCDP { get; set; }
        public DateTime? StartDate_ICCDP { get; set; }
        public DateTime? RecertDate_ICCDP { get; set; }

        public Nullable<int> CertID_ICCDPD { get; set; }
        public Nullable<long> CertificateNo_ICCDPD { get; set; }
        public DateTime? StartDate_ICCDPD { get; set; }
        public DateTime? RecertDate_ICCDPD { get; set; }

        public Nullable<int> CertID_ICCJP { get; set; }
        public Nullable<long> CertificateNo_ICCJP { get; set; }
        public DateTime? StartDate_ICCJP { get; set; }
        public DateTime? RecertDate_ICCJP { get; set; }

            
        public Nullable<int> CertID_ICCS { get; set; }
        public Nullable<long> CertificateNo_ICCS { get; set; }
        public DateTime? StartDate_ICCS { get; set; }
        public DateTime? RecertDate_ICCS { get; set; }


        public Nullable<int> CertID_ICPS { get; set; }
        public Nullable<long> CertificateNo_ICPS { get; set; }
        public DateTime? StartDate_ICPS { get; set; }
        public DateTime? RecertDate_ICPS { get; set; }


        public Nullable<int> CertID_ICPR { get; set; }
        public Nullable<long> CertificateNo_ICPR { get; set; }
        public DateTime? StartDate_ICPR { get; set; }
        public DateTime? RecertDate_ICPR { get; set; }
    }
}
