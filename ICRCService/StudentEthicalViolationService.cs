using ICRC.Data.Infrastructure;
using ICRC.Data.Repositories;
using ICRC.Model;
using ICRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICRCService
{
    public interface IStudentEthicalViolationService
    {
        IEnumerable<Studentviolations> GetEthicalviolations();

        IQueryable<StudentViolationForIndex> GetViolationsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "");
        IQueryable<StudentViolationForIndex> GetEthicalviolationsByBoardID(int ID, string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "");
        Studentviolations GetEthicalviolationByID(int ID);
        IEnumerable<Studentviolations> GetEthicalviolations(Expression<Func<Studentviolations, bool>> where);
        IEnumerable<StudentViolationForIndex> GetVioltaionsByPersonID(int ID);
        void CreateEthicalviolation(Studentviolations Board);
        void UpdateEthicalviolation(Studentviolations Board);
        void Save();
        void Delete(int ID);
    }
    public class StudentEthicalviolationService: IStudentEthicalViolationService
    {
        public readonly IStudentEthicalViolationRepository StudentEthicalViolationRepository;
        public readonly IUnitOfWork unitOfWork;




        public StudentEthicalviolationService(IStudentEthicalViolationRepository StudentEthicalViolationRepository, IUnitOfWork unitOfWork)
        {
            this.StudentEthicalViolationRepository = StudentEthicalViolationRepository;
            this.unitOfWork = unitOfWork;
        }

        #region Methods

        public IQueryable<StudentViolationForIndex> GetViolationsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "")
        {
            return StudentEthicalViolationRepository.GetViolationForIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber, violation);
        }


        public IQueryable<StudentViolationForIndex> GetEthicalviolationsByBoardID(int ID, string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "", string violation = "")
        {
            return GetViolationsForIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber, violation).Where(x => x.Board == ID).AsQueryable();
        }


        public IEnumerable<Studentviolations> GetEthicalviolations()
        {
            return StudentEthicalViolationRepository.GetAll().OrderBy(x=>x.Date);
        }

        public Studentviolations GetEthicalviolationByID(int ID)
        {
            return StudentEthicalViolationRepository.GetByID(ID);
        }

        public IEnumerable<Studentviolations> GetEthicalviolations(Expression<Func<Studentviolations, bool>>where)
        {
            return StudentEthicalViolationRepository.GetMany(where);
        }

        public void CreateEthicalviolation(Studentviolations violation)
        {
            StudentEthicalViolationRepository.Add(violation);
        }

        public void UpdateEthicalviolation(Studentviolations violation)
        {
            StudentEthicalViolationRepository.Update(violation);
        }

        public IEnumerable<StudentViolationForIndex> GetVioltaionsByPersonID(int ID)
        {
            return StudentEthicalViolationRepository.GetByPersonID(ID).AsEnumerable();
        }

        public void Delete(int ID)
        {
            var data = StudentEthicalViolationRepository.GetByID(ID);
            StudentEthicalViolationRepository.Delete(data);
        }

        public void Save()           
        {
            unitOfWork.Commit();
        }


        #endregion  
    }   
}
