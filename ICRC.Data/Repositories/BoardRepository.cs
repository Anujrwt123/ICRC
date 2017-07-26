using ICRC.Data.Infrastructure;
using ICRC.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{
    public class BoardRepository:RepositoryBase<Boards>,IBoardRepository
    {
        public BoardRepository(IDbFactory dbFactory):base(dbFactory)
        {

        }

        public IQueryable<Boards> GetBoardsForIndex(string BoardName = "", string BoardAcronym = "")
        {
            return DbContext.Database.SqlQuery<Boards>("sp_GetBoardsForIndex @BoardName,@BoardAcronym",
                new SqlParameter("@BoardName",BoardName??(object)DBNull.Value),
                new SqlParameter("@BoardAcronym", BoardAcronym?? (object)DBNull.Value)).AsQueryable();
        }
    }


    public interface IBoardRepository:IRepository<Boards>
    {
        IQueryable<Boards> GetBoardsForIndex(string BoardName = "", string BoardAcronym = "");
    }
}
