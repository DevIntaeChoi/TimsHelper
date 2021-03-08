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
    public partial class Coded : Form
    {
        string dbServer = string.Empty;
        public Coded(string DBServer, string Code, string Flag)
        {
            InitializeComponent();
            this.dbServer = DBServer;

            if ( !string.IsNullOrWhiteSpace(Code))
                this.txtCode.Text = Code;
            if (!string.IsNullOrWhiteSpace(Flag))
                this.txtFlag.Text = Flag;
        }

        private void Coded_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCode.Text) || !string.IsNullOrWhiteSpace(txtCodeName.Text) || !string.IsNullOrWhiteSpace(txtFlag.Text))
                    CallCoded();

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CallCoded()
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) && string.IsNullOrWhiteSpace(txtCodeName.Text) && string.IsNullOrWhiteSpace(txtFlag.Text)) return;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();

            string selectCommand = @"select top 100 * from tbl_Coded with (nolock) where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(this.txtCode.Text))
                selectCommand += "and Code = '" + this.txtCode.Text + "'";
            if (!string.IsNullOrWhiteSpace(this.txtCodeName.Text))
                selectCommand += "and CodeName like '%" + this.txtCodeName.Text + "%'";
            if (!string.IsNullOrWhiteSpace(this.txtFlag.Text))
                selectCommand += "and Flag = '" + this.txtFlag.Text + "'";

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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string column = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            if ( column.ToLower().Equals("flag") )
            {
                string value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                txtCode.Text = string.Empty;
                txtFlag.Text = value;
                CallCoded();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtCode.Text) || !string.IsNullOrWhiteSpace(txtCodeName.Text) || !string.IsNullOrWhiteSpace(txtFlag.Text))
                    CallCoded();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;

            if (gv == null || gv.Rows == null || gv.Rows.Count < 1) return;

            string tableName = string.Empty;

            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (gv.Columns[i] == null) continue;

                switch (gv.Columns[i].Name)
                {
                    case "flag":
                        gv.Columns["flag"].DefaultCellStyle.ForeColor = Color.Blue;
                        break;
                }
            }
        }

        private void txtFlag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                CallCoded();
            }
        }
    }
}
