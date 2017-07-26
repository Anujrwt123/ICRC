using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    public class TestScore
    {

        public int ID { get; set; }
        public Nullable<int> TestingCompany { get; set; }
        public string Exam { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string CandidateId { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<DateTime> ExamDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<int> Scale { get; set; }
        public Nullable<int> TestCode { get; set; }
        public Nullable<int> Form { get; set; }
        public string One { get; set; }
        public string Two { get; set; }
        public string Three { get; set; }
        public string Four { get; set; }
        public string Five { get; set; }
        public string Six { get; set; }
        public string Seven { get; set; }
        public string Eight { get; set; }
        public string Nine { get; set; }
        public string Ten { get; set; }
        public string Method { get; set; }
        public Nullable<int> Board { get; set; }
        public string Acronym { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string ZipPlus { get; set; }



        //        public int ID { get; set; }
        //      public string TestingCompany { get; set; }
        //      public string Exam { get; set; }
        //        public string LastName { get;set;}
        //        public string FirstName { get;set;}
        //        public string MiddleName { get;set;}
        //        public string CandidateId { get;set;}
        //        public string EmailAddress { get;set;}
        //        public string ExamDate { get;set;}
        //        public string Status { get;set;}
        //        public string Score { get;set;}
        //        public string Scale { get;set;}
        //        public string TestCode { get;set;}
        //        public string Form { get;set;}
        //        public string One {get;set;}
        //    public string Two { get;set;}
        //public string Three {get;set;}
        //      public string Four {get;set;}
        //      public string Five {get;set;}
        //      public string Six {get;set;}
        //      public string Seventh {get;set;}
        //      public string Eight {get;set;}
        //      public string Nine {get;set;}
        //      public string Ten {get;set;}
        //      public string Method { get;set;}
        //public string Board { get;set;}
        //public string Address1 { get;set;}
        //public string Address2 { get;set;}
        //public string City { get;set;}
        //public string State { get;set;}
        //public string ZipCode { get;set;}
        //public string ZipPlus { get;set;}
        
        [NotMapped]
            public string BoardsName { get; set; }

        [NotMapped]
        public string PreviousFirstName { get; set; }

        [NotMapped]
        public string PreviousLastName { get; set; }

        [NotMapped]
        public string PreviousAddress1{ get; set; }

    }
}
