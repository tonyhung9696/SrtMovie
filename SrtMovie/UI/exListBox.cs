using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SrtMovie
{
    public class exListBoxItem
    {
        public Image imageGoogle = Image.FromFile(Application.StartupPath + "\\img\\google.png");
        public Image imageMicrosoft = Image.FromFile(Application.StartupPath + "\\img\\mircosoft.png");
        public Image imagePlay = Image.FromFile(Application.StartupPath + "\\img\\play.jpg");
        public Image imageList = Image.FromFile(Application.StartupPath + "\\img\\list.png");
        public Color timeColor =System.Drawing.ColorTranslator.FromHtml(json.SubtitlesTimeFontColor);
        public Color Srt1Color = System.Drawing.ColorTranslator.FromHtml(json.SubtitlesSrt1FontColor);
        public Color Srt2Color = System.Drawing.ColorTranslator.FromHtml(json.SubtitlesSrt2FontColor);
        
        public Font _fontTime = new Font("Arial", (float)Convert.ToDouble(json.SubtitlesTimeFontSize), FontStyle.Bold);
        public Font _fontsrt1 = new Font("Arial", (float)Convert.ToDouble(json.SubtitlesSrt1FontSize), FontStyle.Bold);
        public Font _fontsrt2 = new Font("Arial", (float)Convert.ToDouble(json.SubtitlesSrt2FontSize), FontStyle.Bold);
        private string _time;
        private string _srt1;
        private string _srt2;
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public exListBoxItem(int id, string time, string srt1, string srt2)
        {
            _id = id;
            _time = time;
            _srt1 = srt1;
            _srt2 = srt2;
        }

        public string time
        {
            get { return _time; }
            set { _time = value; }
        }

        public string srt1
        {
            get { return _srt1; }
            set { _srt1 = value; }
        }
        public string srt2
        {
            get { return _srt2; }
            set { _srt2 = value; }
        }

        public void drawItem(DrawItemEventArgs e, Padding margin,
                             StringFormat aligment,
                             Size imageSize)
        {
            SolidBrush TimeBrush = new SolidBrush(timeColor);
            SolidBrush Srt1Brush = new SolidBrush(Srt1Color);
            SolidBrush Srt2Brush = new SolidBrush(Srt2Color);
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

            // calculate bounds for title text drawing
            SizeF stringSize = new SizeF();
            stringSize = e.Graphics.MeasureString(this.srt1, _fontsrt1, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal+28);
            SizeF stringSize2 = new SizeF();
            stringSize2 = e.Graphics.MeasureString(this.srt2, _fontsrt2, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal+28);

            Rectangle timeBounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                   e.Bounds.Y + 0,
                                                  e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal,
                                                  (int)_fontTime.GetHeight() + 2);

            // calculate bounds for details text drawing


            Rectangle srt1Bounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                    e.Bounds.Y + timeBounds.Height,
                                                  (int)stringSize.Width,
                                                   (int)stringSize.Height);
            Rectangle srt2Bounds = new Rectangle(e.Bounds.X + margin.Horizontal,
                                                    e.Bounds.Y + timeBounds.Height + srt1Bounds.Height,
                                                  (int)stringSize2.Width,
                                                   (int)stringSize2.Height);
            /*
            Rectangle srt1Bounds = new Rectangle(e.Bounds.X + margin.Horizontal + imageSize.Width,
                                                   e.Bounds.Y + (int)titleFont.GetHeight() + 2 + margin.Vertical + margin.Top,
                                                   e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal,
                                                   e.Bounds.Height - margin.Bottom - (int)titleFont.GetHeight() - 2 - margin.Vertical - margin.Top);
           */
            e.Graphics.DrawString(this.time, _fontTime, TimeBrush, timeBounds, aligment);
            e.Graphics.DrawString(this.srt1, _fontsrt1, Srt1Brush, srt1Bounds, aligment);
            e.Graphics.DrawString(this.srt2, _fontsrt2, Srt2Brush, srt2Bounds, aligment);
            e.Graphics.DrawImage(imageGoogle, e.Bounds.X + margin.Horizontal, e.Bounds.Y + Convert.ToInt32(json.SubtitlesHeight) - 35, imageSize.Width, imageSize.Height);
            e.Graphics.DrawImage(imageMicrosoft, e.Bounds.X + margin.Horizontal + 27, e.Bounds.Y + Convert.ToInt32(json.SubtitlesHeight) - 35, imageSize.Width, imageSize.Height);
            e.Graphics.DrawImage(imagePlay, e.Bounds.X + margin.Horizontal + 54, e.Bounds.Y + Convert.ToInt32(json.SubtitlesHeight) - 35, imageSize.Width, imageSize.Height);
            e.Graphics.DrawImage(imageList, e.Bounds.X + margin.Horizontal + 125, e.Bounds.Y + Convert.ToInt32(json.SubtitlesHeight) - 35, imageSize.Width, imageSize.Height);
            e.DrawFocusRectangle();

        }

    }

    public partial class exListBox : ListBox
    {
        public static Font TimeFont = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt1Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        public static Font srt2Font = new Font("Arial", (float)(Convert.ToDouble(15)), FontStyle.Bold);
        protected Font _font;
        private Size _imageSize = new Size(25, 25);
        private StringFormat _fmt;

        public exListBox(Font timeFont, Font srt1Font, Size srt2Font,
                         StringAlignment aligment, StringAlignment lineAligment)
        {
            this.ItemHeight = 200;
            _fmt = new StringFormat();
            _fmt.Alignment = aligment;
            _fmt.LineAlignment = lineAligment;
        }

        public exListBox()
        {
            InitializeComponent();
            this.ItemHeight = 200;
            _fmt = new StringFormat();
            _fmt.Alignment = StringAlignment.Near;
            _fmt.LineAlignment = StringAlignment.Near;

        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                if (this.Items.Count > 0)
                {
                    exListBoxItem item = (exListBoxItem)this.Items[e.Index];
                    item.drawItem(e, this.Margin, _fmt, this._imageSize);
                }
            }
            catch { }
            
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            /*
            int index = this.IndexFromPoint(e.Location);
            Rectangle bounds = this.GetItemRectangle(index);

            var buttonBounds = new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 4, bounds.Height - 4);
            ControlPaint.DrawButton(Graphics.FromHwnd(this.Handle), buttonBounds, ButtonState.Pushed);

            var textBounds = new Rectangle(buttonBounds.X + 2, buttonBounds.Y + 2, buttonBounds.Width - 4, buttonBounds.Height - 4);
            Graphics.FromHwnd(this.Handle).DrawString(Items[index].ToString(), _font, Brushes.Black, textBounds, StringFormat.GenericDefault);

            Thread.Sleep(100);

            ControlPaint.DrawButton(Graphics.FromHwnd(this.Handle), buttonBounds, ButtonState.Normal);
            Graphics.FromHwnd(this.Handle).DrawString(Items[index].ToString(), _font, Brushes.Black, textBounds, StringFormat.GenericDefault);

            // do what ever you want.
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                MessageBox.Show(index.ToString());
            }
            */
        }
    }
}
