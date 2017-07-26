using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    [Table("TblFileMaker")]
    public class FileMaker
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string currentBoard { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<DateTime> RecertDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<DateTime> Issueddate { get; set; }
        public string Acronym { get; set; }
        public Nullable<int> CertID { get; set; }
        public Nullable<DateTime> dateofEntry { get; set; }
        public string HomePhone { get; set; }
        public string OtherBoards { get; set; }
        public Nullable<DateTime> Renewal { get; set; }
        
        public string WorkPhone { get; set; }
        [NotMapped]
        public string FullName { get; set; }


    }
}
