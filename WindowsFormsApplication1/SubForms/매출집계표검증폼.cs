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
    public partial class 매출집계표검증폼 : Form
    {
        string dbServer = string.Empty;

        public 매출집계표검증폼(string DBServer)
        {
            InitializeComponent();
            this.dbServer = DBServer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();

                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                string selectCommand = @"declare @SalesDateFrom varchar(8) = '" + textBox1.Text + @"'
                , @SalesDateTo varchar(8) = '" + textBox2.Text + @"'

                -- 임시테이블 삭제
                If exists (select * from tempdb.dbo.sysobjects with (nolock) where id = object_id(N'tempdb.[dbo].#tmp') and xtype in (N'U'))
                Begin
	                Drop table #tmp
                End


                select sd.GoodsCode, sd.PlaceCode, sd.PlaySeq
                --, sum(case when GroupID = 'OTHER' then TotAmt else 0 end) OTHERAmt
                --, sum(case when GroupID = 'PLAN' then TotAmt else 0 end) PLANAmt
                , sum(TotAmt) as SDSum
                , cast(0 as float) ADSum
                into #tmp
                from tbl_SalesTotal_Daily sd with (nolock)
	                inner join tbl_Goods_Sales gs with (nolock) on sd.GoodsCode = gs.GoodsCode and sd.PlaceCode = gs.PlaceCode and sd.PlaySeq = gs.PlaySeq
                where 1=1
                --and sd.GoodsCode = '14000326' 
                and SalesGroupID <> 'NOTIN'
                and gs.PlayDate between @SalesDateFrom and @SalesDateTo
                --and sd.KindOfSale in ('SS001', 'SS002', 'SS003')
                group by sd.GoodsCode, sd.PlaceCode, sd.PlaySeq

                select *
                from (
                select sd.GoodsCode, sd.PlaceCode, sd.PlaySeq, cast(sd.SDSum as float) SDSum
                , sum(case when ad.NormalOrCancel = 'N' then ad.TicketAmt else -ad.TicketAmt end) ADSum
                from #tmp sd
	                inner join tbl_Account_Daily ad with (nolock) on sd.GoodsCode = ad.GoodsCode and sd.PlaceCode = ad.PlaceCode and sd.PlaySeq = ad.PlaySeq
                group by sd.GoodsCode, sd.PlaceCode, sd.PlaySeq, sd.SDSum
                ) A
                where SDSum <> ADSum

                ";

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
                dataAdapter.SelectCommand.CommandTimeout = 300;
                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable dt = new DataTable();
                //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(dt);

                dataGridView1.DataSource = dt;
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView gridView = (DataGridView)sender;

                string GoodsCode = gridView.Rows[e.RowIndex].Cells["GoodsCode"].Value.ToString();
                string PlaceCode = gridView.Rows[e.RowIndex].Cells["PlaceCode"].Value.ToString();
                string PlaySeq = gridView.Rows[e.RowIndex].Cells["PlaySeq"].Value.ToString();


                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
                dataGridView2.Refresh();
                dataGridView4.DataSource = null;
                dataGridView4.Rows.Clear();
                dataGridView4.Refresh();

                SqlDataAdapter dataAdapter = new SqlDataAdapter();

                string selectCommand = @"

                select sd.BDate, sd.BDateSeq
                , cast(sum(TotAmt) as float) TotAmt
                from tbl_SalesTotal_Daily sd with (nolock)
                where 1=1
                and sd.GoodsCode = '" + GoodsCode + @"' 
                and sd.PlaceCode = '" + PlaceCode + @"' 
                and SalesGroupID <> 'NOTIN'
                and sd.PlaySeq = '" + PlaySeq + @"'
                group by sd.BDate, sd.BDateSeq
                having sum(TotAmt) <> 0
                order by BDate, BDateSeq

                ";

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
                dataAdapter.SelectCommand.CommandTimeout = 300;

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable dt = new DataTable();
                //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(dt);

                dataGridView2.DataSource = dt;
                
                
                selectCommand = @"

                select BDate, BDateSeq
                , sum(case when NormalOrCancel = 'N' then TicketAmt else -TicketAmt end) TotAmt
                from tbl_Account_Daily ad with (nolock)
                where 1=1
                and ad.GoodsCode = '" + GoodsCode + @"' 
                and ad.PlaceCode = '" + PlaceCode + @"' 
                and ad.PlaySeq = '" + PlaySeq + @"'
                group by BDate, BDateSeq
                having sum(case when NormalOrCancel = 'N' then TicketAmt else -TicketAmt end) <> 0
                order by BDate, BDateSeq

                ";

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
                dataAdapter.SelectCommand.CommandTimeout = 300;

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                dt = new DataTable();
                //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(dt);

                dataGridView4.DataSource = dt;


            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView gridView = (DataGridView)sender;

                string BDate = gridView.Rows[e.RowIndex].Cells["BDate"].Value.ToString();
                string BDateSeq = gridView.Rows[e.RowIndex].Cells["BDateSeq"].Value.ToString();

                InquiryByBDateBDateSeq(BDate, BDateSeq);

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2_CellDoubleClick(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string BDate = textBox3.Text.Split('\t')[0];
                string BDateSeq = textBox3.Text.Split('\t')[1];

                InquiryByBDateBDateSeq(BDate, BDateSeq);
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void InquiryByBDateBDateSeq( string BDate, string BDateSeq)
        {
            dataGridView3.DataSource = null;
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
            dataGridView5.DataSource = null;
            dataGridView5.Rows.Clear();
            dataGridView5.Refresh();


            SqlDataAdapter dataAdapter = new SqlDataAdapter();

            string selectCommand = @"

                select *
                from tbl_SalesTotal_Daily ad with (nolock)
                where ad.BDate = '" + BDate + @"' and ad.BDateSeq = '" + BDateSeq + @"'
                order by DataType, Seq
                ";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[this.dbServer].ConnectionString;

            // Create a new data adapter based on the specified query.
            dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
            dataAdapter.SelectCommand.CommandTimeout = 300;

            // Create a command builder to generate SQL update, insert, and
            // delete commands based on selectCommand. These are used to
            // update the database.
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            // Populate a new data table and bind it to the BindingSource.
            DataTable dt = new DataTable();
            //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(dt);

            dataGridView3.DataSource = dt;


            dataAdapter = new SqlDataAdapter();

            selectCommand = @"

                select *
                from tbl_Account_Daily ad with (nolock)
                where ad.BDate = '" + BDate + @"' and ad.BDateSeq = '" + BDateSeq + @"'
                order by AccountDate
                ";

            // Create a new data adapter based on the specified query.
            dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
            dataAdapter.SelectCommand.CommandTimeout = 300;
            // Create a command builder to generate SQL update, insert, and
            // delete commands based on selectCommand. These are used to
            // update the database.
            commandBuilder = new SqlCommandBuilder(dataAdapter);

            // Populate a new data table and bind it to the BindingSource.
            dt = new DataTable();
            //table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            dataAdapter.Fill(dt);

            dataGridView5.DataSource = dt;
        }
    }
}
