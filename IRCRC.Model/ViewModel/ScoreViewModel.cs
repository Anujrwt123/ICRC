using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCRC.Model.ViewModel
{
    public class ScoreViewModel
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

        
        public string ExamName { get; set; }
        
        public string BoardName { get; set; }
        
        public string PersonName { get; set; }
        
        public string CompanyName { get; set; }
    }
}
