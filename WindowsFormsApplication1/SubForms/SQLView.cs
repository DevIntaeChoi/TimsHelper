using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace TimsHelper.SubForms
{
    public partial class SQLView : Form
    {
        private string dbServer = string.Empty;
        private string cmd = string.Empty;
        private Hashtable param = null;
        BackgroundWorker worker = null;

        // 타이머 생성 및 시작
        System.Timers.Timer timer = new System.Timers.Timer();
            
        public SQLView(string dbServer, string cmd, Hashtable param )
        {
            InitializeComponent();

            this.dbServer = dbServer;
            this.cmd = cmd;
            this.param = param;

            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            worker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;  //Tell the user how the process went
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                string selectCommand = e.Argument.ToString();

                timer.Interval = 1000; // 1 시간
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Start();

                DataSet ds = CallQuery(selectCommand);

                //If the process exits the loop, ensure that progress is set to 100%
                //Remember in the loop we set i < 100 so in theory the process will complete at 99%
                worker.ReportProgress(100);

                e.Result = ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
            if (this.lblStatus.InvokeRequired)
            {
                this.lblStatus.BeginInvoke((MethodInvoker)delegate () { this.lblStatus.Text = e.SignalTime.ToString(); });
            }
            else
            {
                this.lblStatus.Text = e.SignalTime.ToString();
            }
            */
        }

        private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {

            }
            else if (e.Error != null)
            {

            }
            else
            {

                DisplayDataSource((DataSet)e.Result);
            }
        }

        private void SQLView_Load(object sender, EventArgs e)
        {
            try
            {
                LoadQueryList();

                cboQuery.SelectedIndex = 0;

                if ( "GoodsInfo".Equals( cmd ) && param != null )
                {
                    string selectCommand = "select 'tbl_Goods_Code' TableName,* from tbl_Goods_Code with (nolock) where GoodsCode = '" + param["GoodsCode"] + "'";
                    selectCommand += "\n select 'tbl_Goods_Master' TableName,* from tbl_Goods_Master with (nolock) where GoodsCode = '" + param["GoodsCode"] + "'";
                    selectCommand += "\n select 'tbl_Goods_Sales' TableName,* from tbl_Goods_Sales with (nolock) where GoodsCode = '" + param["GoodsCode"] + "'";
                    //selectCommand += "\n select 'tbl_Goods_SalesPrice' TableName,* from tbl_Goods_SalesPrice with (nolock) where GoodsCode = '" + param["GoodsCode"] + "'";
                    selectCommand += "\n select 'tbl_Goods_Payrate' TableName,* from tbl_Goods_Payrate with (nolock) where GoodsCode = '" + param["GoodsCode"] + "'";
                    worker.RunWorkerAsync(selectCommand);
                }
                else if ("VoucherPrefix".Equals(cmd) && param != null)
                {
                    string selectCommand = "select 'tbl_Pack_VoucherMaster' TableName,* from tbl_Pack_VoucherMaster with (nolock) where VoucherPrefix = '" + param["VoucherPrefix"] + "'";
                    selectCommand += "\n select 'tbl_Pack_VoucherGoods' TableName,* from tbl_Pack_VoucherGoods with (nolock) where VoucherPrefix = '" + param["VoucherPrefix"] + "'";
                    selectCommand += "\n select 'tbl_Pack_VoucherGoodsDetail' TableName,* from tbl_Pack_VoucherGoodsDetail with (nolock) where VoucherPrefix = '" + param["VoucherPrefix"] + "'";
                    selectCommand += "\n select 'tbl_Pack_PackageVoucher' TableName,* from tbl_Pack_PackageVoucher with (nolock) where VoucherPrefix = '" + param["VoucherPrefix"] + "'";
                    worker.RunWorkerAsync(selectCommand);
                }
                else if ("VoucherCode".Equals(cmd) && param != null)
                {
                    string selectCommand = "select 'tbl_Pack_VoucherCode' TableName,* from tbl_Pack_VoucherCode with (nolock) where VoucherPrefix = '" + param["VoucherPrefix"] + "'";
                    worker.RunWorkerAsync(selectCommand);
                }
                else if ("GoodsPayrate".Equals(cmd) && param != null)
                {
                    string selectCommand = "select 'tbl_Goods_Payrate' TableName,* from tbl_Goods_Payrate with (nolock) where GoodsCode = '" + param["GoodsCode"] + "'";
                    worker.RunWorkerAsync(selectCommand);
                }
                else if ("GoodsPayrateBiz".Equals(cmd) && param != null )
                {
                    string selectCommand = "select 'tbl_Goods_Payrate' TableName,* from tbl_Goods_Payrate with (nolock) where GoodsCode = 'B' and PlaceCode = '" + param["BizCode"] + "'";
                    worker.RunWorkerAsync(selectCommand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadQueryList()
        {
            cboQuery.Items.Clear();

            DirectoryInfo d = new DirectoryInfo(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.sql"); //Getting Text files
            foreach (FileInfo file in Files)
            {
                cboQuery.Items.Add(file.Name);
            }
        }

        private void btnEditQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL\" + cboQuery.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnInquiry_Click(object sender, EventArgs e)
        {
            try
            {
                string selectCommand = System.IO.File.ReadAllText(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\SQL\" + cboQuery.SelectedItem.ToString());
                worker.RunWorkerAsync(selectCommand);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataSet CallQuery(string selectCommand)
        {
            if (string.IsNullOrWhiteSpace(selectCommand))
            {
                MessageBox.Show("쿼리내용이 없습니다.");
                return null;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;

            // Create a new data adapter based on the specified query.
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

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

            resultDataSet = ds;

            return ds;
        }

        DataSet resultDataSet = null;

        private void DisplayDataSource(DataSet ds)
        {
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0) return;

            int top = 0;

            panel1.Controls.Clear();

            // 엑셀저장용(그리드에 표시안함)
            if ( checkBox1.Checked)
            {
                return;
            }

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataGridView gv = new DataGridView();
                gv.AutoGenerateColumns = true;
                gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                gv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                /* 데이터 양이 많을 경우 성능 향상 기능 */
                //gv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                //gv.RowHeadersVisible = false;
                /* 데이터 양이 많을 경우 성능 향상 기능 */

                //gv.ScrollBars = ScrollBars.Horizontal;
                gv.ReadOnly = true;
                
                gv.CellDoubleClick += new DataGridViewCellEventHandler(DataGridView1_CellDoubleClick);
                gv.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(gridView_DataBindingComplete);
                //gv.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgGrid_RowPostPaint);

                gv.DataSource = ds.Tables[i];

                gv.Location = new Point(0, top );

                if (ds.Tables.Count == 1)
                    gv.Size = new Size(panel1.Width, panel1.Height);
                else
                {
                    if ( ds.Tables[i].Rows.Count < 10 )
                        gv.Size = new Size(panel1.Width, 120);
                    else
                        gv.Size = new Size(panel1.Width, 300);
                }

                top += gv.Size.Height + 20;
                System.Diagnostics.Trace.WriteLine(gv.Size.Height);

                //gv.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                if ( ds.Tables.Count == 1 )
                    gv.Anchor = gv.Anchor | AnchorStyles.Right| AnchorStyles.Bottom;

                panel1.Controls.Add(gv);
            }
        }

        private void dgGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void btnAddQuery_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadQueryList();

                cboQuery.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet workSheet = null;

                    for ( int i = 0; i < resultDataSet.Tables.Count; i++ )
                    {
                        workSheet = package.Workbook.Worksheets.Add(string.Format("sheet{0}", i+1));

                        DataTable dt = resultDataSet.Tables[i];

                        //header
                        for (int colNo = 0; colNo < dt.Columns.Count; colNo++)
                        {
                            workSheet.Cells[1, colNo + 1].Value = dt.Columns[colNo].ColumnName; //헤더값
                        }

                        int excelSheetRowNo = 2;

                        //data
                        for (int rowNo = 0; rowNo < dt.Rows.Count; rowNo++)
                        {
                            for (int colNo = 0; colNo < dt.Columns.Count; colNo++)
                            {
                                workSheet.Cells[excelSheetRowNo, colNo + 1].Value = dt.Rows[rowNo][colNo].ToString();
                            }

                            excelSheetRowNo++;
                        }
                    }

                    SaveExcel(package);
                }

                MessageBox.Show("저장완료");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveExcel(ExcelPackage package)
        {
            // Displays a SaveFileDialog so the user can save the Image  
            // assigned to Button2.  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel file|*.xlsx";
            saveFileDialog1.Title = "Save an Excel File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();

                package.SaveAs(fs);

                fs.Close();
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

            if ( gv.Columns.Contains("TableName") && gv.Rows[0].Cells["TableName"] != null && gv.Rows[0].Cells["TableName"].Value != null)
                tableName = gv.Rows[0].Cells["TableName"].Value.ToString();

            if (!string.IsNullOrWhiteSpace(tableName))
                gv.Name = tableName;
        }

        private void AddEventsToGrid(DataGridView gv)
        {
            if (gv == null) return;

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
                    case "KindOfGoods":
                    case "SubCategory":
                    case "SupplyType":
                    case "SalesType":
                    case "PrintOrNot":
                    case "GoodsOption":
                    case "SettleMethod":
                    case "SettleStatus":
                    case "PaymentFlag":
                    case "SettleRange":
                    case "TmgsOrNot":
                    case "RpctTax":
                    case "ROUTINE_NAME":
                    case "name":
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

            switch (gridView.Columns[e.ColumnIndex].HeaderText)
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
                case "KindOfGoods":
                case "SubCategory":
                case "SupplyType":
                case "SalesType":
                case "PrintOrNot":
                case "GoodsOption":
                case "SettleMethod":
                case "SettleStatus":
                case "PaymentFlag":
                case "SettleRange":
                case "TmgsOrNot":
                case "RpctTax":
                    Code = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    dlg = new Coded(this.dbServer, Code, string.Empty);
                    dlg.Show();

                    break;
                case "GoodsCode":

                    Hashtable param = new Hashtable();
                    param.Add("GoodsCode", gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    dlg = new SQLView(this.dbServer, "GoodsInfo", param);
                    dlg.Show();

                    break;

                case "ROUTINE_NAME":
                case "name":

                    string spName = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    SQLEditor editor = new SQLEditor(this.dbServer, spName);
                    editor.Show();

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
