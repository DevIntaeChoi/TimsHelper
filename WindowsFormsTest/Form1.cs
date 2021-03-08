using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                decimal a = 100*0.01m;

                MessageBox.Show(a.ToString());
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
