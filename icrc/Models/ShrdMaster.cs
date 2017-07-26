using ICRC.Data.Infrastructure;
using ICRC.Model;
using ICRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace IC_RC
{
    public class ShrdMaster
    {

        ICRC.Data.ICRCEntities db = new ICRC.Data.ICRCEntities();

        private static ShrdMaster _instance;


        public static ShrdMaster Instance
        {
            get
            {
                return _instance==null ?_instance = new ShrdMaster():_instance;
            }
        }
        public static Boolean SendEmail( string useremail, string username,string password)
        {
            try
            {
                MailMessage mail = new MailMessage("testeac7@gmail.com", useremail);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                mail.IsBodyHtml = true;
                client.Credentials = new System.Net.NetworkCredential("testeac7@gmail.com", "popup$$1234");
                mail.Subject = "IC&RC";
                mail.Body = "<h2>Hello <b>" + username.ToString() + "</b></h2></br><p> Your account has been created.</br></p><p> Email:  " + useremail + "  </br> </p><p>Password: <b>" + password + "</b>.</p></br> Please click to more Information <a href='http://icrc.infodatixhosting.com/Account/Login'>http://icrc.infodatixhosting.com/Account/Login</a>";
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public List<PaymentType> GetPaymentList()
        {
            List<PaymentType> list = new List<PaymentType>() { new PaymentType() { ID=1,Name="Credit Card"}, new PaymentType() { ID = 1, Name = "Money Order" }, new PaymentType() { ID = 1, Name = "Cheque" } };
            return list;
        }

        public string GetQueryString(string value)
        {
            if (HttpContext.Current.Request.QueryString[value] != null)
            {
                return HttpContext.Current.Request.QueryString[value];
            }
            return "";
        }

        public Users GetUserByEmail(string email)
        {
            return db.Users.Where(x => x.Email == email).SingleOrDefault();
        }
     
        public List<Status> Getstatus()
        {
            List<Status> list = new List<Status>()
            {
                new Status() { ID="Pass",Name="Pass"},
                new Status() { ID="Fail",Name="Fail"}
            };

            return list;
        }


        public int GetPageIndex()
        {
            int num = 1;
            if(HttpContext.Current.Request.QueryString["grid-page"]!=null)
            {
                int.TryParse(HttpContext.Current.Request.QueryString["grid-page"], out num);
            }

            return num;
        }
        public bool IsAdmin(string Username)
        {
            var data=db.Users.FirstOrDefault(x => x.Username == Username);

            var roles = db.UserRoles.Where(x => x.UserID == data.ID && x.RoleID==1);
            if(roles!=null && roles.Count()>0)
            {
                return true;
            }
            return false;
        }

        public Users  GetUser(string username)
        {
            return db.Users.FirstOrDefault(x => x.Username == username && x.Active==true);
        }


        public string GetReturnUrl(string defaultUrl)
        {
            //if (defaultUrl == null) throw new ArgumentNullException(nameof(defaultUrl));


            if (HttpContext.Current.Request.QueryString["ReturnUrl"] != null)
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                int index = url.IndexOf("returnUrl");
                string returnUrl = url.Substring(index, (url.Length - index));
                returnUrl = returnUrl.Replace("returnUrl=", "");
                defaultUrl = returnUrl;
                //Current.Request.QueryString[$"ReturnUrl"].ToString();
            }

           return defaultUrl;
        }

        //public List<CPViewModelForIndex> getPersons()
        //{

        //}


        public List<Suffix> GetSuffix()
        {
            List<Suffix> list = new List<Suffix>()
            {
                new Suffix() {ID="Sr.",Name="Sr." },
                new Suffix() {ID="Jr.",Name="Jr." },
                new Suffix() {ID="I",Name="I" },
                new Suffix() {ID="II",Name="II" },
                new Suffix() {ID="III",Name="III" },
                new Suffix() {ID="IV",Name="IV" },
                new Suffix() {ID="V",Name="V" },
                new Suffix() {ID="VI",Name="VI" },
                new Suffix() {ID="Other",Name="Other"}
            };

            return list;
        }

        public  List<Fee> GetFees()
        {
            List<Fee> list = new List<IC_RC.Fee>()
            {
                new Fee() { ID=25.00,Name="$25.00"},
                new Fee() { ID=50.00,Name="$50.00"},
                new Fee() { ID=75.00,Name="$75.00"},
                new Fee() { ID=10.00,Name="$100.00"},
            };

            return list;
        }

        #region export


        public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }



        public void ExportListFromTsv<T>(IEnumerable<T> data,string name)
        {
            //    HttpContext.Current.Response.ClearContent();
            //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename="+name+".xlsx");
            //    HttpContext.Current.Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            //    WriteTsv(data, HttpContext.Current.Response.Output);
            //    HttpContext.Current.Response.End();


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename="+name+".xls");
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode;
            HttpContext.Current. Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            WriteTsv(data, HttpContext.Current.Response.Output);
            HttpContext.Current.Response.End();
        }
        #endregion


    }



    public class Fee
    {
        public double ID { get; set; }
        public string Name{get;set;}
    }
    public class Suffix
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class PaymentType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }


    public class Status
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}