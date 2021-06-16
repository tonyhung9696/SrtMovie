using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SrtMovie
{
    public partial class ChooseColor : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        private static extern int GetPixel(IntPtr hdc, Point p);
        public bool getcolor = false;
        public static int red = 0;
        public static int green = 0;
        public static int blue = 0;
        public static Color colors;
        public string hexcolor = "";
        public string orgColor = "";
        public ChooseColor(string c)
        {
            InitializeComponent();
            orgColor = c;
            pictureBox2.BackColor = System.Drawing.ColorTranslator.FromHtml(orgColor);
            int r = Convert.ToInt16(pictureBox2.BackColor.R);
            int g = Convert.ToInt16(pictureBox2.BackColor.G);
            int b = Convert.ToInt16(pictureBox2.BackColor.B);
            trackBar1.Value = r;
            trackBar2.Value = g;
            trackBar3.Value = b;
            domainUpDown_R.Text = r.ToString();
            domainUpDown_G.Text = g.ToString();
            domainUpDown_B.Text = b.ToString();
        }


        public void change()
        {
            red = trackBar1.Value;
            green = trackBar2.Value;
            blue = trackBar3.Value;
            domainUpDown_R.Text = red.ToString();
            domainUpDown_G.Text = green.ToString();
            domainUpDown_B.Text = blue.ToString();
            pictureBox2.BackColor = Color.FromArgb(red, green, blue);
            //Form1.colors = Color.FromArgb(red, green, blue);
        }
        private void ChooseColor_Load(object sender, EventArgs e)
        {
            
        }
        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            if (getcolor == true)
            {
                Point p = new Point(MousePosition.X, MousePosition.Y);
                IntPtr hdc = GetDC(new IntPtr(0));
                int c = GetPixel(hdc, p);
                int r = (c & 0xFF);
                int g = (c & 0xFF00) / 256;
                int b = (c & 0xFF0000) / 65536;
                domainUpDown_R.Text = r.ToString();
                domainUpDown_G.Text = g.ToString();
                domainUpDown_B.Text = b.ToString();
                trackBar1.Value = r;
                trackBar2.Value = g;
                trackBar3.Value = b;
                pictureBox2.BackColor = Color.FromArgb(r, g, b);
                //Form1.colors = Color.FromArgb(r, g, b);
            }
        }

        private void pictureBox5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            getcolor = true;
            Cursor = Cursors.Cross;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = new Point(MousePosition.X, MousePosition.Y);
            IntPtr hdc = GetDC(new IntPtr(0));
            int c = GetPixel(hdc, p);
            int r = (c & 0xFF);
            int g = (c & 0xFF00) / 256;
            int b = (c & 0xFF0000) / 65536;
            domainUpDown_R.Text = r.ToString();
            domainUpDown_G.Text = g.ToString();
            domainUpDown_B.Text = b.ToString();
            trackBar1.Value = r;
            trackBar2.Value = g;
            trackBar3.Value = b;
            pictureBox2.BackColor = Color.FromArgb(r, g, b);
            //Form1.colors = Color.FromArgb(r, g, b);
            getcolor = false;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            change();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            change();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            change();
        }
        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hexcolor = HexConverter(pictureBox2.BackColor);
            this.Close();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
