using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = null;
            try
            {
                string logPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\log.txt";
                sw = System.IO.File.AppendText(logPath);

                string defaultSQLPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\SQL\default.sql";

                sw.WriteLine( "[" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "]" + defaultSQLPath);

                string defaultQuery = System.IO.File.ReadAllText(defaultSQLPath);

                sw.WriteLine("[" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "]" + defaultQuery);

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

                sw.WriteLine("[" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "]" + connectionString);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(defaultQuery, connectionString);

                // 초 단위로 설정. 실제로 166시간정도
                dataAdapter.SelectCommand.CommandTimeout = 600000;
                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataSet ds = new DataSet();
                //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(ds);

                sw.WriteLine("[" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "]" + "종료");
                //System.Console.ReadKey();
            } catch ( Exception ex )
            {
                System.Console.WriteLine(ex.Message);

                //System.Console.ReadKey();
            }
            finally
            {
                if ( sw != null )
                    sw.Close();
            }
        }
    }
}
