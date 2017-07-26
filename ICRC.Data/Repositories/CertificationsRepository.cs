


using IC_RC.ViewModels;
using ICRC.Data.Infrastructure;
using ICRC.Model;
using ICRC.Model.ViewModel;
using IRCRC.Model.ViewModel;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Z.BulkOperations;

namespace ICRC.Data.Repositories
{

    public class CertificationsRepository : RepositoryBase<Certifications>, ICertificationsRepository
    {
 
    public CertificationsRepository(IDbFactory dbFactory) : base(dbFactory) { }
    public string perncetage;
        public static bool running = true;
        public  IQueryable<Certifications> GetAllCertifications()
        {

            var data = DbContext.Database.SqlQuery<CertificationViewModel>("exec SP_GetCertifications").AsQueryable().Select(x => new Certifications
            {
                AddToPrintQueues = x.AddToPrintQueues,
                BoardCertificateAcronym = x.BoardCertificateAcronym,
                CertID = x.CertID,
                BoardCertificateNumber = x.BoardCertificateNumber,
                BoardCertificateAcronymName = x.BoardCertificateAcronymName,
                CertificateAcronym = x.CertificateAcronym,
                certificateNo = x.certificateNo,
                CertIssueDate = x.CertIssueDate,
                CertNotes = x.CertNotes,
                CertRequestDate = x.CertRequestDate,
                CertRequestFee = x.CertRequestFee,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ID = x.ID,
                IssueBoard = x.IssueBoard,
                IssueBoardAcronym = x.IssueBoardAcronym,
                ModifiedAt = x.ModifiedAt,
                ModifiedBy = x.ModifiedBy,
                PaymentNumber = x.PaymentNumber,
                PaymentType = x.PaymentType,
                PersonID = x.PersonID,
                PersonName = x.PersonName,
                RecertDate = x.RecertDate
            }).AsQueryable();
            //var data = (from c in DbContext.Certificates
            //            join cr in DbContext.Certifications on c.ID equals cr.CertID
            //            into t
            //            from rt in t.   IfEmpty()
            //            join bc in DbContext.Boards on rt.BoardCertificateAcronym equals bc.ID
            //            into t1
            //            from rt1 in t1.DefaultIfEmpty()
            //            join ib in DbContext.Boards on rt.IssueBoard equals ib.ID
            //            into t2
            //            from rt2 in t2.DefaultIfEmpty()
            //            join cp in DbContext.CertifiedPersons on rt.PersonID equals cp.ID                      
            //            select new
            //            {
            //                AddToPrintQueues = (bool?)rt.AddToPrintQueues,
            //                BoardCertificateAcronym = (int?)rt.BoardCertificateAcronym,
            //                CertID = (int?)rt.CertID,
            //                BoardCertificateNumber = rt.BoardCertificateNumber,
            //                BoardCertificateAcronymName = rt1.Acronym,
            //                CertificateAcronym = c.Name,
            //                certificateNo = rt.certificateNo,
            //                CertIssueDate = rt.CertIssueDate,
            //                CertNotes = rt.CertNotes,
            //                CertRequestDate = rt.CertRequestDate,
            //                CertRequestFee = rt.CertRequestFee,
            //                CreatedAt = rt.CreatedAt,
            //                CreatedBy = (int?)rt.CreatedBy,
            //                ID = rt.ID,
            //                IssueBoard = (int?)rt.IssueBoard,
            //                IssueBoardAcronym = rt2.Acronym,
            //                ModifiedAt = rt.ModifiedAt,
            //                ModifiedBy = rt.ModifiedBy,
            //                PaymentNumber = rt.PaymentNumber,
            //                PaymentType = rt.PaymentType,
            //                PersonID = (int?)rt.PersonID,
            //                PersonName = cp.FirstName + " " + cp.MiddleName + " " + cp.LastName,
            //                RecertDate = rt.RecertDate
            //            }).ToList().Select(x => new Certifications
            //            {
            //                AddToPrintQueues = x.AddToPrintQueues,
            //                BoardCertificateAcronym = x.BoardCertificateAcronym,
            //                CertID = x.CertID,
            //                BoardCertificateNumber = x.BoardCertificateNumber,
            //                BoardCertificateAcronymName = x.BoardCertificateAcronymName,
            //                CertificateAcronym = x.CertificateAcronym,
            //                certificateNo = x.certificateNo,
            //                CertIssueDate = x.CertIssueDate,
            //                CertNotes = x.CertNotes,
            //                CertRequestDate = x.CertRequestDate,
            //                CertRequestFee = x.CertRequestFee,
            //                CreatedAt = x.CreatedAt,
            //                CreatedBy = x.CreatedBy,
            //                ID = x.ID,
            //                IssueBoard = x.IssueBoard ?? x.IssueBoard.Value,
            //                IssueBoardAcronym = x.IssueBoardAcronym,
            //                ModifiedAt = x.ModifiedAt,
            //                ModifiedBy = x.ModifiedBy,
            //                PaymentNumber = x.PaymentNumber,
            //                PaymentType = x.PaymentType,
            //                PersonID = x.PersonID,
            //                PersonName = x.PersonName,
            //                RecertDate = x.RecertDate
            //            }).ToList();

            return data;
        }

        public IQueryable<CertificationViewModelForIndex> GetCertificationsByPersonID(int ID)
        {

            return GetCertificationsForIndex().Where(x => x.PersonID == ID).AsQueryable();
        }

        public bool CheckNumber(int Number)
        {
            var data = DbContext.Certifications.AsQueryable().Where(x => x.certificateNo == Number).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void stop()
        {
            ProgressHub.count = 0;
            ProgressHub.loop = false;
            running = false;
        }
        public Certifications GetCertificationsByID(int ID)
        {
            return DbContext.Database.SqlQuery<CertificationViewModel>("exec SP_GetCertificationsByID @ID", new SqlParameter("@ID", ID)).AsQueryable()
                .Select(x => new Certifications
                {
                    NoteCreatedAt = x.NoteCreatedAt,
                    NoteCreatedBy=x.NoteCreatedBy,
                    AddToPrintQueues = x.AddToPrintQueues,
                    BoardCertificateAcronym = x.BoardCertificateAcronym,
                    BoardCertificateAcronymName = x.BoardCertificateAcronymName,
                    BoardCertificateNumber = x.BoardCertificateNumber,
                    CertID = x.CertID,
                    CertificateAcronym = x.CertificateAcronym,
                    certificateNo = x.certificateNo,
                    CertIssueDate = x.CertIssueDate,
                    CertNotes = x.CertNotes,
                    CertRequestDate = x.CertRequestDate,
                    CertRequestFee = x.CertRequestFee,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    ID = x.ID,
                    IssueBoard = x.IssueBoard,
                    IssueBoardAcronym = x.IssueBoardAcronym,
                    ModifiedAt = x.ModifiedAt,
                    ModifiedBy = x.ModifiedBy,
                    PaymentNumber = x.PaymentNumber,
                    PaymentType = x.PaymentType,
                    PersonID = x.PersonID,
                    PersonName = x.PersonName,
                    RecertDate = x.RecertDate
                }).FirstOrDefault();
        }

        public IQueryable<CertificationViewModel> GetCertificationsByPerID(int ID)
        {
            return DbContext.Database.SqlQuery<CertificationViewModel>("exec SP_GetCertificationsByPersonID @ID", new SqlParameter("@ID", ID)).AsQueryable()
                .Select(x => new CertificationViewModel
                {

                    BoardCertificateAcronym = x.BoardCertificateAcronym,
                    BoardCertificateAcronymName = x.BoardCertificateAcronymName,
                    BoardCertificateNumber = x.BoardCertificateNumber,
                    CertID = x.CertID,
                    CertIssueDate = x.CertIssueDate,
                    RecertDate=x.RecertDate,
                    ID = x.ID,
               });
        }

        public IQueryable<CertificationViewModelForIndex> GetCertificationsForIndex(string FirstName = "",
            string LastName = "",
            string MiddleName = "",
            string Acronym = "",
            string City = "",
            string State = "",
            string certificate = "",
            string certificateNumber = "")
        {
            return DbContext.Database.SqlQuery<CertificationViewModelForIndex>("sp_GetCertificationsForIndex @firstName,@lastName,@middlename,@acronym,@city,@state,@certificate,@certificateNumber",
                new SqlParameter("@firstname", FirstName ?? (object)DBNull.Value),
                new SqlParameter("@lastname", LastName ?? (object)DBNull.Value),
                new SqlParameter("@Middlename", MiddleName ?? (object)DBNull.Value),
                new SqlParameter("@Acronym", Acronym ?? (object)DBNull.Value),
                new SqlParameter("@City", City ?? (object)DBNull.Value),
                new SqlParameter("@State", State ?? (object)DBNull.Value),
                new SqlParameter("@Certificate", certificate ?? (object)DBNull.Value),
                new SqlParameter("@CertificateNumber", certificateNumber ?? (object)DBNull.Value)
                ).AsQueryable();
        }
        public ReportViewModel GetReportDataByCertificationID(int ID)
        {
            return DbContext.Database.SqlQuery<ReportViewModel>("exec sp_ReportCertificationsByCertificationID @ID", new SqlParameter("@ID", ID)).FirstOrDefault();
        }

        public int GenerateNumber()
        {
            Random r = new Random();
            int number = r.Next(100000, 999999);
            if (CheckNumber(number))
            {
                //number = r.Next(100000, 999999);
                GenerateNumber();
            }
            return number;
        }


        public static IEnumerable<string[]> ReadCSV(string path, params string[] separators)
        {
            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(path))
            {
                parser.SetDelimiters(separators);
                while (!parser.EndOfData)
                    yield return parser.ReadFields();
            }
        }
        
        public static string[] SplitCSV(string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }

                list.Add(curr.TrimStart(','));
            }

            return list.ToArray<string>();
        }
        public bool UploadAsync(dynamic files,int count1,int threadnumber)
        {
            var entity = new ICRC.Data.ICRCEntities();
            int percentage = 0;
           
          
            DataTable dt = MakeTable<Certifications>();
            DataTable dtupdate = MakeTable<Certifications>();
            ///Boards board;
            ///

            dt.Columns.Remove("ID");
            CertifiedPersons person;
            //DateTime CertificateIssueDate, RenewalDate, CertificaterequestDate;
            string firstName = string.Empty, middlename, lastname= string.Empty, address= string.Empty, email= string.Empty, city= string.Empty, state= string.Empty, Zip, country, phone,boardCertificateAcronym=string.Empty;
            //boardCertificateNumber, certificateName, issueBoardName,
            int PersonID = 0, otherboard = 0, cerficationNo = 0, certId=0;
            int? currentBoard;
            //paymentType = 0, issueBoard = 0, certificateNumber = 0,certifcateID = 0,
            DateTime startDate=DateTime.Now, recertDate=DateTime.Now;
            string[] cells;
            double count = 0;
            var userdata = entity.CertifiedPersons.ToList();
            var certificationsList = entity.Certifications.ToList();
            //Dictionary<string, CertifiedPersons> persons1 = new Dictionary<string, CertifiedPersons>();
            //Dictionary<string, CertifiedPersons> persons2 = new Dictionary<string, CertifiedPersons>();
            //foreach (CertifiedPersons p in userdata)
            //{
            //    persons1.Add(p.Key1, p);
            //    persons2.Add(p.Key2, p);
            //}

            //List<CertifiedPersons> newPersons = new List<CertifiedPersons>();
            //List<Certifications> newCerts = new List<Certifications>();



            var boardlist = entity.Boards.ToList();
            
            foreach (string row in files)
            {
               
                //ProgressHub.count = Convert.ToInt32((count / count1) * 100);
                if (running == true)
                {
                    bool newrow1 = true;
                    bool newrow2 = true;
                    bool newrow3 = true;
                    bool newrow4 = true;
                    bool newrow5 = true;
                    bool newrow6 = true;
                    bool newrow7 = true;
                    bool newrow8 = true;
                    int i = 0;

                    if (!string.IsNullOrEmpty(row))
                    {
                        start:
                        //cells = row.Split(',');
                        cells = SplitCSV(row);
                       
                        if (cells[0] != "" && cells[0] != "FirstName" && cells[0].Length!=1)
                        {

                            string dtype = dt.Columns[i].DataType.Name;
                            //board = boardlist.FirstOrDefault(x => x.ID == loggedInUser.BoardID);

                            firstName = cells[0];

                            lastname = cells[1];
                             middlename = cells[2];
                                   email = cells[9];                           
                             address =cells[4];
                            city = cells[5];
                            state = cells[6];                           
                            Zip = cells[7];
                            country = cells[8];
                            phone = cells[10];
                           
                             var boards = boardlist.Where(a => a.Acronym == Convert.ToString(cells[12])).FirstOrDefault();
                            if (boards != null)
                            {
                                currentBoard = boards.ID;
                            }
                            else                         
                            {
                                currentBoard = 0;
                            }
                          
                            //boardCertificateAcronym = cells[12];
                            var result = userdata.Where(x => x.CurrentBoardID == currentBoard && x.Email.Replace("\"", "").Trim() == email.Replace("\"", "").Trim() && x.LastName.Replace("\"","").Trim() == lastname.Replace("\"", "").Trim() && x.FirstName.Replace("\"", "").Trim() == firstName.Replace("\"", "").Trim() && x.City.Replace("\"", "").Trim() == city.Replace("\"", "").Trim() && x.State.Replace("\"", "").Trim() == state.Replace("\"", "").Trim()).FirstOrDefault();
                            if (result != null)
                            {
                                PersonID = result.Id;
                            }
                            else
                            {
                                PersonID = 0;
                            }
                            if (PersonID == 0)
                            {
                                person = new CertifiedPersons();
                                person.Suffix = "";
                                person.FirstName = firstName;
                                person.MiddleName = middlename;
                                person.LastName = lastname;
                                person.Address = address;
                                person.City = city;
                                person.State = state;
                                //  person.province = provience;
                                person.Country = country;
                                person.Zip = Zip;
                                person.Email = email;
                                person.BoardAcronym= cells[12];
                                person.CurrentBoardID = Convert.ToInt32(currentBoard);
                                person.OtherBoardID = otherboard;
                                person.Active = true;
                                person.CreatedAt = DateTime.Now;
                                person.CreatedBy = 1;
                                try
                                {
                                    entity.CertifiedPersons.Add(person);
                                    entity.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                  ProgressHub.error = true;
                                    ProgressHub.errormsg = "Something went wrong";
                                    throw;
                                }

                                //DataTable tblPerson = MakeTable<CertifiedPersons>(
                                PersonID = person.Id;
                                userdata.Add(person);
                            }
                          
                                if (cells[13] != "")
                                {

                                    Int32 certid;
                                    Int32.TryParse(cells[13], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow1 == true )
                                    {
                                        boardCertificateAcronym = "ICAADC";
                                        certId = certid;

                                        Int32 certnum;
                                        if (Int32.TryParse(cells[14], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[15], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[16], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow1 = false;
                                   
                                        goto resume;
                                    
                                    }
                                }
                                if (cells[17] != "")
                                {
                                    Int32 certid;
                                    Int32.TryParse(cells[17], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow2 == true )
                                    {

                                        boardCertificateAcronym = "ICADC";
                                        certId = certid;

                                        Int32 certnum;
                                        if (Int32.TryParse(cells[18], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[19], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[20], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow2 = false;
                                  
                                        goto resume;
                                    

                                }
                                }
                                if (cells[21] != "")
                                {
                                    Int32 certid;
                                    Int32.TryParse(cells[21], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow3 == true )
                                    {

                                        boardCertificateAcronym = "ICCDP";
                                        certId = certid;

                                        Int32 certnum;
                                        if (Int32.TryParse(cells[22], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[23], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[24], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow3 = false;
                                   
                                        goto resume;
                                    


                                }
                                }
                                if (cells[25] != "")
                                {
                                    Int32 certid;
                                    Int32.TryParse(cells[25], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow4 == true )
                                    {

                                        boardCertificateAcronym = "ICCDPD";
                                        certId = certid;

                                        Int32 certnum;
                                        if (Int32.TryParse(cells[26], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[27], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[28], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow4 = false;

                                  
                                        goto resume;
                                    

                                }
                                }
                                if (cells[29] != "")
                                {
                                    Int32 certid;
                                    Int32.TryParse(cells[29], out certid);
                                    DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                    DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow5 == true )
                                    {

                                        boardCertificateAcronym = "ICCJP";
                                        certId = certid;
                                  
                                    Int32 certnum;
                                        if (Int32.TryParse(cells[30], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[31], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[32], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow5 = false;
                                   
                                        goto resume;
                                    
                                }
                                }

                                if (cells[33] != "")
                                {
                                    Int32 certid;
                                    Int32.TryParse(cells[33], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow6 == true )
                                    {

                                        boardCertificateAcronym = "ICCS";
                                        certId = certid;

                                        Int32 certnum;
                                        if (Int32.TryParse(cells[34], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[35], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[36], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow6 = false;
                                    if (cells[37] != "" || cells[41] != "")
                                    {
                                        goto resume;
                                    }
                                    else
                                    {
                                        newrow8 = false;
                                    }
                                        
                                    
                                }
                                }
                                if (cells[37] != "")
                                {
                                    Int32 certid;
                                    Int32.TryParse(cells[37], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if ( newrow7 == true )
                                    {

                                        boardCertificateAcronym = "ICPR";
                                        certId = certid;

                                        Int32 certnum;
                                        if (Int32.TryParse(cells[38], out certnum))
                                        {
                                            cerficationNo = certnum;
                                        }
                                        DateTime output;
                                        if (DateTime.TryParse(cells[39], out output))
                                        {
                                            startDate = output;
                                        }
                                        DateTime output1;
                                        if (DateTime.TryParse(cells[40], out output1))
                                        {
                                            recertDate = output1;
                                        }
                                        newrow7 = false;
                                    if (cells[41] != "")
                                    {
                                        goto resume;
                                    }
                                    else
                                    {
                                        newrow8 = false;
                                    }
                                    
                                }
                                }
                            if (cells[41] != "")
                            {

                                Int32 certid;
                                Int32.TryParse(cells[41], out certid);
                                DataRow dat = dt.Select("CertID = " + certid + "").FirstOrDefault();
                                DataRow datupdate = dtupdate.Select("CertID = " + certid + "").FirstOrDefault();
                                if (newrow8 == true)
                                {
                                    boardCertificateAcronym = "ICPS";
                                    certId = certid;

                                    Int32 certnum;
                                    if (Int32.TryParse(cells[42], out certnum))
                                    {
                                        cerficationNo = certnum;
                                    }
                                    DateTime output;
                                    if (DateTime.TryParse(cells[43], out output))
                                    {
                                        startDate = output;
                                    }
                                    DateTime output1;
                                    if (DateTime.TryParse(cells[44], out output1))
                                    {
                                        recertDate = output1;
                                    }
                                    
                                }
                            }

                                resume:

                                if (certId != null || certId != 0 || cerficationNo != null || cerficationNo != 0)
                                {
                                 
                                       var filtercert = certificationsList.Where(a => a.certificateNo == cerficationNo && a.PersonID == PersonID).FirstOrDefault();
                                   
                                    if (filtercert == null)
                                    {

                                   
                                    DataRow dat = dt.Select("CertID = " + certId + " and PersonID = " + PersonID + "").FirstOrDefault();

                                    if (dat == null)
                                    {
                                        dt.Rows.Add();
                                        dt.Rows[dt.Rows.Count - 1]["IssueBoard"] = currentBoard;
                                        dt.Rows[dt.Rows.Count - 1]["AddToPrintQueues"] = 0;

                                        dt.Rows[dt.Rows.Count - 1]["PaymentNumber"] = "0";
                                        dt.Rows[dt.Rows.Count - 1]["PaymentType"] = 0;
                                        dt.Rows[dt.Rows.Count - 1]["CertNotes"] = "NA";
                                        dt.Rows[dt.Rows.Count - 1]["CreatedBy"] = -1;
                                        dt.Rows[dt.Rows.Count - 1]["CreatedAt"] = DateTime.Now;
                                        dt.Rows[dt.Rows.Count - 1]["ModifiedBy"] = -1;
                                        dt.Rows[dt.Rows.Count - 1]["ModifiedAt"] = DateTime.Now;

                                        dt.Rows[dt.Rows.Count - 1]["NoteCreatedBy"] = "NA";
                                        dt.Rows[dt.Rows.Count - 1]["NoteCreatedAt"] = DateTime.Now;
                                        //dt.Rows[dt.Rows.Count - 1]["BoardCertificateAcronym"] = "";

                                        dt.Rows[dt.Rows.Count - 1]["PersonID"] = Convert.ToInt32(PersonID);
                                        dt.Rows[dt.Rows.Count - 1]["CertID"] = certId;
                                        dt.Rows[dt.Rows.Count - 1]["certificateNo"] = cerficationNo;
                                        dt.Rows[dt.Rows.Count - 1]["CertIssueDate"] = startDate;
                                        dt.Rows[dt.Rows.Count - 1]["RecertDate"] = recertDate;
                                        dt.Rows[dt.Rows.Count - 1]["BoardCertificateAcronym"] = boardCertificateAcronym;
                                    }else
                                    {
                                        newrow8 = false;
                                    }
                                    }
                                    else
                                    {
                                    DataRow dtupdate1 = dtupdate.Select("CertID = " + certId + " and PersonID = " + PersonID + "").FirstOrDefault();
                                    
                                   
                                    if (dtupdate1 == null)
                                    {
                                        dtupdate.Rows.Add();
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["ID"] = filtercert.ID;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["IssueBoard"] = currentBoard;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["AddToPrintQueues"] = 0;

                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["PaymentNumber"] = "0";
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["PaymentType"] = 0;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["CertNotes"] = "NA";
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["CreatedBy"] = -1;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["CreatedAt"] = DateTime.Now;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["ModifiedBy"] = -1;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["ModifiedAt"] = DateTime.Now;

                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["NoteCreatedBy"] = "NA";
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["NoteCreatedAt"] = DateTime.Now;
                                        //dt.Rows[dt.Rows.Count - 1]["BoardCertificateAcronym"] = "";

                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["PersonID"] = Convert.ToInt32(PersonID);
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["CertID"] = certId;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["certificateNo"] = cerficationNo;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["CertIssueDate"] = startDate;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["RecertDate"] = recertDate;
                                        dtupdate.Rows[dtupdate.Rows.Count - 1]["BoardCertificateAcronym"] = boardCertificateAcronym;
                                    }else
                                    {
                                        newrow8 = false;
                                    }
                                }
                                if (newrow8 == true)
                                {
                                    goto start;
                                }else
                                {

                                }
                            }

                           
                        }
                        //board = DbContext.Boards.FirstOrDefault(x => x.Acronym == "ADAD");
                        // boardName = cells[9];

                        //    string key1 = boardCertificateAcronym  + "-" + email;
                        //string key2 = lastname + "-" + firstName + "-" + city + "-" + state;

                        //if (persons1.ContainsKey(key1) || persons2.ContainsKey(key2))
                        //{
                        //    //matched
                        //}else
                        //{
                        //    //new record
                        //}


                        // SearchPerson(lastname, city, state, address, email, currentBoard);
                        //int SearchPerson(string lastname, string city, string state, string address, string email, int boardID)

                       


                    }

                    count++;

                    if (threadnumber == 10)
                    {

                        double percent = ProgressHub.count;
                        var rowcount = files;
                        percent = (count / count1) * 100;
                        if (percent > 99)
                        {
                            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
                            //hubContext.Clients.All.onInsertTask(ProgressHub.count);
                            ProgressHub.count = 99;
                        }
                        else
                        {
                            ProgressHub.count = percent;
                        }
                    }
                }
            }
            threadCount++;
            try
            {
                if (dt.Rows.Count >= 1 )
                {
                    var success = SaveData(dt);
                    if ( threadnumber==10)
                    {
                        ProgressHub.error = false;
                        ProgressHub.count = 100;
                    }
                }
                if (dtupdate.Rows.Count >= 1)
                {
                    var updatesuccess = updateData(dtupdate);
                    if (threadnumber == 10)
                    {
                        ProgressHub.error = false;
                        ProgressHub.count = 100;
                    }
                }
                
               
            }
            catch(Exception ex)
            {
                ProgressHub.error = true;
                ProgressHub.errormsg = "Something went wrong";
            }
          
            return true;
        }

        int threadCount=0;



        public void UploadCSV(string filePath, Users loggedInUser)
        {
            //string CSVData = File.ReadAllText(filePath);
            string[] CSVData = File.ReadAllLines(filePath, Encoding.GetEncoding("iso-8859-1"));

           
            //string[] csvData= CSVData.Split('\n');
            int count1 = CSVData.Length;
            int divide = count1 / 10;

            if (CSVData.Count() > 500)
            {
                var part1 = CSVData.Skip(1).Take(divide).ToList();
                var part2 = CSVData.Skip(divide).Take(divide).ToList();
                var part3 = CSVData.Skip(2 * divide).Take(divide).ToList();
                var part4 = CSVData.Skip(3 * divide).Take(divide).ToList();
                var part5 = CSVData.Skip(4 * divide).Take(divide).ToList();
                var part6 = CSVData.Skip(5 * divide).Take(divide).ToList();
                var part7 = CSVData.Skip(6 * divide).Take(divide).ToList();
                var part8 = CSVData.Skip(7 * divide).Take(divide).ToList();
                var part9 = CSVData.Skip(8 * divide).Take(divide).ToList();
                var part10 = CSVData.Skip(9 * divide).ToList();

                new Thread(() => { UploadAsync(part1, part1.Count(), 1); }).Start();
                new Thread(() => { UploadAsync(part2, part2.Count(), 2); }).Start();
                new Thread(() => { UploadAsync(part3, part3.Count(), 3); }).Start();
                new Thread(() => { UploadAsync(part4, part4.Count(), 4); }).Start();
                new Thread(() => { UploadAsync(part5, part5.Count(), 5); }).Start();
                new Thread(() => { UploadAsync(part6, part6.Count(), 6); }).Start();
                new Thread(() => { UploadAsync(part7, part7.Count(), 7); }).Start();
                new Thread(() => { UploadAsync(part8, part8.Count(), 8); }).Start();
                new Thread(() => { UploadAsync(part9, part9.Count(), 9); }).Start();
                new Thread(() => { UploadAsync(part10, part10.Count(), 10); }).Start();
            }
            else
            {
                new Thread(() => { UploadAsync(CSVData, CSVData.Length, 10); }).Start();
            }
            //SqlBulkCopy bulkinsert = new SqlBulkCopy(conn);
            //bulkinsert.DestinationTableName = "dbo.Certifications";
            //bulkinsert.WriteToServer(dt);
            File.Delete(filePath);

          //  ProgressHub.loop = false;
        }

        public bool SaveData(DataTable dt)
        {
            var rows = dt.Select().Where(x => x.IsNull("CertID")||x.IsNull("certificateNo"));
            foreach (var row in rows)
                row.Delete();
            var rows1 = dt.Select("CertID=0");
            foreach (var row in rows1)
                row.Delete();
            string conn = ConfigurationManager.ConnectionStrings["IRCREntities"].ConnectionString;

            using (SqlBulkCopy sbc = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity))
            {
                sbc.DestinationTableName = "dbo.Certifications";
                // Number of records to be processed in one go
                sbc.BatchSize = 400;
                // Add your column mappings here
               
                sbc.ColumnMappings.Add("CertID", "CertID");
                sbc.ColumnMappings.Add("CertificateNo", "certificateNo");
                sbc.ColumnMappings.Add("IssueBoard", "IssueBoard");
                sbc.ColumnMappings.Add("BoardCertificateNumber", "BoardCertificateNumber");
                sbc.ColumnMappings.Add("CertIssueDate", "CertIssueDate");
                sbc.ColumnMappings.Add("RecertDate", "RecertDate");
                sbc.ColumnMappings.Add("CertRequestDate", "CertRequestDate");
                sbc.ColumnMappings.Add("CertRequestFee", "CertRequestFee");
                sbc.ColumnMappings.Add("PaymentNumber", "PaymentNumber");
                sbc.ColumnMappings.Add("PaymentType", "PaymentType");

                sbc.ColumnMappings.Add("CertNotes", "CertNotes");
                sbc.ColumnMappings.Add("AddToPrintQueues", "AddtoPrintQueues");
                sbc.ColumnMappings.Add("PersonID", "PersonID");
                sbc.ColumnMappings.Add("CreatedBy", "CreatedBy");
                sbc.ColumnMappings.Add("CreatedAt", "CreatedAt");

                sbc.ColumnMappings.Add("ModifiedBy", "ModifiedBy");
                sbc.ColumnMappings.Add("ModifiedAt", "ModifiedAt");
                sbc.ColumnMappings.Add("BoardCertificateAcronym", "BoardCertificateAcronym");
                sbc.ColumnMappings.Add("NoteCreatedBy", "NoteCreatedBy");
                sbc.ColumnMappings.Add("NoteCreatedAt", "NoteCreatedAt");
                // Finally write to server
                try
                {
                   

                    sbc.WriteToServer(dt);
                   
                    return true;
                }
                catch (Exception ex)
                {
                    ProgressHub.error = true;
                    ProgressHub.errormsg = "Something went wrong";
                    throw ex;
                    return false;
                }
            };
        }

        public bool updateData(DataTable dt)
        {



            if (dt.Rows.Count >= 1)
            {
                var entity = new ICRC.Data.ICRCEntities();
                var rows = dt.Select().Where(x => x.IsNull("CertID") || x.IsNull("certificateNo"));
                foreach (var row in rows)
                    row.Delete();
                var rows1 = dt.Select("CertID=0");
                foreach (var row in rows1)
                    row.Delete();
                string conn = ConfigurationManager.ConnectionStrings["IRCREntities"].ConnectionString;

               
                    var connection = new SqlConnection(conn);
                    var bulk = new BulkOperation(connection);
                   

                    bulk.DestinationTableName = "dbo.Certifications";
                // Number of records to be processed in one go
            
                // Add your column mappings here
                bulk.ColumnMappings.Add("ID", "ID");
                bulk.ColumnMappings.Add("CertID", "CertID");
                bulk.ColumnMappings.Add("certificateNo", "certificateNo");
                bulk.ColumnMappings.Add("IssueBoard", "IssueBoard");
                bulk.ColumnMappings.Add("BoardCertificateNumber", "BoardCertificateNumber");
                bulk.ColumnMappings.Add("CertIssueDate", "CertIssueDate");
                bulk.ColumnMappings.Add("RecertDate", "RecertDate");
                bulk.ColumnMappings.Add("CertRequestDate", "CertRequestDate");
                bulk.ColumnMappings.Add("CertRequestFee", "CertRequestFee");
                bulk.ColumnMappings.Add("PaymentNumber", "PaymentNumber");
                bulk.ColumnMappings.Add("PaymentType", "PaymentType");

                bulk.ColumnMappings.Add("CertNotes", "CertNotes");
                bulk.ColumnMappings.Add("AddToPrintQueues", "AddtoPrintQueues");
                bulk.ColumnMappings.Add("PersonID", "PersonID");
                bulk.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulk.ColumnMappings.Add("CreatedAt", "CreatedAt");
                bulk.ColumnMappings.Add("ModifiedBy", "ModifiedBy");
                bulk.ColumnMappings.Add("ModifiedAt", "ModifiedAt");
                bulk.ColumnMappings.Add("BoardCertificateAcronym", "BoardCertificateAcronym");
                bulk.ColumnMappings.Add("NoteCreatedBy", "NoteCreatedBy");
                bulk.ColumnMappings.Add("NoteCreatedAt", "NoteCreatedAt");
                    // Finally write to server
                    try
                    {


                    //sbc.WriteToServer(dt);
                    connection.Open();
                       bulk.BulkUpdate(dt);
                    connection.Close();
                    return true;
                    }
                    catch (Exception ex)
                    {
                        ProgressHub.error = true;
                    ProgressHub.errormsg = "Something went wrong";
                    throw ex;
                        return false;
                    }
                };
               
            
            return true;
        }

        static DataTable MakeTable<T>()
        {
            int count=0;
            Type upload = typeof(T);
            DataTable dt = new DataTable();
            var properties = upload.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                System.Reflection.PropertyAttributes propAttributes = prop.Attributes;
               
                //to not add notmapped columns
                prop.CustomAttributes.ToList().ForEach(x => { if (x.AttributeType.Name.ToUpper() == "NOTMAPPEDATTRIBUTE") count++; });
                //if(propAttributes.)
                if (count<=21)
                {
                    dt.Columns.Add(new DataColumn(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                }
                count++;
            }
            return dt;
        }

        //int SearchPerson(string firstName,string middleName,string lastName,int boardID,string email,string address)
        //{

        //    var data=DbContext.CertifiedPersons.Where(x => (x.CurrentBoardID==boardID && x.FirstName == firstName && x.MiddleName == middleName && x.LastName == lastName) 
        //                    ||(x.CurrentBoardID==boardID && x.Email==email)).FirstOrDefault();

        //    if(data!=null)
        //    {
        //        return data.ID;
        //    }
        //    return -1;
        //}
        int SearchPerson(string lastname,string city,string state,string address,string email,int boardID)
        {
            var data = DbContext.CertifiedPersons.Where(x => ((x.CurrentBoardID == boardID && boardID >0) && (x.Email==email && !string.IsNullOrEmpty(email)))
                              || (x.LastName==lastname && x.City==city && x.State==state &&x.Address==address && x.OtherBoardID==boardID)).FirstOrDefault();

            if (data != null)
            {
                return data.Id;
            }
            return -1;

        }

        public void ClearQueue(string ids)
        {
            DbContext.Database.ExecuteSqlCommand("exec SP_ClearCertificatePrintingQueue @ids", new SqlParameter("@ids",ids??(object)DBNull.Value));
        }
        public void mergeperson(string ids,int? id)
        {
            var data = ids.Split(',');
            foreach (var ite in data)
            {
                DbContext.Database.ExecuteSqlCommand("exec merge_person @Ids,@id", new SqlParameter("@Ids", ite ?? (object)DBNull.Value), new SqlParameter("@id", id ?? (object)DBNull.Value));
            }
        }

    }

    public interface ICertificationsRepository:IRepository<Certifications>
    {
        IQueryable<CertificationViewModelForIndex> GetCertificationsByPersonID(int ID);
        IQueryable<CertificationViewModel> GetCertificationsByPerID(int ID);
        IQueryable<CertificationViewModelForIndex> GetCertificationsForIndex(string FirstName = "", string LastName = "", string MiddleName = "", string Acronym = "", string City = "", string State = "", string Certificate = "", string CertificateNumber = "");
        bool CheckNumber(int number);
        Certifications GetCertificationsByID(int ID);
        void ClearQueue(string ids);
        void mergeperson(string ids,int? id);
        void UploadCSV(string filePath,Users LoggedInUser);
        void stop();
         IQueryable<Certifications> GetAllCertifications();
        ReportViewModel GetReportDataByCertificationID(int ID);
        //IEnumerable<Certifications> GetAll();


    }
}


//CurrentboardName = cells[9];
//otherboardName = cells[10];

//var data = DbContext.Boards.FirstOrDefault(x => x.Acronym == CurrentboardName);
//if (data != null)
//{
//    currentBoard= data.ID;
//}

//certifcateID = Convert.ToInt32(cells[8]);
//    //var certificate = DbContext.Certificates.FirstOrDefault(x => x.Name == certificateName);
//    //if (certificate != null)
//    //{
//    //    certifcateID = certificate.ID;
//    //}


//    //   int.TryParse(cells[10], out certificateNumber);
//    //cells[11];
//    issueBoardName = cells[10];
//    var issueb = DbContext.Boards.FirstOrDefault(x => x.Acronym == issueBoardName);
//    if (issueb != null)

//    {
//        issueBoard = issueb.ID;
//    }


//boardCertificateAcronym = cells[12];
//var BCN = DbContext.Boards.FirstOrDefault(x => x.Acronym == boardcertificateName);
//if (BCN != null)
//{
//    boardCertificateAcronym = BCN.ID;
//}


//DateTime output;
//if (DateTime.TryParse(cells[12], out output)) {
//    dt.Rows[dt.Rows.Count - 1]["CertIssueDate"] = output;

//}
//DateTime output1;
//if (DateTime.TryParse(cells[13], out output1))
//{
//    dt.Rows[dt.Rows.Count - 1]["RecertDate"] = output1;
//}
//DateTime output2;
//if (DateTime.TryParse(cells[14], out output2))
//{
//    dt.Rows[dt.Rows.Count - 1]["CertRequestDate"] = output2;
//}
//Double output4;
//if (Double.TryParse(cells[15], out output4))
//{
//    dt.Rows[dt.Rows.Count - 1]["CertRequestFee"] = output4;
//}

//certificateRequestFee = cells[17];
//paymentNumber = cells[18];
//paymentTypeName = cells[19];
//var paytpe = DbContext.PaymentTypes.FirstOrDefault(x => x.Name == cells[17]   );
//if (paytpe != null)
//{
//    paymentType = paytpe.ID;
//}

//CertificateNotes = cells[20];

//int SearchPerson(string lastname, string city, string state, string address, string email, int boardID)
//dt.Rows[dt.Rows.Count - 1]["CertID"] = certifcateID;

//dt.Rows[dt.Rows.Count - 1]["certificateNo"] = Convert.ToInt32(boardCertificateNumber);
//dt.Rows[dt.Rows.Count - 1]["IssueBoard"] = issueBoard;
//dt.Rows[dt.Rows.Count - 1]["AddtoPrintQueues"] = 0;
////dt.Rows[dt.Rows.Count - 1]["BoardCertificateNumber"] = boardCertificateNumber;                       

//dt.Rows[dt.Rows.Count - 1]["PaymentNumber"] = cells[16];
//dt.Rows[dt.Rows.Count - 1]["PaymentType"] = 0;
//dt.Rows[dt.Rows.Count - 1]["CertNotes"] = cells[18];
//dt.Rows[dt.Rows.Count - 1]["CreatedBy"] = -1;
//dt.Rows[dt.Rows.Count - 1]["CreatedAt"] = DateTime.Now;
//dt.Rows[dt.Rows.Count - 1]["ModifiedBy"] = -1;
//dt.Rows[dt.Rows.Count - 1]["ModifiedAt"] = DateTime.Now;
//dt.Rows[dt.Rows.Count - 1]["BoardCertificateAcronym"] = "";
//dt.Rows[dt.Rows.Count - 1]["NoteCreatedBy"] = "NA";
//dt.Rows[dt.Rows.Count - 1]["NoteCreatedAt"] = DateTime.Now;
////dt.Rows[dt.Rows.Count - 1]["BoardCertificateAcronym"] = "";
//if (PersonID <= 0)
//{
//    person = new CertifiedPersons();
//    person.Suffix = "";
//    person.FirstName = firstName;
//    person.MiddleName = middlename;
//    person.LastName = lastname;
//    person.Address = address;
//    person.City = city;
//    person.State = state;
//    //  person.province = provience;
//    //person.Country = country;
//    person.Zip = Zip;
//    person.Email = email;

//    if (board != null)
//    {
//        person.CurrentBoardID = board.ID;
//    }
//    person.OtherBoardID = otherboard;

//    DbContext.CertifiedPersons.Add(person);
//    DbContext.SaveChanges();
//    //DataTable tblPerson = MakeTable<CertifiedPersons>(
//    PersonID = person.Id;
//}
