using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Infrastructure
{
    public class UnitOfWork:IUnitOfWork
    {
        public readonly IDbFactory dbFactory;

        private ICRCEntities dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public ICRCEntities DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public int Commit()
        {
            return DbContext.Commit();
        }

        public void PrintPDF(string Url,string Location,string Name="")
        {
           
            HtmlToPdf Convertor = new HtmlToPdf();
            
           
                        
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString(Url);
            }
            Convertor.Options.MarginTop = 50;
            Convertor.Options.MarginBottom = 50;            
            PdfDocument doc = Convertor.ConvertHtmlString(htmlCode);
            string path;//= Path.Combine("Certifications",);
            path=Path.Combine(Location, Name + "_Certificate.Pdf");            
            doc.Save(path);
        }
    }
}
