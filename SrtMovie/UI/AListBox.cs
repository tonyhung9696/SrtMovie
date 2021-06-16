using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SrtMovie
{
    public class AListBoxItem
    {
        public static Font f = new Font("Arial", 15, FontStyle.Bold);
        public static Font f2 = new Font("Arial", 14, FontStyle.Bold);
        private string _a;
        private string _define;
        private string _sentence;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public AListBoxItem(int id, string a, string define, string sentence)
        {
            _id = id;
            _a = a;
            _define = define;
            _sentence = sentence;
        }

        public string a
        {
            get { return _a; }
            set { _a = value; }
        }

        public string define
        {
            get { return _define; }
            set { _define = value; }
        }
        public string sentence
        {
            get { return _sentence; }
            set { _sentence = value; }
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
            stringSize = e.Graphics.MeasureString(this._define, f, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal);
            SizeF stringSize2 = new SizeF();
            stringSize2 = e.Graphics.MeasureString(this._sentence, f, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal);

            Rectangle aBounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                   e.Bounds.Y + 0,
                                                  e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal,
                                                  (int)f.GetHeight() + 2);

            Rectangle defineBounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                    e.Bounds.Y + aBounds.Height,
                                                  (int)stringSize.Width,
                                                   (int)stringSize.Height);
            Rectangle sentenceBounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                    e.Bounds.Y + aBounds.Height + defineBounds.Height + 10,
                                                  (int)stringSize2.Width,
                                                   (int)stringSize2.Height);
            e.Graphics.DrawString(this.a, f, Brushes.White, aBounds, aligment);
            e.Graphics.DrawString(this.define, f, Brushes.White, defineBounds, aligment);
            e.Graphics.DrawString(this.sentence, f2, Brushes.LightGray, sentenceBounds, aligment);
            e.DrawFocusRectangle();

        }

    }

    public partial class AListBox : ListBox
    {
        public static Font TimeFont = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt1Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt2Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        protected Font _font;
        private Size _imageSize = new Size(25, 25);
        private StringFormat _fmt;

        public AListBox(Font timeFont, Font srt1Font, Size srt2Font,
                         StringAlignment aligment, StringAlignment lineAligment)
        {
            this.ItemHeight = 150;
            _fmt = new StringFormat();
            _fmt.Alignment = aligment;
            _fmt.LineAlignment = lineAligment;
        }

        public AListBox()
        {
            InitializeComponent();
            this.ItemHeight = 150;
            _fmt = new StringFormat();
            _fmt.Alignment = StringAlignment.Near;
            _fmt.LineAlignment = StringAlignment.Near;

        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count > 0)
            {
                AListBoxItem item = (AListBoxItem)this.Items[e.Index];
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
