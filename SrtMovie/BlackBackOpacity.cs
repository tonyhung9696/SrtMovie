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
    public partial class BlackBackOpacity : Form
    {
        public int width = 0;
        public int height = 0;
        public int x = 0;
        public int y = 400;
        Form f;
        bool drag;
        int mousex;
        int mousey;
        int mouseyy;
        public BlackBackOpacity(Form formToCover)
        {
            Point p = new Point(500, y);
            InitializeComponent();
            this.AllowTransparency = true;
            this.Opacity = 0.55;
            //this.TransparencyKey = Color.Black;
            //this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.Location = formToCover.PointToScreen(p);
            this.Top = this.Top + Form1.panelmovetop;
            this.ClientSize = new Size(width, height);
            this.Height= this.Height- Form1.panelmovetop;
            formToCover.LocationChanged += Cover_LocationChanged;
            formToCover.ClientSizeChanged += Cover_ClientSizeChanged;
            this.Show(formToCover);
            formToCover.Focus();
            f = formToCover;

            // Disable Aero transitions, the plexiglass gets too visible
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int value = 1;
                DwmSetWindowAttribute(formToCover.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            }
        }
        private void Cover_LocationChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass follows the owner
            Point p = new Point(500, y+34);
            this.Location = this.Owner.PointToScreen(p);
        }

        private void Cover_ClientSizeChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass keeps the owner covered
            //this.ClientSize = this.Owner.ClientSize;
            //this.Height = this.Height - Form1.panelmovetop;
            this.ClientSize = new Size(width, height);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Restore owner
            this.Owner.LocationChanged -= Cover_LocationChanged;
            this.Owner.ClientSizeChanged -= Cover_ClientSizeChanged;
            if (!this.Owner.IsDisposed && Environment.OSVersion.Version.Major >= 6)
            {
                int value = 1;
                DwmSetWindowAttribute(this.Owner.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            }
            base.OnFormClosing(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            // Always keep the owner activated instead
            this.BeginInvoke(new Action(() => this.Owner.Activate()));
        }

        private const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);

        private void BlackBackOpacity_Load(object sender, EventArgs e)
        {
            //panel4.Width = panel6.Width;
            //label1.MaximumSize = new Size(panel6.Width, 100);
            //label3.MaximumSize = new Size(panel6.Width, 100);
            //label3.Top = label1.Height + 23;

            //pictureBox1.Top = label1.Height + label3.Height + 30;
            //pictureBox2.Top = label1.Height + label3.Height + 30;
            //pictureBox3.Top = label1.Height + label3.Height + 30;
            //panel4.Height = label1.Height + label3.Height + 23 + 25 + 10;
        }
       
        public void setSize(int w, int h)
        {
            try
            {
                width = w;
                height = h;
                if (w == 0)
                {
                    this.Hide();
                }
                else
                {
                    this.Show();
                    this.ClientSize = new Size(width, height);
                }
            }
            catch { }
        }
        public void setHide()
        {
            this.Hide();
        }
        public void setShow()
        {
            this.Show();
        }

        public void setLocationX(int xx)
        {
            try
            {
                x = xx;
                Point p = new Point(x, y + 34);
                this.Location = this.Owner.PointToScreen(p);
            }
            catch { }
        }
        public void setLocationY(int yy)
        {
            try
            {
                y = yy;
                Point p = new Point(x, y + 34);
                this.Location = this.Owner.PointToScreen(p);
            }
            catch { }
        }
        private void BlackBackOpacity_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            mousex = System.Windows.Forms.Cursor.Position.X - this.Left;
            mousey = System.Windows.Forms.Cursor.Position.Y - this.Top;
        }

        private void BlackBackOpacity_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == true)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - mousey;
                this.Left = System.Windows.Forms.Cursor.Position.X - mousex;
                y = this.Top- this.Owner.Top-34;
            }
        }

        private void BlackBackOpacity_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
    }
}
