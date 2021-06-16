using SpeechLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class Form1 : Form
    {

        Point labelSize = new Point();
        BlackBackOpacity fBBO;
        LabelText fLT;

        private delegate void resizePictureBox1(int panelHeight);
        public static int panelmovetop = 34;

        Panel nP = new Panel();
        Panel resizePanel = new Panel();
        Label resizeLabel = new Label();
        PictureBox resizePictureBox = new PictureBox();
        bool resize = false;
        bool finishresize = true;
        bool panel6Enable = false;
        int resizestart = 0;
        DateTime resizeStartTime = DateTime.Now;  

        int lasttop = 0;
        int lastleft = 0;
        bool drag;
        int mousex;
        int mousey;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        int srt1count = 0;
        int srt2count = 0;
        string moviepath="";
        string srtpath1="";
        string srtpath2="";
        string title = "";
        string ep = "";
        List<Srt> srts = new List<Srt>();

        int panel6Width = 0;
        int heightsplite = 2;
        int paneltop = 0;
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }
        public void loadSrt()
        {
            if (srtpath1 != "")
            {
                readSrt(srtpath1, 1);
            }
            if (srtpath2 != "")
            {
                readSrt(srtpath2, 2);
            }
        }
        public Form1(string m,string srt1,string srt2,string t, string e)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            moviepath = m;
            srtpath1 = srt1;
            srtpath2 = srt2;
            title = t;
            if (e!="")
            {
                ep = e;
            }
            else
            {
                ep = "0";
            }
            loadSrt();
            if (m!="")
            {
                axWindowsMediaPlayer1.URL = m;
                axWindowsMediaPlayer1.stretchToFit = true;
                axWindowsMediaPlayer1.Size = this.Size;
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.windowlessVideo = true;
                //axWindowsMediaPlayer1.uiMode = "none";
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    axWindowsMediaPlayer1.fullScreen = true;
                }
            }
            if (moviepath!="")
            {
                fBBO = new BlackBackOpacity(this);
                fBBO.Show();
                fBBO.setHide();
                fLT = new LabelText(fBBO);
                fLT.Show();
                labelSize = fLT.getSize();
                fBBO.setSize(labelSize.X, labelSize.Y);
                int test = axWindowsMediaPlayer1.currentMedia.imageSourceWidth - (labelSize.X / 2) + panel6.Width;
                fBBO.setLocationX(test);
            }
            
            
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
                
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    resizeStartTime = DateTime.Now;
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    resize = true;
                }
                panel1.Width = this.Width - 4;
                panel1.Height = this.Height - 37;
                return;
            }

            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            json.getsetting();
            exListBox1.ItemHeight = Convert.ToInt32(json.SubtitlesHeight);
            this.DoubleBuffered = true;
            string test = "a" + Environment.NewLine + "b";
            string[] tests = Regex.Split(test,"\\r\\n", RegexOptions.IgnoreCase);

            panel6Width = panel6.Width;
            Thread t1 = new Thread(addSrt);
            t1.Start();
            //MethodInvoker TA = new MethodInvoker(addSrt);
            //this.Invoke(TA);
            //addSrt();
            panel6.MouseWheel += new MouseEventHandler(Panel1_MouseWheel);
            checkSettingDiff();
        }
        private void Button14_Click(object sender, EventArgs e)
        {
            fBBO.Close();
            fLT.Close();
            if (title!="")
            {
                int currentSec = Convert.ToInt32(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
                int sec = currentSec % 60;
                int min = currentSec / 60;
                int hour = currentSec / 60 / 60;
                string time = String.Format("{0:00}:{1:00}:{2:00}",hour,min,sec);
                string cmd = String.Format("select count(*) from [History] where [title]='{0}' and [ep]='{1}'", title, ep);
                staticvalue.sqlite.countcmd(cmd);
                int count = staticvalue.sqlite.getcount();
                if (count>0)
                {
                    cmd = String.Format("update [History] set [datetime]='{0}', [LastWatchTime]='{1}'",DateTime.Now.ToString("yyyy-MM-dd hh:mm"),time);
                    staticvalue.sqlite.sqlcmd(cmd);
                }
                else
                {
                    cmd = String.Format("insert into [History] ([ep],[title],[datetime],[LastWatchTime]) values ('{0}','{1}','{2}','{3}')",ep,title, DateTime.Now.ToString("yyyy-MM-dd hh:mm"), time);
                    staticvalue.sqlite.sqlcmd(cmd);
                }
            }
            this.Close();
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Width == System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width || this.Height== System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Normal;
                this.Top = lasttop;
                this.Left = lastleft;
                this.Width = 1024;
                this.Height = 768;
            }
            else
            {
                this.StartPosition = FormStartPosition.Manual;
                if (lasttop==0)
                {
                    lasttop = this.Top;
                }
                if (lastleft==0)
                {
                    lastleft = this.Left;
                }
                    
                this.Top = 0;
                this.Left = 0;
                //this.Size = new Size(screenWidth,screenHeight);
                this.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
                this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
                this.WindowState = FormWindowState.Normal;
            }
            if (finishresize == true)
            {
                resizeStartTime = DateTime.Now;
                resize = true;
            }

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            mousex = System.Windows.Forms.Cursor.Position.X - this.Left;
            mousey = System.Windows.Forms.Cursor.Position.Y - this.Top;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag==true)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - mousey;
                this.Left = System.Windows.Forms.Cursor.Position.X - mousex;
            }
        }
        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.WindowState==FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
            if (finishresize == true)
            {
                resizeStartTime = DateTime.Now;
                resize = true;
            }
            Application.DoEvents();
            
        }

        public void readSrt(string filepath,int srt)
        {
            string text = File.ReadAllText(filepath,Encoding.UTF8);
            if (text.IndexOf("1\r\n") == 0)
            {
                text = text.Substring(1, text.Length - 1).Trim();
            }
            string strRegex = @"\r\n[\d]?[\d]?[\d]?[\d]?[\d]?[\d]?[\d]?[\d]\r\n";
            string[] str = Regex.Split(text, strRegex);
            if (srt==1)
            {
                srt1count = str.Length;
            }
            else
            {
                srt2count = str.Length;
            }
            int id = 1000;
            foreach (string s in str)
            {
                string[] o = Regex.Split(s, "\r\n");
                int count = o.Length;
                string time = o[0];
                string[] times = Regex.Split(time, "-->");
                string starttime = times[0];
                string endtime = times[1];
                string line1 = o[1];
                string line2 = "";
                if (o.Length>=3)
                {
                    line2= o[2]; 
                }
                string allline = line1 + " " + line2;
                if (srt==1)
                {
                    Srt st = new Srt();
                    st.id = id;
                    st.time = time;
                    st.starttime = starttime;
                    st.endtime = endtime;
                    st.srt1line1 = line1;
                    st.srt1line2 = line2;
                    st.srt1 = allline;
                    string[] timeSE = Regex.Split(st.time, "-->");
                    string[] timesplie = timeSE[0].Split(':');
                    string[] timesplie2 = timeSE[1].Split(':');
                    int l = 0;
                    int startsec = 0;
                    for (int i=(timesplie.Length-1);i>=0;i--)
                    {
                        if (l==0)
                        {
                            string[] sss = timesplie[i].Split(',');
                            int ss = (Convert.ToInt32(sss[1].Replace(",","")) * 100 / 1000)+ Convert.ToInt32(sss[0])*100;
                            startsec = ss;
                        }
                        else
                        {
                            int ss = Convert.ToInt32(Convert.ToInt32(timesplie[i])* Math.Pow(Convert.ToDouble(60),Convert.ToDouble(l)) * 100);
                            startsec = startsec + ss;
                        }
                        l++;
                    }
                    int j = 0;
                    int endsec = 0;
                    for (int i = (timesplie2.Length - 1); i >= 0; i--)
                    {
                        if (j == 0)
                        {
                            string[] sss = timesplie2[i].Split(',');
                            int ss = (Convert.ToInt32(sss[1].Replace(",", "")) * 100 / 1000) + Convert.ToInt32(sss[0]) * 100;
                            endsec = ss;
                        }
                        else
                        {
                            int ss = Convert.ToInt32(Convert.ToInt32(timesplie2[i]) * Math.Pow(Convert.ToDouble(60), Convert.ToDouble(j)) * 100);
                            endsec = endsec + ss;
                        }
                        j++;
                    }
                    st.startsec = startsec;
                    st.endsec = endsec;
                    srts.Add(st);
                }
                else
                {
                    if (srt1count==srt2count)
                    {
                        srts.Where(w => w.id == id).ToList().ForEach(x => { x.srt2 = allline;x.srt2line1 = line1;x.srt2line2 = line2; });
                    }
                }
                id++;
            }
            if (srt1count != srt2count && srt==2)
            {
                MessageBox.Show("Not Match");
            }
        }
        public void addSrt()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(addSrt));
            }
            else
            {
                int count = srts.Count;
                if (count > 0)
                {
                    int i = 0;
                    foreach (Srt st in srts)
                    {
                        //addSrtPanel(st);
                        exListBox1.Items.Add(new exListBoxItem(st.id, st.starttime.Substring(0, st.starttime.IndexOf(",")) + " > " + st.endtime.Substring(0, st.endtime.IndexOf(",")).Trim(), st.srt1, st.srt2));
                    }
                    panel6Enable = true;
                }
            }
        }
        protected void PictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            Panel p = (Panel)pb.Parent;
            foreach (Control c in p.Controls)
            {
                if (c.Name=="srt1")
                {

                    Start.CW.g.tranText2(c.Text);
                    Start.CW.g.speak();
                }
            }
        }
        protected void PictureBox2_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            Panel p = (Panel)pb.Parent;
            foreach (Control c in p.Controls)
            {
                if (c.Name == "srt1")
                {
                    SpVoiceClass voice = new SpVoiceClass();
                    voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(json.msspeech);//Item(0)男聲
                    voice.Speak(c.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                }
            }
        }
        protected void PictureBox3_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            Panel p = (Panel)pb.Parent;
            
            foreach (Control c in p.Controls)
            {
                if (c.Name == "time")
                {
                    int startsec = 0;
                    string[] times = Regex.Split(c.Text, ">");
                    string[] timesplie = times[0].Split(':');
                    int l = 1;
                    for (int i = (timesplie.Length - 1); i >= 0; i--)
                    {
                        if (l == 1)
                        {
                            string[] sss = timesplie[i].Split(',');
                            int ss = Convert.ToInt32(sss[0]);
                            startsec = ss;
                        }
                        else
                        {
                            int ss = Convert.ToInt32(timesplie[i]) * (60 ^ l);
                            startsec = startsec + ss;
                        }
                        l++;
                    }
                    axWindowsMediaPlayer1.Ctlcontrols.currentPosition = startsec;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
        }
        protected void PictureBox4_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            Panel p = (Panel)pb.Parent;
            foreach (Control c in p.Controls)
            {
                if (c.Name == "srt1")
                {
                    Detail form = new Detail(c.Text);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
            }
        }
        public void addpanel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(addpanel));
            }
            else
            {
                panel6.Controls.Add(nP);
            }
        }
        
        
        public void changelabel()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(changelabel));
            }
            else
            {
                resizeLabel.MaximumSize = new Size(panel6.Width - 30, 60);
                resizeLabel.Width = panel6.Width;
            }
        }
        public void changePictureBox(int panelheight)
        {
            if (this.InvokeRequired)
            {
                //this.Invoke(new MethodInvoker(changelabel));
                resizePictureBox1 rPB = new resizePictureBox1(changePictureBox);
                this.Invoke(rPB, new object[] { panelheight });
            }
            else
            {
                resizePictureBox.Top = panelheight - 30 - heightsplite;
            }
        }
        private void panel6_SizeChanged(object sender, EventArgs e)
        {

        }

        private void panel6_Scroll(object sender, ScrollEventArgs e)
        {

        }
        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            try
            {
                exListBox1.Refresh();
                resizeStartTime = DateTime.Now;
                resize = true;
            }
            catch (Exception ex)
            {

            }
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (panel6Enable==true)
            {
                panel6.Enabled = true;
                panel6Enable = false;
            }

            if (moviepath != "")
            {
                double currentSec = axWindowsMediaPlayer1.Ctlcontrols.currentPosition * 100;
                int count = srts.Where(w => w.startsec <= currentSec && w.endsec >= currentSec).Count();
                if (count > 0)
                {
                    var v = srts.Where(w => w.startsec <= currentSec && w.endsec >= currentSec).First();
                    string srt1 = v.srt1line1 + Environment.NewLine + v.srt1line2;
                    string srt2 = v.srt2line1 + Environment.NewLine + v.srt2line2;
                    string text = "";
                    if (json.Subtitles=="S1S2")
                    {
                        if (v.srt2line2!="")
                        {
                            text = srt1 + Environment.NewLine + srt2;
                        }
                        else
                        {
                            text = srt1;
                        }
                    }
                    else if (json.Subtitles=="S1")
                    {
                        text = srt1;
                    }
                    else if (json.Subtitles=="S2")
                    {
                        text = srt2;
                    }
                    fLT.setText(text);
                    labelSize = fLT.getSize();
                    fBBO.setSize(labelSize.X, labelSize.Y);
                    int LocationX = (axWindowsMediaPlayer1.ClientSize.Width/2) - (labelSize.X / 2)+ splitContainer1.Panel1.Width ;
                    fBBO.setLocationX(LocationX);
                }
                else
                {
                    fLT.setText("");
                    fBBO.setHide();
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (moviepath!="")
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
            Setting form = new Setting(moviepath,srtpath1);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
            if (moviepath != "")
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            checkSettingDiff();
            if (Setting.msg==true)
            {
                exListBox1.ItemHeight = Convert.ToInt32(json.SubtitlesHeight);
                exListBox1.Refresh();
                exListBox1.Items.Clear();
                Thread t1 = new Thread(addSrt);
                t1.Start();
            }
        }
        public void checkSettingDiff()
        {
            if (json.Display=="M")
            {
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel2Collapsed = false;
                fBBO.Show();
                fLT.Show();
            }
            else if (json.Display=="S")
            {
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = true;
                fBBO.setHide();
                fLT.Hide();
            }
            else
            {
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;
                fBBO.Show();
                fLT.Show();
            }
            fLT.setFontSize();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void exListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.exListBox1.IndexFromPoint(e.Location);
            int yy = e.Location.Y;
            int x = e.Location.X;
            int y = yy % exListBox1.ItemHeight;
            if (y >= exListBox1.ItemHeight - 35 && y <= exListBox1.ItemHeight - 10)
            {
                if (x >= 6 && x <= 31)
                {
                    Start.CW.g.tranText2(((exListBoxItem)exListBox1.Items[index]).srt1);
                    Start.CW.g.speak();
                }
                else if (x >= 34 && x <= 59)
                {
                    SpVoiceClass voice = new SpVoiceClass();
                    voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(1);//Item(0)男聲
                    voice.Speak(((exListBoxItem)exListBox1.Items[index]).srt1, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                }
                else if (x >= 60 && x <= 85)
                {
                    int startsec = 0;
                    string[] times = Regex.Split(((exListBoxItem)exListBox1.Items[index]).time, ">");
                    string[] timesplie = times[0].Split(':');
                    int l = 1;
                    for (int i = (timesplie.Length - 1); i >= 0; i--)
                    {
                        if (l == 1)
                        {
                            string[] sss = timesplie[i].Split(',');
                            int ss = Convert.ToInt32(sss[0]);
                            startsec = ss;
                        }
                        else
                        {
                            int ss = Convert.ToInt32(timesplie[i]) * (60 ^ l);
                            startsec = startsec + ss;
                        }
                        l++;
                    }
                    axWindowsMediaPlayer1.Ctlcontrols.currentPosition = startsec;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else if (x >= 131 && x <= 156)
                {
                    Detail form = new Detail(((exListBoxItem)exListBox1.Items[index]).srt1);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.Show();
                }
                //MessageBox.Show((e.Location.Y).ToString());
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                panel1.Width = this.Width - 4;
                panel1.Height = this.Height - 37;
                if (this.Height != 681)
                {
                    int LocationY = (axWindowsMediaPlayer1.ClientSize.Height) - (labelSize.Y) - 100;
                    fBBO.setLocationY(LocationY);
                    Application.DoEvents();
                }
                else
                {
                    int LocationY = 400;
                    fBBO.setLocationY(LocationY);
                    Application.DoEvents();
                }
            }
            catch { }
            
        }
    }
}
