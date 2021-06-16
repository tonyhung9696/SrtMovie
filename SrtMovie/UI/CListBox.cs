using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SrtMovie
{
    public class CListBoxItem
    {
        public static Font f = new Font("Arial", 15, FontStyle.Bold);
        private string _a;
        private string _b;
        private string _c;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public CListBoxItem(int id, string a, string b, string c)
        {
            _id = id;
            _a = a;
            _b = b;
            _c = c;
        }
        public string a
        {
            get { return _a; }
            set { _a = value; }
        }
        public string b
        {
            get { return _b; }
            set { _b = value; }
        }
        public string c
        {
            get { return _c; }
            set { _c = value; }
        }

        public void drawItem(DrawItemEventArgs e, Padding margin,
                             StringFormat aligment,
                             Size imageSize)
        {
            e.Graphics.FillRectangle(Brushes.Black, e.Bounds);

            /*
            var bounds = e.Bounds;
            var buttonBounds = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
            ControlPaint.DrawButton(e.Graphics, buttonBounds, ButtonState.Normal);
            var textBounds = new Rectangle(buttonBounds.X + 2, buttonBounds.Y + 2, buttonBounds.Width - 4, buttonBounds.Height - 4);
            _font = e.Font;
            e.Graphics.DrawString("123123", _font, Brushes.Black, textBounds, StringFormat.GenericDefault);
            */
            // if selected, mark the background differently


            // draw some item separator
            e.Graphics.DrawLine(Pens.DarkGray, e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);
            SizeF stringSize = new SizeF();
            stringSize = e.Graphics.MeasureString(this.c, f, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal);

            Rectangle aBounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                    e.Bounds.Y + 0,
                                                  75,
                                                   (int)stringSize.Height);

            Rectangle bBounds = new Rectangle(e.Bounds.X + margin.Horizontal + 110,
                                                    e.Bounds.Y + 0,
                                                   e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal,
                                                   (int)stringSize.Height);
            Rectangle cBounds = new Rectangle(e.Bounds.X + margin.Horizontal + 220,
                                                    e.Bounds.Y + 0,
                                                   e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal,
                                                   (int)stringSize.Height);
            e.Graphics.DrawString(this.a, f, Brushes.White, aBounds, aligment);
            e.Graphics.DrawString(this.b, f, Brushes.White, bBounds, aligment);
            e.Graphics.DrawString(this.c, f, Brushes.White, cBounds, aligment);
            e.DrawFocusRectangle();

        }

    }

    public partial class CListBox : ListBox
    {
        public static Font TimeFont = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt1Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt2Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        protected Font _font;
        private Size _imageSize = new Size(25, 25);
        private StringFormat _fmt;

        public CListBox(Font timeFont, Font srt1Font, Size srt2Font,
                         StringAlignment aligment, StringAlignment lineAligment)
        {
            this.ItemHeight = 40;
            _fmt = new StringFormat();
            _fmt.Alignment = aligment;
            _fmt.LineAlignment = lineAligment;
        }

        public CListBox()
        {
            InitializeComponent();
            this.ItemHeight = 40;
            _fmt = new StringFormat();
            _fmt.Alignment = StringAlignment.Near;
            _fmt.LineAlignment = StringAlignment.Near;

        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count > 0)
            {
                CListBoxItem item = (CListBoxItem)this.Items[e.Index];
                item.drawItem(e, this.Margin, _fmt, this._imageSize);
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected void listBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
