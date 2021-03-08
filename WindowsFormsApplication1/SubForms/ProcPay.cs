using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimsHelper.SubForms;

namespace TimsHelper
{
    public partial class ProcPay : Form
    {
        string dbServer = string.Empty;

        public ProcPay(string DBServer, string BDateSeq)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(BDateSeq))
                txtBDateSeq.Text = BDateSeq;
            this.dbServer = DBServer;
        }

        private void ProcPay_Load(object sender, EventArgs e)
        {
            try
            {
                CallProcPay(txtBDateSeq.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CallProcPay(string BDateSeq)
        {
            if (string.IsNullOrEmpty(BDateSeq)) return;

            SqlDataAdapter dataAdapter = new SqlDataAdapter();

            string selectCommand = @"proc_pay '" + BDateSeq + "', '', 'Y'";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;

            // Create a new data adapter based on the specified query.
            dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

            // Create a command builder to generate SQL update, insert, and
            // delete commands based on selectCommand. These are used to
            // update the database.
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            // Populate a new data table and bind it to the BindingSource.
            DataSet ds = new DataSet();
            //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(ds);

            DisplayDataSource(ds);
        }

        private void DisplayDataSource(DataSet ds)
        {
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0) return;

            int top = 0;

            panel1.Controls.Clear();

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataGridView gv = new DataGridView();
                gv.AutoGenerateColumns = true;
                gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                gv.ReadOnly = true;
                gv.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView1_CellDoubleClick);
                gv.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(gridView_DataBindingComplete);

                gv.DataSource = ds.Tables[i];

                gv.Location = new Point(0, top);

                gv.Size = new Size(panel1.Width, gv.Height);
                top += gv.Size.Height;
                System.Diagnostics.Trace.WriteLine(gv.Size.Height);

                gv.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                panel1.Controls.Add(gv);
            }
        }

        private void btnProcPay_Click(object sender, EventArgs e)
        {
            try
            {
                CallProcPay(txtBDateSeq.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // 그리드 name 에 테이블명을 셋팅
            SetGridName((DataGridView)sender);

            // 컬럼 더블클릭시 이벤트 등록
            AddEventsToGrid((DataGridView)sender);
        }

        private void SetGridName(DataGridView gv)
        {
            if (gv == null || gv.Rows == null || gv.Rows.Count < 1) return;

            string tableName = string.Empty;

            if (gv.Columns.Contains("TableName") && gv.Rows[0].Cells["TableName"] != null && gv.Rows[0].Cells["TableName"].Value != null)
                tableName = gv.Rows[0].Cells["TableName"].Value.ToString();

            if (!string.IsNullOrWhiteSpace(tableName))
                gv.Name = tableName;
        }

        private void AddEventsToGrid(DataGridView gv)
        {
            if (gv == null || string.IsNullOrWhiteSpace(gv.Name)) return;

            /*
            switch (gv.Name)
            {
                case "tbl_Booking_Master":
                case "tbl_Booking_Payment":
                case "tbl_Booking_SeatAssign":
                    
                    break;
            }
            */
            AddEventsForCoded(gv);
        }

        private void AddEventsForCoded(DataGridView gv)
        {
            for (int i = 0; i < gv.Columns.Count; i++)
            {
                if (gv.Columns[i] == null) continue;

                switch (gv.Columns[i].Name)
                {
                    case "TypeOfCancel":
                    case "DelyStatus":
                    case "KindOfSource":
                    case "CardName":
                    case "SttlType":
                    case "DelyMethod":
                    case "Status":
                    case "ConfirmStatus":
                    case "SalesFixStatus":
                    case "WHStatus":
                    case "KindOfSettle":
                    case "GoodsCode":
                    case "SttlStatus":
                    case "VoucherPrefix":
                        gv.Columns[gv.Columns[i].Name].DefaultCellStyle.ForeColor = Color.Blue;
                        break;
                    
                }
            }
        }

        private void DataGridView1_CellDoubleClick(Object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gridView = (DataGridView)sender;

            string Code = string.Empty;
            Form dlg = null;
            Hashtable param = null;

            switch ( gridView.Columns[e.ColumnIndex].HeaderText )
            {
                case "TypeOfCancel":
                case "DelyStatus":
                case "KindOfSource":
                case "CardName":
                case "SttlType":
                case "DelyMethod":
                case "Status":
                case "ConfirmStatus":
                case "SalesFixStatus":
                case "WHStatus":
                case "KindOfSettle":
                case "SttlStatus":

                    Code = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    dlg = new Coded(this.dbServer, Code, string.Empty);
                    dlg.Show();

                    break;
                case "GoodsCode":

                    param = new Hashtable();
                    param.Add("GoodsCode", gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() );
                    dlg = new SQLView(this.dbServer, "GoodsInfo", param);
                    dlg.Show();

                    break;
                case "VoucherPrefix":

                    param = new Hashtable();
                    param.Add("VoucherPrefix", gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    dlg = new SQLView(this.dbServer, "VoucherPrefix", param);
                    dlg.Show();

                    break;
            }
        }
    }
}
