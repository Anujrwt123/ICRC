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
    public interface IScoreboardService
    {
        IEnumerable<ScoreBoard> GetScoreboards();
        ScoreBoard GetScoreboardByID(int ID);
        IEnumerable<ScoreBoard> GetScoreboards(Expression<Func<ScoreBoard, bool>> where);
        void CreateScoreboard(ScoreBoard Board);
        void UpdateScoreboard(ScoreBoard Board);
        void Save();
        void Delete(int ID);
    }

    public class ScoreboardService: IScoreboardService
    {
        public readonly IScoreBoardRepoistory ScoreBoardRepository;
        public readonly IUnitOfWork unitOfWork;

        public ScoreboardService(IScoreBoardRepoistory ScoreBoardRepository, IUnitOfWork unitOfWork)
        {
            this.ScoreBoardRepository = ScoreBoardRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Methods

     

        public IEnumerable<ScoreBoard> GetScoreboards()
        {
            var boards= ScoreBoardRepository.GetAll().OrderBy(x => x.Name);
            return boards;
        }

        
        public ScoreBoard GetScoreboardByID(int ID)
        {
            return ScoreBoardRepository.GetByID(ID);
        }

        public IEnumerable<ScoreBoard> GetScoreboards(Expression<Func<ScoreBoard, bool>> where)
        {
            return  ScoreBoardRepository.GetMany(where);
        }

        public void CreateScoreboard(ScoreBoard board)
        {
            //board.CreatedAt = DateTime.Now;
            //int ID;
            //int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
            //certification.CreatedBy=ID;
            ScoreBoardRepository.Add(board);
            //certificationRepository.Add(certification);
        }

        public void UpdateScoreboard(ScoreBoard boards)
        {
            ScoreBoardRepository.Update(boards);            
        }

       

        public void Delete(int ID)
        {
            var data = ScoreBoardRepository.GetByID(ID);
            ScoreBoardRepository.Delete(data);
        }

        public void Save()
        {
            unitOfWork.Commit();
        }



        #endregion

    }
}
