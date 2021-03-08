
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TimsHelper;
using TimsHelper.Common;
using TimsHelper.SubForms;
using WebSocketSharp;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string[] args = null;

        public Form1( string[] args )
        {
            InitializeComponent();
            this.args = args;
        }

        /// <summary>
        /// 로그온 타입
        /// </summary>
        public enum LogonType
        {
            LOGON32_LOGON_INTERACTIVE = 2,
            LOGON32_LOGON_NETWORK = 3,
            LOGON32_LOGON_BATCH = 4,
            LOGON32_LOGON_SERVICE = 5,
            LOGON32_LOGON_UNLOCK = 7,
            LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
            LOGON32_LOGON_NEW_CREDENTIALS = 9
        }

        /// <summary>
        /// 로그온 제공자
        /// </summary>
        public enum LogonProvider
        {
            LOGON32_PROVIDER_DEFAULT = 0,
            LOGON32_PROVIDER_WINNT35 = 1,
            LOGON32_PROVIDER_WINNT40 = 2,
            LOGON32_PROVIDER_WINNT50 = 3
        }


        [DllImport("advapi32.dll", SetLastError = true)]
        public extern static bool LogonUser(String lpszUsername, String lpszDomain,
        String lpszPassword, int dwLogonType,
        int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        /*
        /// <summary>
        /// 로그온
        /// </summary>
        /// <param name="lpszUsername">사용자 계정</param>
        /// <param name="lpszDomain">도메인</param>
        /// <param name="lpszPassword">암호</param>
        /// <param name="dwLogonType">로그온 조류</param>
        /// <param name="dwLogonProvider">로그온 프로바이더</param>
        /// <param name="phToken">엑세스 토큰</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("advapi32.dll", EntryPoint = "LogonUser", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
            int dwLogonType, int dwLogonProvider, out int phToken);
        */

        public static IntPtr LogonUser2(string userName, string password, string domainName,
            LogonType logonType, LogonProvider logonProvider)
        {
            int token = 0;
            /*
            bool logonSuccess = LogonUser(userName, domainName, password,
                (int)logonType, (int)logonProvider, out token);
                */
                /*
            if (logonSuccess)
                return new IntPtr(token);
                */
            int retval = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
            throw new System.ComponentModel.Win32Exception(retval);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < System.Configuration.ConfigurationManager.ConnectionStrings.Count; i++)
                {
                    string name = System.Configuration.ConfigurationManager.ConnectionStrings[i].Name;
                    if (name.Equals("LocalSqlServer")) continue;
                    cboDatabase.Items.Add(name);
                }

                cboDatabase.SelectedIndex = 0;

                for (int i = 0; i < args.Length; i++)
                {
                    MessageBox.Show(args[i]);
                }

                /* font 리스트 불러오기 */
                List<string> koreanFonts = new List<string>();
                List<string> otherFonts = new List<string>();
                foreach (FontFamily font in System.Drawing.FontFamily.Families)
                {
                    bool bKorean = false;
                    char[] charArr = font.Name.ToCharArray();

                    foreach (char c in charArr)
                    {
                        if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                        {
                            bKorean = true;
                            break;
                        }
                    }

                    if (bKorean)
                        koreanFonts.Add(font.Name);
                    else
                        otherFonts.Add(font.Name);
                }

                foreach ( string fontName in koreanFonts )
                    cboFontList.Items.Add(fontName);
                foreach (string fontName in otherFonts)
                    cboFontList.Items.Add(fontName);

                cboFontList.SelectedIndex = 0;
                /* font 리스트 불러오기 */
            }
            catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                textBox1.Text = EntFramework.Crypto.CryptoHelper.GetCryptoHelper.AES_Encrypt(EntFramework.Crypto.CryptoHelper.GetCryptoHelper.AESKey
                    , EntFramework.Crypto.CryptoHelper.GetCryptoHelper.AESIV, textBox1.Text);

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr tokenHandle = new IntPtr(0);
            IntPtr dupeTokenHandle = new IntPtr(0);

            try
            {
                /*
                if (args.Length < 3)
                {
                    Console.WriteLine("Usage: DomainName UserName Password");
                    return;
                }
                */

                // args[0] - DomainName
                // args[1] - UserName
                // args[2] - Password

                const int LOGON32_PROVIDER_DEFAULT = 0;
                //This parameter causes LogonUser to create a primary token.
                const int LOGON32_LOGON_INTERACTIVE = 2;
                const int SecurityImpersonation = 2;

                tokenHandle = IntPtr.Zero;
                dupeTokenHandle = IntPtr.Zero;

                // Call LogonUser to obtain an handle to an access token.
                bool returnValue = LogonUser("Ticket2000\administrator", "Ticket2000", "/Exlzpt2018?><", LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    ref tokenHandle);

                if (false == returnValue)
                {
                    Console.WriteLine("LogonUser failed with error code : {0}",
                        Marshal.GetLastWin32Error());
                    MessageBox.Show(string.Format("LogonUser failed with error code : {0}",
                        Marshal.GetLastWin32Error()));
                    return;
                }

                /*
                // Check the identity.
                Console.WriteLine("Before impersonation: "
                    + WindowsIdentity.GetCurrent().Name);

                // The token that is passed to the following constructor must 
                // be a primary token to impersonate.
                WindowsIdentity newId = new WindowsIdentity(tokenHandle);
                WindowsImpersonationContext impersonatedUser = newId.Impersonate();

                // Check the identity.
                Console.WriteLine("After impersonation: "
                    + WindowsIdentity.GetCurrent().Name);

                // Stop impersonating.
                impersonatedUser.Undo();

                // Check the identity.
                Console.WriteLine("After Undo: " + WindowsIdentity.GetCurrent().Name);

                // Free the tokens.
                if (tokenHandle != IntPtr.Zero)
                    CloseHandle(tokenHandle);
                if (dupeTokenHandle != IntPtr.Zero)
                    CloseHandle(dupeTokenHandle);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred. " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //string ar = "20000.0000";

                //MessageBox.Show( Convert.ToDouble(ar).ToString() );

                string a = "20190401";
                string b = "20190401";

                MessageBox.Show(a.CompareTo(b).ToString());

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnEnc_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = EntFramework.Crypto.CryptoHelper.GetCryptoHelper.AES_Decrypt(EntFramework.Crypto.CryptoHelper.GetCryptoHelper.AESKey,
                               EntFramework.Crypto.CryptoHelper.GetCryptoHelper.AESIV, textBox1.Text);

            } catch( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string requestUrl = "http://ipointapi.interpark.com/ipointTx?methodName=avail&memNo=2101410047&comTp=04&firstKind=200&secondKind=036&thirdKind=157&relVc1=P&relVc2=20181022&relVc3=11120&relVc4=&relVc5=&ipAmt=10&occurDept=9999&regNo=1111&rmk=%c5%d7%bd%ba%c6%ae%bb%f3%c7%b011120&";
                XmlDocument xmlDoc = new XmlDocument();

                WebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                // If required by the server, set the credentials.  
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded;charset=euc-kr;";
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("euc-kr"));
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);

                xmlDoc.LoadXml(responseFromServer);

                // Clean up the streams and the response.  
                reader.Close();
                response.Close();

                Console.WriteLine("xmlDoc - " + xmlDoc.OuterXml);

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

                long a = (long)Convert.ToDouble("23456.23");

                MessageBox.Show(a.ToString());

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string time = "20181030";
            DateTime theTime = DateTime.ParseExact(time,
                                                    "yyyyMMdd",
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None);

            MessageBox.Show(theTime.ToString("yyyy-MM-dd"));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SPBackupHelper form = new SPBackupHelper( cboDatabase.SelectedItem.ToString() );
            form.Show();
        }

        class Test
        {
            public string GoodsCode { get; set; }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Test t = new Test();
                t.GoodsCode = "123";

                Test t2 = new Test();
                t2.GoodsCode = "124";

                Test t3 = new Test();
                t3.GoodsCode = "124";

                List<Test> ar = new List<Test>();
                ar.Add(t);
                ar.Add(t2);
                ar.Add(t3);

                int cnt = ar.GroupBy(x => x.GoodsCode).Where( x => x.Count() > 1 ).Count();

                MessageBox.Show(cnt.ToString());
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            statusStrip1.Items[0].Text = "프로그램을 종료합니다.";
        }

        private void closeToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            statusStrip1.Items[0].Text = string.Empty;
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            try
            {
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if ( e.CloseReason != CloseReason.ApplicationExitCall )
            //{
            //    e.Cancel = true;
            //    GoBackground();
            //}
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ComebackFromTray();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            QueryGenerator form = new QueryGenerator();
            form.ShowDialog();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            ComebackFromTray();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            ComebackFromTray();
        }

        private void GoBackground()
        {
            //WindowState = FormWindowState.Minimized;
            //this.ShowInTaskbar = false;
            //notifyIcon1.Visible = true;
            //notifyIcon1.ShowBalloonTip(500);
        }

        private void ComebackFromTray()
        {
            //WindowState = FormWindowState.Normal;
            //this.ShowInTaskbar = true;
            //notifyIcon1.Visible = false;
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnProcPay_Click(object sender, EventArgs e)
        {
            ProcPay dlg = new ProcPay( cboDatabase.SelectedItem.ToString(),txtInput.Text );
            dlg.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string TimsUserID = System.Configuration.ConfigurationManager.AppSettings["TimsUserID"];
            AccessPrivate dlg = new AccessPrivate( cboDatabase.SelectedItem.ToString(),TimsUserID);
            dlg.Show();
        }

        private void btnCoded_Click(object sender, EventArgs e)
        {
            string Code = txtInput.Text;
            Coded dlg = new Coded(cboDatabase.SelectedItem.ToString(), Code, string.Empty);
            dlg.Show();
        }

        private void btnSQLViewer_Click(object sender, EventArgs e)
        {
            SQLView dlg = new SQLView(cboDatabase.SelectedItem.ToString(), null, null );
            dlg.Show();
        }
        
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {

                using (var ws = new WebSocket("wss://localhost:8088/Echo/"))
                {
                    ws.OnMessage += new EventHandler<MessageEventArgs>(this.ws_OnMessage);

                    ws.Connect();
                    ws.Send("BALUS");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ws_OnMessage(object sender, MessageEventArgs e )
        {
            MessageBox.Show("abc");
        }

        private void btnScreen_Click(object sender, EventArgs e)
        {
            try
            {
                PointF dpi = PointF.Empty;
                using (Graphics g = this.CreateGraphics())
                {
                    dpi.X = g.DpiX;
                    dpi.Y = g.DpiY;
                }

                //int convertValue = PrinterUnitConvert.Convert(100, PrinterUnit.Display, PrinterUnit.HundredthsOfAMillimeter);

                MessageBox.Show(dpi.X + " " + dpi.Y);

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                float abc = 0.02f;
                

                MessageBox.Show(abc.ToString());

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string testToString( A a )
        {
            return a.Name;
        }

        public void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font PF2 = new Font("굴림", 11);
            Bitmap bitmap1 = Bitmap.FromHicon(SystemIcons.Hand.Handle);
            Graphics formGraphics = e.Graphics;
            formGraphics.PageUnit = System.Drawing.GraphicsUnit.Millimeter;
            
            Rectangle bmpRectangle = new Rectangle(10, 10, 70, 35);
            Rectangle bmpRectangle_2 = new Rectangle(100, 45, 30, 20);
            formGraphics.DrawString("좌표s", PF2, Brushes.Black, 32, 10);
            formGraphics.DrawRectangle(Pens.Blue, bmpRectangle);
            formGraphics.DrawRectangle(Pens.Blue, bmpRectangle_2);
            formGraphics.Dispose();
            int nMaggin = e.MarginBounds.Bottom;
            int nBotton = e.PageBounds.Bottom;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("GoodsCode", txtInput.Text );
                SQLView dlg = new SQLView(cboDatabase.SelectedItem.ToString(), "GoodsInfo", param);
                dlg.Show();

            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("VoucherPrefix", txtInput.Text);
                SQLView dlg = new SQLView(cboDatabase.SelectedItem.ToString(), "VoucherPrefix", param);
                dlg.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("VoucherPrefix", txtInput.Text);
                SQLView dlg = new SQLView(cboDatabase.SelectedItem.ToString(), "VoucherCode", param);
                dlg.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                매출집계표요약폼 form = new 매출집계표요약폼();
                form.Show();
            } catch ( Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                매출집계표검증폼 form = new 매출집계표검증폼(cboDatabase.SelectedItem.ToString());
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                //Trace.Listeners.Add(new ConsoleTraceListener());
                //Trace.Listeners.Add(new EventLogTraceListener());
                //Trace.AutoFlush = true;
                Trace.WriteLine("===========PointOrder 시작===========");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("BizCode", txtInput.Text);
                SQLView dlg = new SQLView(cboDatabase.SelectedItem.ToString(), "GoodsPayrateBiz", param);
                dlg.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("GoodsCode", txtInput.Text);
                SQLView dlg = new SQLView(cboDatabase.SelectedItem.ToString(), "GoodsPayrate", param);
                dlg.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
