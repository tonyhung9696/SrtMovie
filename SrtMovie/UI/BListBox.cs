using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SrtMovie
{
    public class BListBoxItem
    {
        public static Font f = new Font("Arial", 15, FontStyle.Bold);
        private string _sentence;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public BListBoxItem(int id, string sentence)
        {
            _id = id;
            _sentence = sentence;
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
            stringSize = e.Graphics.MeasureString(this._sentence, f, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal);

            Rectangle sentenceBounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                    e.Bounds.Y + 0,
                                                  (int)stringSize.Width,
                                                   (int)stringSize.Height);
            e.Graphics.DrawString(this.sentence, f, Brushes.White, sentenceBounds, aligment);
            e.DrawFocusRectangle();

        }

    }

    public partial class BListBox : ListBox
    {
        public static Font TimeFont = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt1Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt2Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        protected Font _font;
        private Size _imageSize = new Size(25, 25);
        private StringFormat _fmt;

        public BListBox(Font timeFont, Font srt1Font, Size srt2Font,
                         StringAlignment aligment, StringAlignment lineAligment)
        {
            this.ItemHeight = 63;
            _fmt = new StringFormat();
            _fmt.Alignment = aligment;
            _fmt.LineAlignment = lineAligment;
        }

        public BListBox()
        {
            InitializeComponent();
            this.ItemHeight = 63;
            _fmt = new StringFormat();
            _fmt.Alignment = StringAlignment.Near;
            _fmt.LineAlignment = StringAlignment.Near;

        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count > 0)
            {
                BListBoxItem item = (BListBoxItem)this.Items[e.Index];
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
