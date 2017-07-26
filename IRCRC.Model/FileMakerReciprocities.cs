using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    [Table("FileMakerReciprocities")]
    public class FileMakerReciprocities
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public Nullable<DateTime> DateProcessed { get; set; }
        public string CurrentBoard { get; set; }
        public string CurrentBoardAddress1 { get; set; }
        public string CurrentBoardAddress2 { get; set; }
        public string CurrentBoardCity { get; set; }
        public string ContactC { get; set; }
        public string FullName { get; set; }
        public string RequestedBoard { get; set; }
        public string RequestedBoardAddress1 { get; set; }
        public string RequestedBoardAddress2 { get; set; }
        public string RequestedBoardCity { get; set; }
        public string RequestedBoardTelephone { get; set; }
    }
}
