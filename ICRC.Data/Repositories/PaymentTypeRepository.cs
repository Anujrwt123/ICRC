using ICRC.Data.Infrastructure;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class PaymentTypeRepository : RepositoryBase<PaymentType>, IPaymentTypeRepository
    {

        public PaymentTypeRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        

    }

    public interface IPaymentTypeRepository : IRepository<PaymentType>
    {
        
    }
}
