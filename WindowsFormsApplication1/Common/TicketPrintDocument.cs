using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

namespace TimsHelper.Common
{
    public class TicketPrintDocument : System.Drawing.Printing.PrintDocument
    {
        private Font _font;
        private string _text;

        public string TextToPrint
        {
            get { return _text; }
            set { _text = value; }
        }

        public Font PrinterFont
        {
            // Allows the user to override the default font
            get { return _font; }
            set { _font = value; }
        }

        static int curChar;
        
        public TicketPrintDocument() : base()
{
            //Set the file stream
            //Instantiate out Text property to an empty string
            _text = string.Empty;
        }

        /// <summary>
        /// Constructor to initialize our printing object
        /// and the text it's supposed to be printing
        /// </summary>
        /// <param name=str>Text that will be printed</param>
        /// <remarks></remarks>
        public TicketPrintDocument(string str) : base()
        {
            _text = str;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);

            if (_font == null)
            {
                //Create the font we need
                _font = new Font("Times New Roman", 10);
            }
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            int printHeight;
            int printWidth;
            int leftMargin;
            int rightMargin;
            Int32 lines;
            Int32 chars;

            //Set print area size and margins
            {
                printHeight = base.DefaultPageSettings.PaperSize.Height - base.DefaultPageSettings.Margins.Top - base.DefaultPageSettings.Margins.Bottom;
                printWidth = base.DefaultPageSettings.PaperSize.Width - base.DefaultPageSettings.Margins.Left - base.DefaultPageSettings.Margins.Right;
                leftMargin = base.DefaultPageSettings.Margins.Left;  //X
                rightMargin = base.DefaultPageSettings.Margins.Top;  //Y
            }

            //Check if the user selected to print in Landscape mode
            //if they did then we need to swap height/width parameters
            if (base.DefaultPageSettings.Landscape)
            {
                int tmp;
                tmp = printHeight;
                printHeight = printWidth;
                printWidth = tmp;
            }

            Font PF2 = new Font("굴림", 11);
            Bitmap bitmap1 = Bitmap.FromHicon(SystemIcons.Hand.Handle);
            Graphics formGraphics = e.Graphics;
            formGraphics.PageUnit = System.Drawing.GraphicsUnit.Millimeter;

            Rectangle bmpRectangle = new Rectangle(0, 0, 100, 32);
            Rectangle bmpRectangle_2 = new Rectangle(150, 140, 30, 50);
            Rectangle bmpRectangle_3 = new Rectangle(80, 95, 20, 18);
            Rectangle bmpRectangle_4 = new Rectangle(20, 100, 30, 50);
            formGraphics.DrawString("좌표s", PF2, Brushes.Black, 32, 10);
            bmpRectangle.Width = 100;
            formGraphics.DrawRectangle(Pens.Blue, bmpRectangle);
            formGraphics.DrawRectangle(Pens.Blue, bmpRectangle_2);
            formGraphics.DrawRectangle(Pens.Blue, bmpRectangle_3);
            formGraphics.DrawRectangle(Pens.Blue, bmpRectangle_4);
            formGraphics.Dispose();
            int nMaggin = e.MarginBounds.Bottom;
            int nBotton = e.PageBounds.Bottom;

            e.HasMorePages = false;
        }

        public int RemoveZeros(int value)
        {
            //Check the value passed into the function,
            //if the value is a 0 (zero) then return a 1,
            //otherwise return the value passed in
            switch (value)
            {
                case 0:
                    return 0;
                default:
                    return value;
            }
        }
    }
}
