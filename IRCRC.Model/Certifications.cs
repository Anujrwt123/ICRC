using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    public class Certifications
    {
        public int ID { get; set; }
        [DisplayName ("IC&RC Acronym")]
        public Nullable<int> CertID { get; set; }
        [DisplayName("Certificate Number")]
        public Nullable<int> certificateNo { get; set; }
        [DisplayName("Issuing Board")]
        public Nullable<int> IssueBoard { get; set; }

        [DisplayName("Board's Certificate Number")]
        public string BoardCertificateNumber { get; set; }
      
        [DisplayName("Certificate Issued Date")]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:d}")]
        public Nullable<DateTime> CertIssueDate { get; set; }
        [DisplayName("Expiration Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<DateTime> RecertDate { get; set; }
        [DisplayName("Certificate Request Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<DateTime> CertRequestDate { get; set; }
        [DisplayName("Certificate Request Fee")]
        public Nullable<double> CertRequestFee { get; set; }
        [DisplayName("Payment Number")]
        public string PaymentNumber { get; set; }

      
        [DisplayName("Payment Type")]
        public Nullable<int> PaymentType { get; set; }
        [DisplayName("Certificate Notes")]
        public string CertNotes { get; set; }
        [DisplayName("Add To Print Queues")]
        public Nullable<bool> AddToPrintQueues { get; set; }
        [DisplayName("Person")]
        public int? PersonID{ get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        [DisplayName("Board's Certifiate Acronym")]
        public string BoardCertificateAcronym { get; set; }
        public Nullable<DateTime> NoteCreatedAt { get; set; }
        public string NoteCreatedBy { get; set; }
     
        [NotMapped]
        public CertifiedPersons PersonOBJ { get; set; }
        [NotMapped]
        public string IssueBoardAcronym { get; set; }
        [NotMapped]
        public string CertificateAcronym {get;set;}
        [NotMapped]
        public string PersonName { get; set; }      

        [NotMapped]
        public string BoardCertificateAcronymName { get; set; }
    }
}
