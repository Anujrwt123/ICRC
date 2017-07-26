using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class ReciprocityPersonMapping
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int ReciprocityID { get; set; }
    }
}
