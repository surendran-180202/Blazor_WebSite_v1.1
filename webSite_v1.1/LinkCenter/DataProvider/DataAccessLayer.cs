using LinkCenter.DataModal;
using System.Data.SqlClient;
using System.Text.Json;

namespace LinkCenter.DataProvider
{
    public class DataAccessLayer
    {
        public static string connectionString = "Data Source=MS-NB0102;Initial Catalog=SBPersonal;Integrated Security=True;Trusted_Connection=SSPI; Encrypt=false; TrustServerCertificate=true ";
        private int currentUserID = 1;
        private string sqlQuery = string.Empty;
        private string jsonFile = string.Empty;
        private SqlConnection conn = new SqlConnection(connectionString);
        // private List<tblExperience> listExperience= new List<tblExperience>();
        // private List<tblEducationDetails> listEducation = new List<tblEducationDetails>();

        //private readonly IConfiguration? _configuration;
        //public static string connectionString = _configuration.GetConnectionString("DefaultConnection");
        private bool CheckDatabaseExists(string tblName)
        {
            bool result = false;
            try
            {
                sqlQuery = $"SELECT count(*) as Exist from INFORMATION_SCHEMA.TABLES where table_name = '{tblName}'";
                using(SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    conn.Open();
                    int databaseID = (int) cmd.ExecuteScalar();
                    conn.Close();
                    result = (databaseID > 0);
                }
            }
            catch(Exception ex)
            {
                result = false;
            }
            return result;
        }
        public List<tblExperience> GetAllExperience()
        {
            List<tblExperience> listExperience = new List<tblExperience>();
            if(!CheckDatabaseExists("tblExperience"))
            {
                sqlQuery = "CREATE TABLE tblExperience(USERID int NOT NULL, YEAR varchar(50) NOT NULL, LEARING varchar(50), INSTITUTE varchar(50), PRIMARY KEY(YEAR));";
                jsonFile = "Experience";
                CreateTable(sqlQuery, jsonFile);
            };
            SqlCommand cmd = new SqlCommand("Select * from tblExperience where USERID =@userid", conn);
            cmd.Parameters.AddWithValue("@userid", currentUserID);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                tblExperience data = new tblExperience();
                data.USERID = reader.GetInt32(0);
                data.YEAR = reader.GetString(1);
                data.LEARING = reader.GetString(2);
                data.INSTITUTE = reader.GetString(3);
                listExperience.Add(data);
            }
            conn.Close();
            return listExperience;
        }

        public List<tblEdnModal> GetAllEducation()
        {
            List<tblEdnModal> listEducation = new List<tblEdnModal>();
            if(!CheckDatabaseExists("tblEducation"))
            {
                sqlQuery = "CREATE TABLE tblEducation(USERID int NOT NULL, TITLE varchar(50) NOT NULL, YEAR varchar(50), CLASS varchar(50), INSTITUTE varchar(50),PERCENTAGE varchar(50), PRIMARY KEY(YEAR));";
                jsonFile = "Education";
                CreateTable(sqlQuery, jsonFile);
            };
            SqlCommand cmd = new SqlCommand("Select * from tblEducation where USERID =@userid", conn);
            cmd.Parameters.AddWithValue("@userid", currentUserID);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                tblEdnModal data = new tblEdnModal();
                data.USERID = reader.GetInt32(0);
                data.TITLE = reader.GetString(1);
                data.YEAR = reader.GetString(2);
                data.CLASS = reader.GetString(3);
                data.INSTITUTE = reader.GetString(4);
                data.PERCENTAGE = reader.GetString(5);
                listEducation.Add(data);
            }
            conn.Close();
            return listEducation;
        }
        private bool CreateTable(string tblQuery, string jsonFile)
        {
            try
            {
                conn.Open();
                using(SqlCommand command = new SqlCommand(tblQuery, conn))
                    command.ExecuteNonQuery();
                ImportData(jsonFile);
                conn.Close();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        private bool ImportData(string jsonFileName)
        {
            string filePath = $"wwwroot/jsonData/{jsonFileName}.json";
            string json = string.Empty;
            List<tblExperience> listExperience = new List<tblExperience>();
            List<tblEdnModal> listEducation = new List<tblEdnModal>();

            switch(jsonFileName)
            {
                case "Experience":
                    using(StreamReader reader = new StreamReader(filePath))
                    {
                        json = reader.ReadToEnd();
                        listExperience = JsonSerializer.Deserialize<List<tblExperience>>(json);
                    }
                    try
                    {
                        foreach(var item in listExperience)
                        {
                            string strAddQuery = "insert into tblExperience(USERID,YEAR,LEARING,INSTITUTE)values (@userid,@year,@learing,@institute)";
                            SqlCommand cmd = new SqlCommand(strAddQuery, conn);
                            cmd.Parameters.AddWithValue("@userid", item.USERID);
                            cmd.Parameters.AddWithValue("@year", item.YEAR);
                            cmd.Parameters.AddWithValue("@learing", item.LEARING);
                            cmd.Parameters.AddWithValue("@institute", item.INSTITUTE);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                    break;

                case "Education":
                    using(StreamReader reader = new StreamReader(filePath))
                    {
                        json = reader.ReadToEnd();
                        listEducation = JsonSerializer.Deserialize<List<tblEdnModal>>(json);
                    }
                    try
                    {
                        foreach(var item in listEducation)
                        {
                            string strAddQuery = "insert into tblEducation(USERID,TITLE,YEAR,CLASS,INSTITUTE,PERCENTAGE)values (@userid,@title,@year,@class,@institute,@percentage)";
                            SqlCommand cmd = new SqlCommand(strAddQuery, conn);
                            cmd.Parameters.AddWithValue("@userid", item.USERID);
                            cmd.Parameters.AddWithValue("@title", item.TITLE);
                            cmd.Parameters.AddWithValue("@year", item.YEAR);
                            cmd.Parameters.AddWithValue("@class", item.CLASS);
                            cmd.Parameters.AddWithValue("@institute", item.INSTITUTE);
                            cmd.Parameters.AddWithValue("@percentage", item.PERCENTAGE);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    catch(Exception ex)
                    {
                        return false;
                    }
                    break;

                default:
                    Console.WriteLine("There is no data to import ");
                    break;
            }
            return true;
        }
    }
}
