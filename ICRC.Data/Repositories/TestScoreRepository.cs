using ICRC.Data.Infrastructure;
using ICRC.Model;
using IRCRC.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data.Repositories
{


    public class TestScoreRepository : RepositoryBase<TestScore>, ITestScoreRepository
    {
        public TestScoreRepository(IDbFactory dbFactory) : base(dbFactory) { }

        public static IEnumerable<string[]> ReadCSV(string path, params string[] separators)
        {
            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(path))
            {
                parser.SetDelimiters(separators);
                while (!parser.EndOfData)
                    yield return parser.ReadFields();
            }
        }

        public void UploadCSV(string FilePath)
        {
            DataTable dt = new DataTable();

            Type type = typeof(TestScore);
            var properties = type.GetProperties();
            foreach (PropertyInfo prop in properties)
            {


                if (prop.Name != "ID" && prop.Name != "PreviousFirstName" && prop.Name != "PreviousLastName" && prop.Name != "PreviousAddress1")
                {
                    dt.Columns.Add(new DataColumn(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                }

            }

            List<string[]> str = TestScoreRepository.ReadCSV(FilePath, ",").ToList();
            string CSVData = File.ReadAllText(FilePath);
            dt.Columns.Remove("BoardsName");
            dt.Columns.Remove("Acronym");
            int count = 0;
            foreach (string[] row in str)
            {
                if (count != 0)
                {
                    if (!string.IsNullOrEmpty(row[0]))
                    {
                        dt.Rows.Add();
                        int i = 0;
                        foreach (string cell in row)
                        {
                            if (i == 0)
                            {
                                string ss = cell;
                                var d = DbContext.TestingCompanies.Where(x => x.Name == ss).FirstOrDefault();
                                dt.Rows[dt.Rows.Count - 1][0] = d.ID;
                                i++;
                                continue;
                            }
                           
                            if (i < 31)
                            {

                                string dType = dt.Columns[i].DataType.Name;

                                if (dType == "Int32")
                                {
                                    int output = 0;
                                    if (int.TryParse(cell, out output))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = output;
                                    }
                                    if (i == 24)
                                    {
                                        string sss = cell;
                                        if (sss != "" && output == 0)
                                        {
                                            var dd = DbContext.Boards.Where(x => x.Acronym == sss).ToList();
                                            if (dd.Count > 0)
                                            {
                                                dt.Rows[dt.Rows.Count - 1][24] = dd[0].ID;
                                            }

                                        }
                                        i++;
                                        continue;
                                    }

                                }

                               

                                else if (dType == "DateTime")
                                {
                                    DateTime output;
                                    if (DateTime.TryParse(cell, out output))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = output;
                                    }
                                }
                                else
                                {
                                    
                                    dt.Rows[dt.Rows.Count - 1][i] = cell;
                                }
                                i++;
                            }
                            else
                                continue;
                        }
                    }
                }
                count++;
            }

            string conn = ConfigurationManager.ConnectionStrings["IRCREntities"].ConnectionString;

            SqlBulkCopy bulkinsert = new SqlBulkCopy(conn);
            foreach (DataColumn dc in dt.Columns)
            {
                bulkinsert.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
            }

            bulkinsert.DestinationTableName = "dbo.TestScores";
            try
            {
                bulkinsert.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //File.Delete(FilePath);
        }



        public IEnumerable<TestScore> GetScoresForIndex(string lastname, string firstname, string middlename, string emailaddress, string address1, string address2, string exam, string status, string boards)
        {
            if(boards== "- Board Acronym-")
            {
                boards = "";
            }
            return DbContext.Database.SqlQuery<TestScore>("exec [spGetTestSocreForIndex] @LastName,@FirstName,@middleName,@emailAddress,@address1,@address2,@exam,@status,@boards",
                new SqlParameter("@LastName", lastname ?? (object)DBNull.Value),
                new SqlParameter("@FirstName", firstname ?? (object)DBNull.Value),
                new SqlParameter("@middleName", middlename ?? (object)DBNull.Value),
                new SqlParameter("@emailAddress", emailaddress ?? (object)DBNull.Value),
                new SqlParameter("@address1", address1 ?? (object)DBNull.Value),
                new SqlParameter("@address2", address2 ?? (object)DBNull.Value),
                new SqlParameter("@exam", exam ?? (object)DBNull.Value) ,
                new SqlParameter("@status", status ?? (object)DBNull.Value),
                new SqlParameter("@boards", boards ?? (object)DBNull.Value)).AsQueryable();
        }

        public IEnumerable<TestScoreViewModel> GetTestScoreByPerson(string name)
        {
            return DbContext.Database.SqlQuery<TestScoreViewModel>("sp_SearchLastName @value", new SqlParameter("@value", name));
        }

        public IEnumerable<TestScoreViewModel> GetDistinctTestScores()
        {
            var data = DbContext.Database.SqlQuery<TestScoreViewModel>("sp_GetTestScores").ToList();

            return data;
        }

        public IEnumerable<TestScoreViewModel> GetLastNames(int num)
        {
            var data = DbContext.Database.SqlQuery<TestScoreViewModel>("sp_GetLastNames @page", new SqlParameter("@page", num)).ToList();

            return data;
        }


        public IEnumerable<TestScoreViewModel> GetFirstNames(string name)
        {
            var data = DbContext.Database.SqlQuery<TestScoreViewModel>("sp_GetFirstNames @name", new SqlParameter("@name", name)).ToList();

            return data;
        }

        public IEnumerable<TestScoreViewModel> GetDataByFirstAndLastName(string firstname, string lastname, string address1)
        {
            var data = DbContext.Database.SqlQuery<TestScoreViewModel>("sp_GetAllByFirstandLastName @Firstname, @Lastname , @address1",
                new SqlParameter("@Firstname", firstname ?? (object)DBNull.Value),
                new SqlParameter("@Lastname", lastname ?? (object)DBNull.Value),
                new SqlParameter("@Address1", address1 ?? (object)DBNull.Value)).AsQueryable();
            return data;
        }


        public void UpdateScores(TestScore model)
        {
            DbContext.Database.ExecuteSqlCommand("sp_udpateScores @Exam,@ExamDate,@Score,@Status,@TestingCompany,@Board,@Id,@firstname,@lastname,@address1", new SqlParameter("@Exam", model.Exam),
                new SqlParameter("@ExamDate", model.ExamDate),
                new SqlParameter("@Score", model.Score)
                , new SqlParameter("@Status", model.Status),
                new SqlParameter("@TestingCompany", model.TestingCompany),
                new SqlParameter("@Board", model.Board),
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@firstname", model.FirstName ?? (object)DBNull.Value),
                new SqlParameter("@lastname", model.LastName ?? (object)DBNull.Value),
                new SqlParameter("@address1", model.Address1 ?? (object)DBNull.Value)
                );

            //DbContext.Database.SqlQuery(null,"sp_udpateScores @Eaxm,@ExamDate,@Status,@TestingCompany,@Board,@Id", new SqlParameter("@Exam", model.Exam),
            //    new SqlParameter("@ExamDate", model.ExamDate)
            //    , new SqlParameter("@Status", model.Status),
            //    new SqlParameter("@TestingCompany", model.TestingCompany),
            //    new SqlParameter("@Board", model.Board),
            //    new SqlParameter("@ID", model.ID));
        }

        public override void Update(TestScore model)
        {
            DbContext.Database.ExecuteSqlCommand("sp_udpateTestScoreInformation @LastName,@FirstName,@MiddleName,@Address1,@Address2,@EmailAddress,@City,@state,@ZipCode,@ZipPlus,@PreviousFname,@PreviousLName,@previousAddress1",
                new SqlParameter("@LastName", model.LastName == null ? "" : model.LastName),
                new SqlParameter("@FirstName", model.FirstName == null ? "" : model.FirstName),
                new SqlParameter("@MiddleName", model.MiddleName == null ? "" : model.MiddleName),
                new SqlParameter("@Address1", model.Address1 == null ? "" : model.Address1),
                new SqlParameter("@Address2", model.Address2 == null ? "" : model.Address2),
                new SqlParameter("@EmailAddress", model.EmailAddress == null ? "" : model.EmailAddress),
                new SqlParameter("@City", model.City == null ? "" : model.City),
                new SqlParameter("@State", model.State == null ? "" : model.State),
                new SqlParameter("@ZipCode", model.ZipCode == null ? "" : model.ZipCode),
                new SqlParameter("@ZipPlus", model.ZipPlus == null ? "" : model.ZipPlus),
                new SqlParameter("@PreviousFName", model.PreviousFirstName == null ? "" : model.PreviousFirstName),
                new SqlParameter("@PreviousLName", model.PreviousLastName == null ? "" : model.PreviousLastName),
                new SqlParameter("@PreviousAddress1", model.PreviousAddress1 == null ? "" : model.PreviousAddress1)

             );
        }

    }

    public interface ITestScoreRepository : IRepository<TestScore>
    {
        IEnumerable<TestScoreViewModel> GetTestScoreByPerson(string name);

        IEnumerable<TestScore> GetScoresForIndex(string lastname, string firstname, string middlename, string emailaddress, string address1, string address2, string exam, string status, string boards);
        IEnumerable<TestScoreViewModel> GetDistinctTestScores();

        IEnumerable<TestScoreViewModel> GetLastNames(int num);

        IEnumerable<TestScoreViewModel> GetFirstNames(string name);

        IEnumerable<TestScoreViewModel> GetDataByFirstAndLastName(string firstname, string lastname, string address1);


        void UploadCSV(string FilePath);
        void UpdateScores(TestScore model);
    }

}
