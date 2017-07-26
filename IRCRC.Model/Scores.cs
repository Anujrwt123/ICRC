using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    public class Scores
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public DateTime ExamDate { get; set; }
        public bool Status { get; set; }
        public double ScaledScore { get; set; }
        public int ExamID { get; set; }
        public int TestingCompany { get; set; }
        public int Board { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedAt { get; set; }

        [NotMapped]
        public string ExamName { get; set; }
        [NotMapped]
        public string BoardName { get; set; }
        [NotMapped]
        public string PersonName { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
    }
}
