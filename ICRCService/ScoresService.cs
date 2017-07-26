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
    public interface IScoreservice
    {
        IEnumerable<Scores> GetScores();
        Scores GetScoreByID(int ID);
        IEnumerable<Scores> GetScores(Expression<Func<Scores, bool>> where);
        void CreateScore(Scores Board);
        void UpdateScore(Scores Board);
        IEnumerable<Scores> ScoresGetByPersonID(int ID);
        void Save();
        void Delete(int ID);
    }

    public class ScoresService: IScoreservice
    {
        private readonly IScoresRepository scoresRepository;
        private readonly IUnitOfWork unitOfWork;

        public ScoresService(IScoresRepository scoresRepository, IUnitOfWork unitOfWork)
        {
            this.scoresRepository = scoresRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Methods

        public IEnumerable<Scores> GetScores()
        {
            return scoresRepository.GetAll().OrderBy(x=>x.ExamDate);
        }

        public Scores GetScoreByID(int ID)
        {
            return scoresRepository.GetByID(ID);
        }

        public void CreateScore(Scores board)
        {
            scoresRepository.Add(board);
        }

        public void UpdateScore(Scores board)
        {
            scoresRepository.Update(board);
        }

        public IEnumerable<Scores> GetScores(Expression<Func<Scores, bool>> where)
        {
            return scoresRepository.GetMany(where);
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Scores>  ScoresGetByPersonID(int ID)
        {
            return scoresRepository.ScoresGetByPersonID(ID).OrderBy(x=>x.ExamDate);
        }

        public void Delete(int ID)
        {
            var data = scoresRepository.GetByID(ID);
            scoresRepository.Delete(data);
        }

        #endregion

    }
}
