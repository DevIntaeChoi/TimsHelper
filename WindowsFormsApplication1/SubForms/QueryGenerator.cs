using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class QueryGenerator : Form
    {
        public QueryGenerator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string goodsNames = richTextBox1.Text;
                List<string> goodsNameList = goodsNames.Split('\n').ToList();
                MessageBox.Show(goodsNameList.Count.ToString());

                string query = "select GoodsCode, GoodsName from tbl_Goods_Code gc with (nolock) where GoodsName in (";
                for ( int i = 0; i < goodsNameList.Count; i++ )
                {
                    if ( i == 0 )
                    {
                        query += "'" + goodsNameList[i].Trim() + "'";
                    }
                    else
                    {
                        query += "\n,'" + goodsNameList[i].Trim() + "'";
                    }
                }

                query += ")\n";

                richTextBox1.Text = query;
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string goodsNames = richTextBox1.Text;

                List<string> goodsNameList = goodsNames.Split('\n').ToList();
                MessageBox.Show(goodsNameList.Count.ToString());

                string query = "";
                for (int i = 0; i < goodsNameList.Count; i++)
                {
                    string line = goodsNameList[i];
                    string[] columns = line.Split('\t').ToArray<string>();

                    string goodsName = columns[0];
                    string startDate = (columns.Length >= 2) ? columns[columns.Length - 2] : string.Empty;
                    string endDate = (columns.Length >= 1) ? columns[columns.Length - 1]: string.Empty;

                    if (i == 0)
                    {
                        query += "select '' GoodsCode,'" + goodsName.Trim() + "' GoodsName, '" + startDate + "' StartDate, '" + endDate+ "' EndDate";
                    }
                    else
                    {
                        query += "\nunion all\nselect '' GoodsCode,'" + goodsName.Trim() + "' GoodsName, '" + startDate + "' StartDate, '" + endDate + "' EndDate";
                    }
                }

                richTextBox1.Text = query;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
