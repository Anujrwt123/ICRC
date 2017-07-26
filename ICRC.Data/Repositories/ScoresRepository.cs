using ICRC.Data.Infrastructure;
using ICRC.Model;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class ScoresRepository : RepositoryBase<Scores>,IScoresRepository
    {
        public ScoresRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public override IEnumerable<Scores> GetAll()
        {

            var data=DbContext.Database.SqlQuery<ScoreViewModel>("exec sp_GetScores").Select(x=>new Scores {

                         Board = x.Board,
                         BoardName = x.BoardName,
                         CompanyName = x.CompanyName,
                         CreatedAt = x.CreatedAt,
                         CreatedBy = x.CreatedBy,
                         ExamDate = x.ExamDate,
                         ExamID = x.ExamID,
                         ExamName = x.ExamName,
                         ID = x.ID,
                         ModifiedAt = x.ModifiedAt,
                         ModifiedBy = x.ModifiedBy,
                         PersonID = x.PersonID,
                         PersonName = x.PersonName,
                         ScaledScore = x.ScaledScore,
                         Status = x.Status,
                         TestingCompany = x.TestingCompany,
                     }).AsQueryable();


            return data;
            
        }


        public IEnumerable<Scores> ScoresGetByPersonID(int PersonID)
        {

            var data = GetAll().Where(x => x.PersonID == PersonID);
            return data;
        }
    }


    public interface IScoresRepository : IRepository<Scores>
    {
        IEnumerable<Scores> ScoresGetByPersonID(int ID);
            

    }
}
