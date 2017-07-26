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
    public interface ICertificateService
    {
        IEnumerable<Certificates> GetCertificates();
        Certificates GetCertificateByID(int ID);
        IEnumerable<Certificates> GetCertificate(Expression<Func<Certificates, bool>> where);
        void CreateCertificate(Certificates cetificate);
        void UpdateCertificate(Certificates cetificate);
        void Save();
        
        void Delete(int ID);
    }

    public class CertificateService:ICertificateService
    {

        private readonly ICertificatesRepository certificateRepository;
        private readonly IUnitOfWork unitOfWork;

        public CertificateService(ICertificatesRepository certificateRepository,IUnitOfWork unitOfWork)
        {
            this.certificateRepository = certificateRepository;
            this.unitOfWork = unitOfWork;
        }


        #region Methods
        


        public IEnumerable<Certificates> GetCertificates()
        {
            return certificateRepository.GetAll().OrderBy(x=>x.Name);
        }
        


        public Certificates GetCertificateByID(int ID)
        {
            return certificateRepository.GetByID(ID);
        }
        
        public IEnumerable<Certificates> GetCertificate(Expression<Func<Certificates,bool>>where)
        {
            return certificateRepository.GetMany(where);
        }

        public void CreateCertificate(Certificates certificate)
        {
            certificate.CreatedAt = DateTime.Now;
            int ID;
            int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
            certificate.CreatedBy = ID;
            certificateRepository.Add(certificate);
        }

        public void UpdateCertificate(Certificates certificate)
        {

            certificate.ModifiedAt= DateTime.Now;
            int ID;
            int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
            certificate.ModifiedBy= ID;
            certificateRepository.Update(certificate);
        }

        public void Delete(int ID)
        {
            var data = certificateRepository.GetByID(ID);
            certificateRepository.Delete(data);
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        
        
        
        #endregion

    }
}
