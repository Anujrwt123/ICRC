using ICRC.Data.Infrastructure;
using ICRC.Model;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class UsersRepository : RepositoryBase<Users>,IUsersRepository
    {
        public UsersRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

        public string GetUserName(int ID)
        {
            return DbContext.Users.Where(x => x.ID == ID).Select(x => x.Username).FirstOrDefault();
        }

        public IEnumerable<UsersViewModel> GetAllForIndex()
        {
            return DbContext.Database.SqlQuery<UsersViewModel>("exec sp_GetUSers").AsEnumerable();
        }
        public void AssginRolesToUser(int ID,int[]roles)
        {
            UserRoles userRole;
            if (roles!=null && roles.Length>0)
            {
                foreach(int role in roles)
                {
                    userRole = new UserRoles();
                    userRole.RoleID = role;
                    userRole.UserID = ID;
                    DbContext.UserRoles.Add(userRole);
                }                
            }            
        }

        public bool ValidateUser(string username,string password)
        {
            var data=DbContext.Users.Where(x => x.Username == username && x.Password == password).ToList();
            if(data.Count>0)
            {
                return true;
            }
            return false;
        }

        public bool IsUsernameExists(string username)
        {
            var data=DbContext.Users.Where(x => x.Username == username).ToList();
            if(data.Count>0)
            {
                return true;
            }
            return false;
        }

        public override void Add(Users entity)
        {
            DbContext.Users.Add(entity);
            DbContext.SaveChanges();
            int[] roles = new int[] { 2 };
            AssginRolesToUser(entity.ID,roles);
            DbContext.SaveChanges();
        }
    }

    public interface IUsersRepository:IRepository<Users>
    {
        IEnumerable<UsersViewModel> GetAllForIndex();
        string GetUserName(int ID);
        void AssginRolesToUser(int ID, int[] roles);
        bool ValidateUser(string userName, string password);
        bool IsUsernameExists(string username);
                  
    }
}
