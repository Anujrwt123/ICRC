using ICRC.Data.Infrastructure;
using ICRC.Model;
using IRCRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public interface IFileMakerRepository:IRepository<FileMaker>
    {
        IEnumerable<FileMakerReciprocities> GetReciprocities();
    }
    public class FileMakerRepository:RepositoryBase<FileMaker>,IFileMakerRepository
    {
        public FileMakerRepository(IDbFactory dbFactory):base(dbFactory)
        {
            
            
        }

        public override IEnumerable<FileMaker> GetAll()
        {
            return  DbContext.FileMaker.ToList().Select(x => new FileMaker
            {
                Acronym=x.Acronym,
                Address=x.Address,
                CertID=x.CertID,
                City=x.City,
                currentBoard=x.currentBoard,
                dateofEntry=x.dateofEntry,
                FirstName=x.FirstName,
                HomePhone=x.HomePhone,
                ID=x.ID,
                Issueddate=x.Issueddate,
                LastName=x.LastName,
                MiddleName=x.MiddleName,
                OtherBoards=x.OtherBoards,
                RecertDate=x.RecertDate,
                Renewal=x.Renewal,
                State=x.State,
                WorkPhone=x.WorkPhone,
                Zip=x.Zip,
                FullName=x.FirstName+" "+x.MiddleName+" "+x.LastName                 
            }).AsQueryable(); 


        }

        public IEnumerable<FileMakerReciprocities> GetReciprocities()
        {
            return DbContext.FileMakerReciprocities.AsQueryable();
        }
    }
}
