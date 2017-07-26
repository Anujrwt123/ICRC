using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Infrastructure
{
    public class DbFactory: Disposable,IDbFactory
    {
        ICRCEntities dbContext;
        public ICRCEntities Init()
        {
            return dbContext ?? (dbContext = new ICRCEntities());
        }

        protected override void DisposeCore()
        {
            if(dbContext != null)
            {
                dbContext.Dispose();
            }
        }
          
    }
}
