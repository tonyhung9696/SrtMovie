using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SrtMovie
{
    public partial class MovieTable : Form
    {
        bool drag;
        int mousex;
        int mousey;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        int recordID = 0;
        public MovieTable()
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

        private void MovieTable_Load(object sender, EventArgs e)
        {
            load();
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

        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox6.Text;
            string moviepath = textBox1.Text;
            string srtpath1 = textBox2.Text;
            string srtpath2 = textBox3.Text;
            string title = textBox4.Text;
            string ep = textBox5.Text;
            string cmd = "";
            cmd = String.Format("select count(*) from [Movie] where [id]='{0}'",id);
            staticvalue.sqlite.countcmd(cmd);
            int count = staticvalue.sqlite.getcount();
            if (count>0)
            {
                cmd = String.Format("update [Movie] set [ep]='{0}',[title]='{1}',[moviepath]='{2}',[srtpath1]='{3}',[srtpath2]='{4}'",ep,title,moviepath,srtpath1,srtpath2);
                staticvalue.sqlite.sqlcmd(cmd);
                load();
                dataGridView1.CurrentCell = dataGridView1.Rows[recordID].Cells[0];
                dataGridView1.Rows[recordID].Selected = true;
            }
            else
            {
                cmd = String.Format("insert into [Movie] ([ep],[title],[moviepath],[srtpath1],[srtpath2]) values ('{0}','{1}','{2}','{3}','{4}')",ep,title,moviepath,srtpath1,srtpath2);
                staticvalue.sqlite.sqlcmd(cmd);
                load();
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells[0];
                dataGridView1.Rows[dataGridView1.Rows.Count-2].Selected = true;
                
            }
            MessageBox.Show("OK");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string id = textBox6.Text;
            string cmd = String.Format("delete from [Movie] where [id]='{0}'", id);
            staticvalue.sqlite.sqlcmd(cmd);
            load();
            if (recordID - 1>=0)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[recordID - 1].Cells[0];
                dataGridView1.Rows[recordID - 1].Selected = true;
            }
            else
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                dataGridView1.Rows[0].Selected = true;
            }
           
            MessageBox.Show("OK");
        }
        public void load()
        {
            string cmd = "";
            cmd = String.Format("select * from [Movie] order by [Title]");
            staticvalue.sqlite.selectcmd(cmd);
            SQLiteDataReader reader = staticvalue.sqlite.reader;
            DataTable data = new DataTable();
            data.Load(reader);
            reader.Close();
            dataGridView1.DataSource = data;
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 13.5F);
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int RowIndex = dataGridView1.CurrentCell.RowIndex;
                int ColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
                if (dataGridView1.Rows[RowIndex].Cells[ColumnIndex].Value != null)
                {
                    string id = dataGridView1.Rows[RowIndex].Cells["id"].FormattedValue.ToString();
                    string title = dataGridView1.Rows[RowIndex].Cells["title"].FormattedValue.ToString();
                    string ep = dataGridView1.Rows[RowIndex].Cells["ep"].FormattedValue.ToString();
                    string moviepath = dataGridView1.Rows[RowIndex].Cells["moviepath"].FormattedValue.ToString();
                    string srtpath1 = dataGridView1.Rows[RowIndex].Cells["srtpath1"].FormattedValue.ToString();
                    string srtpath2 = dataGridView1.Rows[RowIndex].Cells["srtpath2"].FormattedValue.ToString();

                    textBox1.Text = moviepath;
                    textBox2.Text = srtpath1;
                    textBox3.Text = srtpath2;
                    textBox4.Text = title;
                    textBox5.Text = ep;
                    textBox6.Text = id;
                }
                recordID = RowIndex;
            }
            catch { }
        }
    }
}
