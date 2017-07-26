using ICRC.Data.Infrastructure;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{

    public interface IEthicalviolationsRepository:IRepository<EthicalViolation>
    {

    }


    public class EthicalviolationsRepository:RepositoryBase<EthicalViolation>,IEthicalviolationsRepository
    {
        public EthicalviolationsRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }              

        

    }
}
