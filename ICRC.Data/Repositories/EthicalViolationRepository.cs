using ICRC.Data.Infrastructure;
using ICRC.Model;
using ICRC.Model.ViewModel;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class StudentEthicalViolationRepository:RepositoryBase<Studentviolations>, IStudentEthicalViolationRepository
    {

        public StudentEthicalViolationRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }
        
        public override IEnumerable<Studentviolations> GetAll()
        {
            var data = DbContext.Database.SqlQuery<IRCRC.Model.ViewModel.StudentEthicalViolationViewModel>("exec sp_StudentViolatons")
                .Select(x => new Studentviolations
                {
                    Board = x.Board ,
                    BoardName = x.BoardName,
                    Comments = x.Comments,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    Date = x.Date,
                    EthicalViolationId = x.EthicalViolationId ,
                    ID = x.ID,
                    IsLetterSent = x.IsLetterSent,
                    IsScanned = x.IsScanned,
                    ISsharable = x.ISsharable,
                    ModifiedAt = x.ModifiedAt,
                    ModifiedBy = x.ModifiedBy,
                    PersonID = x.PersonID,
                    PersonName = x.PersonName,
                    EthicalViolation= x.EthicalViolation,
                    Notes = x.Notes,
                    ViolationTypes = x.ViolationTypes
                }).AsQueryable();

            //var data=(from bc in DbContext.Boards join
            //    voi in DbContext.Studentviolations on bc.ID equals voi.Board
            //      into g1
            //          from grp1 in g1.DefaultIfEmpty()
            //          join person in DbContext.CertifiedPersons on grp1.PersonID equals person.ID
            //       into g2
            //          from grp2 in g2.DefaultIfEmpty()
            //          join ethi in DbContext.Ethicalviolations on  grp1.EthicalviolationId equals ethi.ID
            //         select new
            //         {
            //             Board=(int?)grp1.Board,
            //             BoardName=bc.Acronym,
            //             Comments= grp1.Comments,
            //             CreatedAt= grp1.CreatedAt,
            //             CreatedBy= grp1.CreatedBy,
            //             Date= grp1.Date,
            //             EthicalviolationId = (int?)grp1.EthicalviolationId,
            //             ID=grp1.ID,
            //             IsLetterSent= grp1.IsLetterSent,
            //             IsScanned= grp1.IsScanned,
            //             ISsharable= grp1.ISsharable,
            //             ModifiedAt= grp1.ModifiedAt,
            //             ModifiedBy= grp1.ModifiedBy,
            //             PersonID=(int?)grp1.PersonID,
            //             PersonName=grp2!=null?(grp2.FirstName+" "+ grp2.MiddleName+" "+ grp2.LastName):null,
            //             Ethicalviolation=ethi.Name,
            //             Notes= grp1.Notes
            //         }).ToList().Select(x=>new Studentviolations {

            //             Board = x.Board?? x.Board.Value,
            //             BoardName = x.BoardName,
            //             Comments = x.Comments,
            //             CreatedAt = x.CreatedAt,
            //             CreatedBy = x.CreatedBy,
            //             Date = x.Date,
            //             EthicalviolationId=x.EthicalviolationId?? x.EthicalviolationId.Value,                         
            //             ID = x.ID,
            //             IsLetterSent = x.IsLetterSent,
            //             IsScanned = x.IsScanned,
            //             ISsharable = x.ISsharable,
            //             ModifiedAt = x.ModifiedAt,
            //             ModifiedBy = x.ModifiedBy,
            //             PersonID = x.PersonID??x.PersonID.Value,
            //             PersonName = x.PersonName,
            //             Ethicalviolation=x.Ethicalviolation,
            //             Notes=x.Notes                         
            //         }).ToList();

            return data;
        }

        public IQueryable<StudentViolationForIndex> GetByPersonID(int ID)
        {

            return GetViolationForIndex().Where(x => x.Person == ID).AsQueryable();
            // var data = DbContext.Database.SqlQuery<IRCRC.Model.ViewModel.StudentEthicalViolationViewModel>("exec sp_StudentViolatons @PersonID",new SqlParameter("@PersonID",ID))
            //   .Select(x => new Studentviolations
            //   {
            //       Board = x.Board,
            //       BoardName = x.BoardName,
            //       Comments = x.Comments,
            //       CreatedAt = x.CreatedAt,
            //       CreatedBy = x.CreatedBy,
            //       Date = x.Date,
            //       EthicalViolationId = x.EthicalViolationId,
            //       ID = x.ID,
            //       IsLetterSent = x.IsLetterSent,
            //       IsScanned = x.IsScanned,
            //       ISsharable = x.ISsharable,
            //       ModifiedAt = x.ModifiedAt,
            //       ModifiedBy = x.ModifiedBy,
            //       PersonID = x.PersonID,
            //       PersonName = x.PersonName,
            //       EthicalViolation = x.EthicalViolation,
            //       Notes = x.Notes
            //   }).AsQueryable();

            //return data;
        }

        public override Studentviolations GetByID(int ID)
        {
            return DbContext.Database.SqlQuery<StudentEthicalViolationViewModel>("exec sp_GetStudentViolationByID @id", new SqlParameter("@id", ID)).ToList()
                 .Select(x => new Studentviolations
                 {
                     Board = x.Board,
                     BoardName = x.BoardName,
                     Comments = x.Comments,
                     CreatedAt = x.CreatedAt,
                     CreatedBy = x.CreatedBy,
                     Date = x.Date,
                     EthicalViolationId = x.EthicalViolationId,
                     ID = x.ID,
                     IsLetterSent = x.IsLetterSent,
                     IsScanned = x.IsScanned,
                     ISsharable = x.ISsharable,
                     ModifiedAt = x.ModifiedAt,
                     ModifiedBy = x.ModifiedBy,
                     PersonID = x.PersonID,
                     PersonName = x.PersonName,
                     EthicalViolation = x.EthicalViolation,
                     Notes = x.Notes,
                     ViolationTypes=x.ViolationTypes
                 }).ToList().FirstOrDefault();
        }


        public IQueryable<StudentViolationForIndex> GetViolationForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "")
        {
            return DbContext.Database.SqlQuery<StudentViolationForIndex>("exec sp_GetStudentViolationsForIndex @firstname,@lastName,@middleName,@Acronym,@City,@State,@Certificate,@CertificateNumber,@Violation",
                new SqlParameter("@firstname", FirstName ?? (object)DBNull.Value),
                new SqlParameter("@lastname", LastName ?? (object)DBNull.Value),
                new SqlParameter("@Middlename", MiddleName ?? (object)DBNull.Value),
                new SqlParameter("@Acronym", Acronym ?? (object)DBNull.Value),
                new SqlParameter("@City", City ?? (object)DBNull.Value),
                new SqlParameter("@State", State ?? (object)DBNull.Value),
                new SqlParameter("@Certificate", Certificate ?? (object)DBNull.Value),
                new SqlParameter("@CertificateNumber", CertificateNumber ?? (object)DBNull.Value),
                new SqlParameter("@violation", violation ?? (object)DBNull.Value)).AsQueryable();
        }
    }

    public interface IStudentEthicalViolationRepository :IRepository<Studentviolations>
    {
        IQueryable<StudentViolationForIndex> GetByPersonID(int ID);

        IQueryable<StudentViolationForIndex> GetViolationForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "");



    }
}
