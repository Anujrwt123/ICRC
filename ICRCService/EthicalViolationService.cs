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
    public interface IEthicalViolationService
    {
        IEnumerable<EthicalViolation> GetEthicalviolations();
        EthicalViolation GetEthicalviolationByID(int ID);
        IEnumerable<EthicalViolation> GetEthicalviolations(Expression<Func<EthicalViolation, bool>> where);       
        void CreateEthicalviolation(EthicalViolation Board);
        void UpdateEthicalviolation(EthicalViolation Board);
        void Save();
        void Delete(int ID);
    }
    public class EthicalviolationService : IEthicalViolationService
    {
        public readonly IEthicalviolationsRepository EthicalviolationRepository;
        public readonly IUnitOfWork unitOfWork;

        public EthicalviolationService(IEthicalviolationsRepository EthicalviolationRepository, IUnitOfWork unitOfWork)
        {
            this.EthicalviolationRepository = EthicalviolationRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Methods
        public IEnumerable<EthicalViolation> GetEthicalviolations()
        {
            return EthicalviolationRepository.GetAll().OrderBy(x=>x.Name);
        }

        public EthicalViolation GetEthicalviolationByID(int ID)
        {
            return EthicalviolationRepository.GetByID(ID);
        }

        public IEnumerable<EthicalViolation> GetEthicalviolations(Expression<Func<EthicalViolation, bool>> where)
        {
            return EthicalviolationRepository.GetMany(where);
        }

        public void CreateEthicalviolation(EthicalViolation violation)
        {
            EthicalviolationRepository.Add(violation);
        }

        public void UpdateEthicalviolation(EthicalViolation violation)
        {
            EthicalviolationRepository.Update(violation);
        }

        public void Delete(int ID)
        {
            var data = EthicalviolationRepository.GetByID(ID);
            EthicalviolationRepository.Delete(data);

        }

     

        public void Save()
        {
            unitOfWork.Commit();
        }


        #endregion  
    }

}
