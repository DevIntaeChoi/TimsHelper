using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimsHelper.SubForms
{
    public partial class SQLEditor : Form
    {
        private string dbServer = string.Empty;
        string spName = string.Empty;

        public SQLEditor(string dbServer, string spName)
        {
            InitializeComponent();

            this.dbServer = dbServer;
            this.spName = spName;
        }

        private void SQLEditor_Load(object sender, EventArgs e)
        {
            if ( !string.IsNullOrWhiteSpace( this.spName ))
            {
                this.textBox1.Text = this.spName;
                button1_Click(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter = new SqlDataAdapter("sp_helptext " + this.textBox1.Text , connectionString);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    StringBuilder sb = new StringBuilder();

                    for( int i = 0; i < dt.Rows.Count; i++ )
                    {
                        sb.Append(dt.Rows[i][0].ToString());
                    }

                    richTextBox1.Text = sb.ToString();
                }
                else
                    richTextBox1.Text = "SP 내역을 읽어오지 못했습니다.";

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
