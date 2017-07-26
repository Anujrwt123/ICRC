using ICRC.Data.Infrastructure;
using ICRC.Data.Repositories;
using ICRC.Model;
using ICRC.Model.ViewModel;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICRCService
{
    public interface ICertifiedPersonService 
    {

        List<CertifiedPersons> GetCertifiedPersons();
        List<CPViewModelForIndex> GetCertifiedPersonsByBoardId(int ID, string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "");
        IEnumerable<CPViewModelForIndex> GetCertifedPersonsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", bool excel = false);
        CertifiedPersons GetCertifiedPersonByID(int ID);
        IEnumerable<CertifiedPersons> GetCertifiedPersons(Expression<Func<CertifiedPersons, bool>> where);
        void CreateCertifiedPerson(CertifiedPersons Board);
        void UpdateCertifiedPerson(CertifiedPersons Board);
        void Save();
        void Delete(int ID);
        int GetMax();
        

    }

    public class CertifiedPersonService:ICertifiedPersonService
    {
        public readonly ICertifiedPersonRepository certifiedRepository;
        public readonly IUnitOfWork unitOfWork;

        public CertifiedPersonService(ICertifiedPersonRepository certifiedRepository,IUnitOfWork unitOfWork)
        {
            this.certifiedRepository = certifiedRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Methods

        public int GetMax()
        {
            return certifiedRepository.GetMax();
        }
        public List<CPViewModelForIndex> GetCertifiedPersonsByBoardId(int ID, string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            return GetCertifedPersonsForIndex(FirstName, LastName, MiddleName, Acronym, City, State).Where(x => x.CurrentBoardID == ID).ToList();
        }

        public IEnumerable<CPViewModelForIndex> GetCertifedPersonsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "",bool excel=false)
      {

                return certifiedRepository.GetAllIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate , CertificateNumber,excel).ToList();
            //.Select(x => new CPViewModelForIndex
            //{
            //    FirstName=x.FirstName,
            //    LastName=x.LastName,
            //    MiddleName=x.MiddleName,
            //    State=x.State,
            //    City=x.City,
            //    BoardAcronym=x.BoardAcronym,
            //    CurrentBoardID=x.CurrentBoardID
            //}).ToList();
        }

        public List<CertifiedPersons> GetCertifiedPersons()
        {
            return certifiedRepository.GetAll().Select(x=> new CertifiedPersons{
                Address=x.Address,
                Address2=x.Address2,
                City=x.City,
                Country=x.Country,
                CreatedAt=x.CreatedAt,
                CreatedBy=x.CreatedBy,
                CurrentBoardID=x.CurrentBoardID,
                Email=x.Email,
              //  Ethicalviolation=x.Ethicalviolation,
                FirstName=x.FirstName,
                Id=x.Id,
                LastName=x.LastName,
                MiddleName=x.MiddleName,
                ModifiedAt=x.ModifiedAt,
                ModifiedBy=x.ModifiedBy,
                Notes=x.Notes,
                OtherBoardID=x.OtherBoardID,
                //province=x.province,
                SSN=x.SSN,
                State=x.State,
                Zip=x.Zip,
                FullName=x.FirstName+" "+x.MiddleName+" "+x.LastName
            }).OrderBy(x=>x.FirstName).ThenBy(x=>x.MiddleName).ThenBy(x=>x.LastName).ToList();
        }

        public CertifiedPersons GetCertifiedPersonByID (int ID)
        {
            return certifiedRepository.GetByID(ID);
        }

        public IEnumerable<CertifiedPersons> GetCertifiedPersons(Expression<Func<CertifiedPersons,bool>>where)
        {
            return certifiedRepository.GetMany(where);
        }

        public void CreateCertifiedPerson(CertifiedPersons person)
        {

            //int max = GetMax();
            person.CreatedAt = DateTime.Now;
           // person.ID = max + 1;
           // int ID;
            //int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
            //person.CreatedBy = ID;
            certifiedRepository.Add(person);
        }

        public void UpdateCertifiedPerson(CertifiedPersons person)
        {
            person.ModifiedAt = DateTime.Now;
           // int ID;
            //int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
          //  person.ModifiedBy = ID;
            certifiedRepository.Update(person);
        }

        public void Delete(int ID)
        {
            var data = certifiedRepository.GetByID(ID);

            certifiedRepository.Delete(data);
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        #endregion


    }


}


