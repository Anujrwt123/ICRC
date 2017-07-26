using ICRC.Data.Infrastructure;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class RolesRepository:RepositoryBase<Roles>,IRolesRepository
    {
        public RolesRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

        public IEnumerable<UserRoles> GetRolesForUser(int ID)
        {
            return DbContext.UserRoles.Where(x => x.UserID == ID).ToList();
        }

    }

    public interface IRolesRepository:IRepository<Roles>
    {
        IEnumerable<UserRoles> GetRolesForUser(int ID);
         
    }
}
