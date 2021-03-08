using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SPBackupHelper : Form
    {
        // Specify a connection string. Replace the given value with a 
        // valid connection string for a Northwind SQL Server sample
        // database accessible to your system.

        string dbServer = string.Empty;

        public SPBackupHelper( string DBServer )
        {
            InitializeComponent();
            this.dbServer = DBServer;
        }

        private void SPBackupHelper_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = DateTime.Now.ToString("yyyyMMdd");
                textBox2.Text = DateTime.Now.ToString("yyyyMMdd");

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];

                    if (row.Cells["name"].Value == null) break;

                    string spName = row.Cells["name"].Value.ToString();
                    
                    Backup(spName);
                }

                MessageBox.Show("완료");

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Backup( string spName )
        {
            if (string.IsNullOrWhiteSpace(spName)) return;

            SqlConnection sqlConnection1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString);

            try
            {
                
                //SqlCommand cmd = new SqlCommand();
                //SqlDataReader reader;

                //cmd.CommandText = "sp_helptext " + spName;
                //cmd.CommandType = CommandType.Text;
                //cmd.Connection = sqlConnection1;

                sqlConnection1.Open();

                StringBuilder spText = new StringBuilder();

                //reader = cmd.ExecuteReader();
                //// Data is accessible through the DataReader object here.

                //using (SqlDataReader rdr = cmd.ExecuteReader())
                //{
                //    while (rdr.Read())
                //    {
                //        var myString = rdr.GetString(0); //The 0 stands for "the 0'th column", so the first column of the result.
                //                                         // Do somthing with this rows string, for example to put them in to a list
                //        spText.Append(myString);
                //    }
                //}

                DataSet ds = new DataSet();

                // SqlDataAdapter 초기화
                SqlDataAdapter adapter = new SqlDataAdapter("sp_helptext " + spName, sqlConnection1);

                // Fill 메서드 실행하여 결과 DataSet을 리턴받음
                adapter.Fill(ds);

                if (ds.Tables.Count > 0 )
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        spText.Append(ds.Tables[0].Rows[i][0]);
                    }
                }

                // D:\git_repo\StoredProcedures
                File.WriteAllText( textBox3.Text + spName + @".sql", spText.ToString());
            }
            catch ( Exception  ex )
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection1.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "yyyyMMdd" && textBox1.Text.Length != 8)
                {
                    MessageBox.Show("조회시작일자를 입력해 주세요.");
                    return;
                }

                if (textBox2.Text != "yyyyMMdd" && textBox2.Text.Length != 8)
                {
                    MessageBox.Show("조회종료일자를 입력해 주세요.");
                    return;
                }

                BindingSource bindingSource1 = new BindingSource();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                dataGridView1.DataSource = bindingSource1;

                string selectCommand = @"select ROW_NUMBER() OVER(ORDER BY modify_date desc) AS RowNum, name, create_date, modify_date from sys.objects 
                where type = 'P'"; // and modify_date > '20180901'";
                selectCommand += " and ((create_date >= '" + textBox1.Text + "' and create_date <= '" + textBox2.Text + 
                    @"') or (modify_date >= '" + textBox1.Text + "' and modify_date <= '" + textBox2.Text + @"'))";
                    

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
