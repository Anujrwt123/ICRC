using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;           

namespace IC_RC.ViewModels
{
    public class CertificationViewModel
    {
        public int ID { get; set; }
        public Nullable<int> CertID { get; set; }
        public Nullable<int> certificateNo { get; set; }
        public Nullable<int> IssueBoard { get; set; }
        public string BoardCertificateNumber { get; set; }
        public string BoardCertificateAcronym { get; set; }
        public Nullable<DateTime> CertIssueDate { get; set; }
        public Nullable<DateTime> RecertDate { get; set; }
        public Nullable<DateTime> CertRequestDate { get; set; }
        public Nullable<double> CertRequestFee { get; set; }
        public string PaymentNumber { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public string CertNotes { get; set; }
        public Nullable<bool> AddToPrintQueues { get; set; }
        public Nullable<int> PersonID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public string NoteCreatedBy { get; set; }
        public Nullable<DateTime> NoteCreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }
        

       
        public string BoardCertificateAcronymName { get; set; }
       
        public string IssueBoardAcronym { get; set; }
       
        public string CertificateAcronym { get; set; }

               public string PersonName { get; set; }
    }
}