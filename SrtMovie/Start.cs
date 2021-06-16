using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SrtMovie
{
    public partial class Start : Form
    {
        public static controlWeb CW;
        bool drag;
        int mousex;
        int mousey;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        public Start()
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
        
        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            mousex = System.Windows.Forms.Cursor.Position.X - this.Left;
            mousey = System.Windows.Forms.Cursor.Position.Y - this.Top;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag == true)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - mousey;
                this.Left = System.Windows.Forms.Cursor.Position.X - mousex;
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        private void Start_Load(object sender, EventArgs e)
        {
            json.getsetting();
            load();
            Thread t1 = new Thread(openControlWeb);
            t1.Start();
            /*
            string text = File.ReadAllText("C:\\Users\\tony\\Desktop\\test.srt");
            if (text.IndexOf("1\r\n")==0)
            {
                text = text.Substring(1, text.Length - 1).Trim();
            }
            string strRegex = @"\r\n[\d]?[\d]?[\d]?[\d]?[\d]?[\d]?[\d]?[\d]\r\n";
            string[] str = Regex.Split(text, strRegex);
            foreach (string s in str)
            {
                string[] o = Regex.Split(s, "\r\n");
                int count = o.Length;
                string time = o[0];
                string line1 = o[1];
                string line2 = o[2];
                string allline = line1 +" " +line2;
            }
            */
        }
        public void openControlWeb()
        {
            CW = new controlWeb();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            CW.close();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Open form = new Open();
            int x = this.Location.X;
            int y = this.Location.Y;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(x , y );
            form.ShowDialog();
            this.Show();
            load();
        }
        public void load()
        {
            string cmd = "";
            cmd = String.Format("select * from [History] order by [datetime] desc");
            staticvalue.sqlite.selectcmd(cmd);
            SQLiteDataReader reader = staticvalue.sqlite.reader;
            DataTable data = new DataTable();
            data.Load(reader);
            reader.Close();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = data;
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 13.5F);
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AddColumns(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MovieTable form = new MovieTable();
            int x = this.Location.X;
            int y = this.Location.Y;
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(x, y);
            form.ShowDialog();
            this.Show();
        }
        public void AddColumns(DataGridView DGV)
        {
            DataGridViewLinkColumn buttonColumn = new DataGridViewLinkColumn();
            buttonColumn.HeaderText = "Action";
            buttonColumn.Name = "Action";
            buttonColumn.Text = "Watch";
            buttonColumn.UseColumnTextForLinkValue = true;
            dataGridView1.Columns.Add(buttonColumn);
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            dataGridView1.CellClick+=HandleCellClick;
        }
        public void HandleCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0 && dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                DataGridView dgv = (DataGridView)sender;
                string title = dgv.Rows[e.RowIndex].Cells["title"].Value.ToString();
                string ep = dgv.Rows[e.RowIndex].Cells["ep"].Value.ToString();
                string cmd = String.Format("select * from [Movie] where [title]='{0}' and [ep]='{1}'", title, ep);
                staticvalue.sqlite.selectcmd(cmd);
                SQLiteDataReader reader = staticvalue.sqlite.getreader();
                reader.Read();
                string moviepath = reader["moviepath"].ToString();
                string srtpath1 = reader["srtpath1"].ToString();
                string srtpath2 = reader["srtpath2"].ToString();
                dgv.EndEdit();
                bool moviepathExist = function.fileExist(moviepath);
                bool srtpath1Exist = function.fileExist(srtpath1);
                bool srtpath2Exist = function.fileExist(srtpath2);
                if (moviepathExist==false || srtpath1Exist==false ||srtpath2Exist==false)
                {
                    MessageBox.Show("Your file path may not exist!");
                }
                else
                {
                    this.Hide();
                    Form1 form = new Form1(moviepath, srtpath1, srtpath2, title, ep);
                    form.ShowDialog();
                    this.Show();
                    load();
                }
                
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Setting form = new Setting();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();

        }
    }
}
