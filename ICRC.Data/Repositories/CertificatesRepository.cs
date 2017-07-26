using ICRC.Data.Infrastructure;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class CertificatesRepository:RepositoryBase<Certificates>,ICertificatesRepository
    {
        public CertificatesRepository(IDbFactory dbFactory) : base(dbFactory) { }

       
    }

    public interface ICertificatesRepository:IRepository<Certificates>
    {
       
    }
}
