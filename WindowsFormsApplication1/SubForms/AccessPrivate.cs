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

namespace TimsHelper
{
    public partial class AccessPrivate : Form
    {
        string dbServer = string.Empty;
        string UserID = string.Empty;

        public AccessPrivate( string DBServer, string UserID )
        {
            InitializeComponent();
            this.dbServer = DBServer;
            this.UserID = UserID;
        }

        private void AccessPrivate_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.UserID))
                CallAccessPrivate(this.UserID);
        }

        private void CallAccessPrivate( string UserID )
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            SqlDataAdapter dataAdapter = new SqlDataAdapter();

            string selectCommand = @"select top 100 * from tbl_Access_Private ";
            if (!string.IsNullOrWhiteSpace(UserID))
                selectCommand += "where UserID like '" + UserID + "%'";
            selectCommand += " order by seq desc";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;

            // Create a new data adapter based on the specified query.
            dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

            // Create a command builder to generate SQL update, insert, and
            // delete commands based on selectCommand. These are used to
            // update the database.
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            // Populate a new data table and bind it to the BindingSource.
            DataTable dt = new DataTable();
            //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(this.UserID))
                    CallAccessPrivate(this.UserID);

            } catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
