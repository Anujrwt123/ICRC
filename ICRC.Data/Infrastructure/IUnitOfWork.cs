using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        int Commit();

        void PrintPDF(string url,string Location,string Name);

    }
}
