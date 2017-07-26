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
    public interface IPaymentTypeService
    {
        IEnumerable<PaymentType> GetPaymentTypes();
        PaymentType GetPaymentTypeByID(int ID);
        IEnumerable<PaymentType> GetPaymentTypes(Expression<Func<PaymentType, bool>> where);        
        void CreatePaymentType(PaymentType paymentType);
        void UpdatePaymentType(PaymentType paymentType);
        void Save();

    }
    public class PaymentTypeService : IPaymentTypeService
    {
        public readonly IPaymentTypeRepository paymentTypeRepository;
        public readonly IUnitOfWork unitOfWork;

        public PaymentTypeService(IPaymentTypeRepository paymentTypeRepository, IUnitOfWork unitOfWork)
        {
            this.paymentTypeRepository = paymentTypeRepository;
            this.unitOfWork = unitOfWork;
        }


        #region Methods

        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            return paymentTypeRepository.GetAll().OrderBy(x=>x.Name);
        }


        public PaymentType GetPaymentTypeByID(int ID)
        {
            return paymentTypeRepository.GetByID(ID);
        }


        public IEnumerable<PaymentType> GetPaymentTypes(Expression<Func<PaymentType, bool>> where)
        {
            return paymentTypeRepository.GetMany(where);
        }

        public void CreatePaymentType(PaymentType paymentType)
        {
            paymentTypeRepository.Add(paymentType);
        }

        public void UpdatePaymentType(PaymentType paymentType)
        {
            paymentTypeRepository.Update(paymentType);
        }
        
        public void Save()
        {
            unitOfWork.Commit();
        }
        #endregion  
    }
}
