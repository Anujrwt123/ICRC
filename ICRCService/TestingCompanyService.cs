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
    public interface ITestingCompanyService
    {
        IEnumerable<TestingCompany> GetTestingCompanies();
       
        TestingCompany GetTestingCompanyByID(int ID);
        IEnumerable<TestingCompany> GetTestingCompanies(Expression<Func<TestingCompany, bool>> where);        
        void CreateCompany(TestingCompany company);
        void UpdateCompany(TestingCompany company);
        void Save();
        void Delete(int ID);
    }

    public class TestingCompanyService : ITestingCompanyService
    {
        private readonly ITestingCompanyRepository testingCompanyRepository;
        private readonly ICertificatesRepository certificatesRepository;
        private readonly ICertifiedPersonRepository certifiedPersonRepository;
        private readonly IUnitOfWork unitofwork;


        public TestingCompanyService(ITestingCompanyRepository testingCompanyRepository, ICertificatesRepository certificatesRepository, ICertifiedPersonRepository certifiedPersonRepository, IUnitOfWork unitofwork)
        {
            this.testingCompanyRepository = testingCompanyRepository;
            this.certificatesRepository = certificatesRepository;
            this.certifiedPersonRepository = certifiedPersonRepository;
            this.unitofwork = unitofwork;
        }

        #region Methods

             

        public IEnumerable<TestingCompany> GetTestingCompanies()
        {
            return testingCompanyRepository.GetAll().OrderBy(x=>x.Name);
        }

        public TestingCompany GetTestingCompanyByID(int ID)
        {
            return testingCompanyRepository.GetByID(ID);
        }

        public IEnumerable<TestingCompany> GetTestingCompanies(Expression<Func<TestingCompany, bool>> where)
        {
            return testingCompanyRepository.GetMany(where);
        }

        public void CreateCompany(TestingCompany company)
        {
         //   company.CreatedAt = DateTime.Now;
            //int ID;
            //int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
            //certification.CreatedBy=ID;
            testingCompanyRepository.Add(company);
        }

        public void UpdateCompany(TestingCompany company)
        {
            testingCompanyRepository.Update(company);
        }

       public void Delete(int ID)
        {
            var data=testingCompanyRepository.GetByID(ID);
            testingCompanyRepository.Delete(data);
        }

        public void Save()
        {
            unitofwork.Commit();
        }



        #endregion

    }   
}
