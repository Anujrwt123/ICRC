using ICRC.Data.Infrastructure;
using ICRC.Data.Repositories;
using ICRC.Model;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICRCService
{
    public class UsersService:IUsersService
    {
            public readonly IUsersRepository userRepository;
            public readonly IUnitOfWork unitOfWork;

        public UsersService(IUsersRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<UsersViewModel> GetAllForIndex()
        {
           return userRepository.GetAllForIndex();
        }

        public IEnumerable<Users> GetUsers()
        {
            return userRepository.GetAll();
        }

        public Users GetUserByID(int ID)
        {
            return userRepository.GetByID(ID);
        }
        public IEnumerable<Users> GetUsers(Expression<Func<Users,bool>> where)
        {
            return userRepository.GetMany(where);
        }
        public string GetUserName(int ID)
        {
            return userRepository.GetUserName(ID);
        }
        public void CreateUser(Users user)
        {
            
            userRepository.Add(user);
        }

        public void UpdateUser(Users user)
        {
            userRepository.Update(user);
        }

        public void AssginRolesToUser(int ID, int[] roles)
        {
            userRepository.AssginRolesToUser(ID, roles);
        }

        public bool ValiddateUser(string username, string password)
        {
            return userRepository.ValidateUser(username, password);
        }
        public bool IsUsernameExists(string username)
        {
            return userRepository.IsUsernameExists(username);
        }

        public void Delete(Users user)
        {
            userRepository.Delete(user);
        }
        public int Save()
        {
            return unitOfWork.Commit();
        }

    }


    public interface IUsersService
    {
        IEnumerable<Users> GetUsers();
        Users GetUserByID(int ID);
        IEnumerable<Users> GetUsers(Expression<Func<Users, bool>> where);
        string GetUserName(int ID);
        void CreateUser(Users user);
        void UpdateUser(Users users);
        void AssginRolesToUser(int ID, int[] roles);
        int Save();
        void Delete(Users user);
        bool ValiddateUser(string username, string password);
        bool IsUsernameExists(string username);
        IEnumerable<UsersViewModel> GetAllForIndex();
        
    }
}
