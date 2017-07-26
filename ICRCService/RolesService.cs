using ICRC.Data.Infrastructure;
using ICRC.Data.Repositories;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICRCService
{
    public interface IRoleService
    {
        IEnumerable<Roles> GetRoles();
        Roles GetRoleByID(int ID);
        IEnumerable<Roles> GetRoles(Expression<Func<Roles, bool>> where);        
        void CreateRole(Roles user);
        void UpdateRole(Roles Roles);
        IEnumerable<UserRoles> GetRolesForUser(int ID);
        void Save();
    }

    public class RolesService:IRoleService
    {

        public readonly IRolesRepository roleRepository;
        public readonly IUnitOfWork unitOfWork;


        public RolesService(IRolesRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.roleRepository= roleRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Roles> GetRoles()
        {
            return roleRepository.GetAll();
        }

        public Roles GetRoleByID(int ID)
        {
            return roleRepository.GetByID(ID);
        }
        public IEnumerable<Roles> GetRoles(Expression<Func<Roles, bool>> where)
        {
            return roleRepository.GetMany(where);
        }
      

        public void CreateRole(Roles role)
        {
            roleRepository.Add(role);
        }

        public void UpdateRole(Roles role)
        {
            roleRepository.Update(role);
        }

        public IEnumerable<UserRoles> GetRolesForUser(int ID)
        {
            return roleRepository.GetRolesForUser(ID);
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

    }
}
