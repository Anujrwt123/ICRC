using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Model
{
    public class Certificates
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [DisplayName("Created By")]
        public Nullable<int> CreatedBy { get; set; }
         [DisplayName("Created At")]
        public Nullable<DateTime> CreatedAt { get; set; }
         [DisplayName("Modified By")]
        public Nullable<int> ModifiedBy { get; set; }
         [DisplayName("Modified At")]
        public Nullable<DateTime> ModifiedAt { get; set; }
    }
}
