using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace SrtMovie
{
    public partial class Open : Form
    {
        //public static controlWeb CW;
        bool drag;
        int mousex;
        int mousey;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        public Open()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            mousex = System.Windows.Forms.Cursor.Position.X - this.Left;
            mousey = System.Windows.Forms.Cursor.Position.Y - this.Top;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == true)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - mousey;
                this.Left = System.Windows.Forms.Cursor.Position.X - mousex;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void Open_Load(object sender, EventArgs e)
        {
            json.setup_json();
            //Thread t1 = new Thread(openControlWeb);
            //t1.Start();
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string filename = (e.Data.GetData((DataFormats.FileDrop)) as string[])[0];
            textBox1.Text = filename;
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            string filename = (e.Data.GetData((DataFormats.FileDrop)) as string[])[0];
            textBox2.Text = filename;
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void textBox3_DragDrop(object sender, DragEventArgs e)
        {
            string filename = (e.Data.GetData((DataFormats.FileDrop)) as string[])[0];
            textBox3.Text = filename;
        }

        private void textBox3_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string moviepath = textBox1.Text;
            string srtpath1 = textBox2.Text;
            string srtpath2 = textBox3.Text;
            string title = textBox4.Text;
            string ep = textBox5.Text;
            if (moviepath!="" || srtpath1 !="")
            {
                if (title == "" && ep == "")
                {
                    this.Hide();
                    Form1 form = new Form1(moviepath,srtpath1,srtpath2,title,ep);
                    form.ShowDialog();
                    this.Show();
                }
                else if (title != "" && ep != "")
                {
                    string cmd = String.Format("select count(*) from [Movie] where [moviepath]='{0}'", moviepath);
                    staticvalue.sqlite.countcmd(cmd);
                    int count = staticvalue.sqlite.getcount();
                    if (count<=0)
                    {
                        staticvalue.sqlite.sqlcmd(String.Format("insert into [Movie] ([ep],[title],[moviepath],[srtpath1],[srtpath2]) values ('{0}','{1}','{2}','{3}','{4}')", ep, title, moviepath, srtpath1, srtpath2));
                    }
                    this.Hide();
                    Form1 form = new Form1(moviepath, srtpath1, srtpath2, title, ep);
                    form.ShowDialog();
                    this.Show();
                }
                else if (title != "" || ep != "")
                {
                    MessageBox.Show("請填上完整資料");
                }
            }
            else
            {
                MessageBox.Show("請填上完整資料");
            }
            

        }

        private void Button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Setting form = new Setting();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }
    }
}
