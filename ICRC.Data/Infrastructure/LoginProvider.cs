using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Infrastructure
{
    public class LoginProvider
    {
        private ICRCEntities dataContext;

        private DbFactory _DbFactory;
        protected IDbFactory DbFactory
        {
            get
            {
                return _DbFactory == null ? (_DbFactory = new DbFactory()) : _DbFactory;
            }



        }

        protected ICRCEntities DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }


        public bool ValidateUser(string username, string password)
        {
            var data = DbContext.Users.Where(x => x.Username == username && x.Password == password).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsUsernameExists(string username)
        {
            var data = DbContext.Users.Where(x => x.Username == username).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }

        public Users GetUser(string username)
        {

            return DbContext.Users.Where(x => x.Username == username).FirstOrDefault();
        } 


        public int RegisterUser(Users user)
        {
            DbContext.Users.Add(user);
            return DbContext.SaveChanges();

        }
    }
}
