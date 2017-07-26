using ICRC.Data.Infrastructure;
using ICRC.Model;
using ICRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class CertifiedPersonRepository:RepositoryBase<CertifiedPersons>,ICertifiedPersonRepository
    {
        public CertifiedPersonRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<CPViewModelForIndex> GetAllIndex(string FirstName = "", 
            string LastName = "", 
            string MiddleName = "", 
            string Acronym = "", 
            string City = "",
            string State = "",
            string certificate = "",
            string certificateNumber = "", bool excel=false)
        {
            if (excel)
            {
                return DbContext.Database.SqlQuery<CPViewModelForIndex>("exec [sp_getCertifiedPersonsForExcel]  @firstName,@lastName,@middlename,@acronym,@city,@state,@certificate,@certificateNumber",
                   new SqlParameter("@firstname", FirstName ?? (object)DBNull.Value),
                   new SqlParameter("@lastname", LastName ?? (object)DBNull.Value),
                   new SqlParameter("@Middlename", MiddleName ?? (object)DBNull.Value),
                   new SqlParameter("@Acronym", Acronym ?? (object)DBNull.Value),
                   new SqlParameter("@City", City ?? (object)DBNull.Value),
                   new SqlParameter("@State", State ?? (object)DBNull.Value),
                   new SqlParameter("@Certificate", certificate ?? (object)DBNull.Value),
                   new SqlParameter("@CertificateNumber", certificateNumber ?? (object)DBNull.Value)
                   ).AsQueryable();
            }
        
            else
            {
                //return DbContext.CertifiedPersons.AsQueryable();
                return DbContext.Database.SqlQuery<CPViewModelForIndex>("exec sp_getCertifiedPersonsForIndex  @firstName,@lastName,@middlename,@acronym,@city,@state,@certificate,@certificateNumber",
                    new SqlParameter("@firstname", FirstName ?? (object)DBNull.Value),
                    new SqlParameter("@lastname", LastName ?? (object)DBNull.Value),
                    new SqlParameter("@Middlename", MiddleName ?? (object)DBNull.Value),
                    new SqlParameter("@Acronym", Acronym ?? (object)DBNull.Value),
                    new SqlParameter("@City", City ?? (object)DBNull.Value),
                    new SqlParameter("@State", State ?? (object)DBNull.Value),
                    new SqlParameter("@Certificate", certificate ?? (object)DBNull.Value),
                    new SqlParameter("@CertificateNumber", certificateNumber ?? (object)DBNull.Value)
                    ).AsQueryable();
            }
            //return data;
        }
        public int GetMax()
        {
            return DbContext.CertifiedPersons.Max(x => x.Id);
        }
    }

    public interface ICertifiedPersonRepository:IRepository<CertifiedPersons>
    {
        IQueryable<CPViewModelForIndex> GetAllIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string certificate = "", string certificateNumber = "", bool excel = false);
        int GetMax();
    }
}
