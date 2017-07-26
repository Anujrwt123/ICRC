using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model.ViewModel
{
    public class StudentViolationForIndex
    {
        public int ID { get; set; }
        public int Board { get; set; }
        public string BoardName { get; set; }

        public string PersonName { get; set; }
        public int Person { get; set; }
        public DateTime? Date { get; set; }
        public bool IsScanned { get; set; }
        public bool IsLetterSent { get; set; }

        public bool IsSharable { get; set; }

        public string Notes { get; set; }

        public string EthicalViolation { get; set; }
    }
}
