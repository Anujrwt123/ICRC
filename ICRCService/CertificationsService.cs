using IC_RC.ViewModels;
using ICRC.Data.Infrastructure;
using ICRC.Data.Repositories;
using ICRC.Model;
using ICRC.Model.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICRCService
{
    public interface ICertificationService
    {
        IQueryable<Certifications> GetCertifications();
        IQueryable<CertificationViewModelForIndex> GetCertificationsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "");
        IQueryable<CertificationViewModelForIndex> GetCertificationsByBoardID(int ID, string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "");
        Certifications GetCertificationByID(int ID);
        IQueryable<Certifications> QueueForPrint();
        ReportViewModel QueueToPrintByCertificationID(int ID);
        IEnumerable<Certifications> GetCertifications(Expression<Func<Certifications, bool>> where);
        IQueryable<CertificationViewModelForIndex> GetCertificationsByPersonID(int ID);
        IQueryable<CertificationViewModel> GetCertificationsByPersonID1(int ID);
        void CreateCertification(Certifications Board);
        void UpdateCertification(Certifications Board);
        void Save();
        void ClearQueue(string ids);
        void Mergeperson(string ids,int? id);
        void GenerateCertificate(List<int> certifications,string path);
        void UploadCSV(string filePath,Users LoggedInUser);
        bool CheckNumber(int number);
        void Delete(int ID);
        void stop();
    }

    public class CertificationsService:ICertificationService
    {
        private readonly ICertificationsRepository certificationRepository;
        private readonly  ICertificatesRepository  certificatesRepository;
        private readonly  ICertifiedPersonRepository certifiedPersonRepository;
        private readonly IUnitOfWork unitofwork;


        public CertificationsService(ICertificationsRepository certificationRepository, ICertificatesRepository certificatesRepository, ICertifiedPersonRepository certifiedPersonRepository, IUnitOfWork unitofwork)
        {
            this.certificationRepository = certificationRepository;
            this.certificatesRepository = certificatesRepository;
            this.certifiedPersonRepository = certifiedPersonRepository;
            this.unitofwork = unitofwork;
        }

        #region Methods

        public void ClearQueue(string ids)
        {
            certificationRepository.ClearQueue(ids);
        }

        public void Mergeperson(string ids, int? id)
        {
            certificationRepository.mergeperson(ids,id);
        }


        public void GenerateCertificate(List<int> certifications,string path)
        {
            string Url;
            //string folderName = Path.Combine(path, "Certifications");
            if (Directory.Exists(path))
            {
                DirectoryInfo dir=new DirectoryInfo(path);

                FileInfo[] files = dir.GetFiles();
                foreach(var file in files)
                {
                    file.Delete();
                }
                //DirectoryInfo[] di = dir.GetDirectories();
                //foreach (var directory in di)
                //{
                //    directory.Delete();
                //}

                Directory.Delete(path);
            }
            foreach (var item in certifications)
            {               
                Directory.CreateDirectory(path);
               Url= "http://icrc.infodatixhosting.com/Certifications/PrintCertificate?id="+ item;
               //Url = "http://localhost:65147/Certifications/PrintCertificate?id=" + item;
                unitofwork.PrintPDF(Url, path,item.ToString());                    
            }
        }

        public IQueryable<Certifications> QueueForPrint()
        {
            
            return GetCertifications().Where(x => x.AddToPrintQueues == true);
        }

        public ReportViewModel QueueToPrintByCertificationID(int ID)
        {
            return certificationRepository.GetReportDataByCertificationID(ID);

        }

        public IQueryable<CertificationViewModelForIndex> GetCertificationsByBoardID(int ID,string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            return GetCertificationsForIndex().Where(x => x.IssueBoard == ID);
        }

        public bool CheckNumber(int number)
        {
            return certificationRepository.CheckNumber(number);
        }

        public IQueryable<CertificationViewModelForIndex> GetCertificationsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "")
        {
            var certifications = certificationRepository.GetCertificationsForIndex(FirstName, LastName, MiddleName, Acronym, City, State, Certificate, CertificateNumber).OrderBy(x=>x.CertID);            
            return certifications.AsQueryable();
        }

        public IQueryable<Certifications> GetCertifications()
        {        
            return certificationRepository.GetAllCertifications();
        }

        public Certifications GetCertificationByID(int ID)
        {
            return certificationRepository.GetCertificationsByID(ID);
        }

        public IEnumerable<Certifications> GetCertifications(Expression<Func<Certifications, bool>>where)
        {
            return certificationRepository.GetMany(where);
        }

        public void CreateCertification(Certifications certification )
        {
            certification.CreatedAt = DateTime.Now;
            //int ID;
            //int.TryParse(System.Web.HttpContext.Current.Session["User"].ToString(), out ID);
            //certification.CreatedBy=ID;
            certificationRepository.Add(certification);
        }

        public void UpdateCertification(Certifications certification)
        {
            certificationRepository.Update(certification);
        }

        public IQueryable<CertificationViewModelForIndex> GetCertificationsByPersonID(int ID)
        {
            return certificationRepository.GetCertificationsByPersonID(ID);
        }
        public IQueryable<CertificationViewModel> GetCertificationsByPersonID1(int ID)
        {
            return certificationRepository.GetCertificationsByPerID(ID);
        }

        public void Delete(int ID)
        {
            var data = certificationRepository.GetByID(ID);
            certificationRepository.Delete(data);
        }

        public void UploadCSV(string filePath,Users LoggedInUser)
        {
           certificationRepository.UploadCSV(filePath,LoggedInUser);
        }
        public void stop()
        {
          certificationRepository.stop();
        }
        public void Save()
        {
            unitofwork.Commit();
        }



        #endregion

    }
}
