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
    public partial class LabelText : Form
    {
        public LabelText()
        {
            InitializeComponent();
        }
        public LabelText(Form formToCover)
        {
            InitializeComponent();
            this.AllowTransparency = true;
            this.TransparencyKey = Color.Black;
            this.BackColor = Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.Location = formToCover.PointToScreen(Point.Empty);
            this.ClientSize = formToCover.ClientSize;
            formToCover.LocationChanged += Cover_LocationChanged;
            formToCover.ClientSizeChanged += Cover_ClientSizeChanged;
            this.Show(formToCover);
            formToCover.Focus();

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
            this.Location = this.Owner.PointToScreen(Point.Empty);
        }

        private void Cover_ClientSizeChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass keeps the owner covered
            this.ClientSize = this.Owner.ClientSize;
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
        private void LabelText_Load(object sender, EventArgs e)
        {

        }
        public Point getSize()
        {
            Point size = new Point(label1.Width, label1.Height+2);
            return size;
        }
        public void setText(string t)
        {
            label1.Text = t;
        }
        public void setFontSize()
        {
            label1.Font = new Font("Microsoft JhengHei UI", (float)(Convert.ToDouble(json.MovieSrt1FontSize)), FontStyle.Bold);
        }
    }
}
