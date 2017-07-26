using ICRC.Data.Infrastructure;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
   
        public class ScoreBoardRepository : RepositoryBase<ScoreBoard>, IScoreBoardRepoistory
        {
            public ScoreBoardRepository(IDbFactory dbFactory) : base(dbFactory) { }

        }

        public interface IScoreBoardRepoistory : IRepository<ScoreBoard>
        {

        }
    
}
