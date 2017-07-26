using ICRC.Data.Infrastructure;
using ICRC.Data.Repositories;
using ICRC.Model;
using IRCRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRCService
{
    public interface IFileMakerService
    {
        IEnumerable<FileMaker> GetFileMakerData();
        IEnumerable<FileMakerReciprocities> GetReciprocities();


    }
    public class FileMakerService:IFileMakerService
    {
        //test
        public readonly IFileMakerRepository FileMakerRepository;
        public readonly IUnitOfWork UnitOfWork;
        public FileMakerService(IFileMakerRepository FileMakerRepository, IUnitOfWork UnitOfWork)
        {
            this.FileMakerRepository = FileMakerRepository;
            this.UnitOfWork = UnitOfWork;
        }
       public IEnumerable<FileMaker> GetFileMakerData()
        {
            return FileMakerRepository.GetAll();
        }

        public IEnumerable<FileMakerReciprocities> GetReciprocities()
        {
            return FileMakerRepository.GetReciprocities();
        }
    }
}
