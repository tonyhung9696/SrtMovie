using SpeechLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SrtMovie
{
    public partial class Setting : Form
    {
        bool drag;
        int mousex;
        int mousey;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        string moviepath = "";
        string srtpath = "";
        public SettingConfig OrgSC = new SettingConfig();
        public SettingConfig NewSC = new SettingConfig();
        public static bool msg = false;
        public Setting(string m,string s)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            moviepath = m;
            srtpath = s;
        }
        public Setting()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
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
        private void Setting_Load(object sender, EventArgs e)
        {
            msg = false;
            startLoad();
            OrgSC.Display = json.Display;
            OrgSC.SubtitlesHeight = json.SubtitlesHeight;
            OrgSC.MovieSrt1FontSize = json.MovieSrt1FontSize;
            OrgSC.Subtitles = json.Subtitles;
            OrgSC.SubtitlesSrt1FontColor = json.SubtitlesSrt1FontColor;
            OrgSC.SubtitlesSrt1FontSize = json.SubtitlesSrt1FontSize;
            OrgSC.SubtitlesSrt2FontColor = json.SubtitlesSrt2FontColor;
            OrgSC.SubtitlesSrt2FontSize = json.SubtitlesSrt2FontSize;
            OrgSC.SubtitlesTimeFontColor = json.SubtitlesTimeFontColor;
            OrgSC.SubtitlesTimeFontSize = json.SubtitlesTimeFontSize;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            this.Close();
            if (radioButton2.Checked==true)
            {
                json.Display = "M";
                json.setjsonsetting("Display", "M");
                NewSC.Display = "M";
            }
            else if (radioButton3.Checked == true)
            {
                json.Display = "S";
                json.setjsonsetting("Display", "S");
                NewSC.Display = "S";
            }
            else
            {
                json.Display = "MS";
                json.setjsonsetting("Display", "MS");
                NewSC.Display = "MS";
            }
            if (radioButton4.Checked == true)
            {
                json.Subtitles = "S1S2";
                json.setjsonsetting("Subtitles", "S1S2");
                NewSC.Subtitles = "S1S2";
            }
            else if (radioButton5.Checked == true)
            {
                json.Subtitles = "S1";
                json.setjsonsetting("Subtitles", "S1");
                NewSC.Subtitles = "S1";
            }
            else if (radioButton6.Checked == true)
            {
                json.Subtitles = "S2";
                json.setjsonsetting("Subtitles", "S2");
                NewSC.Subtitles = "S2";
            }
            else
            {
                json.Subtitles = "N";
                json.setjsonsetting("Subtitles", "N");
                NewSC.Subtitles = "N";
            }
            json.SubtitlesTimeFontSize = textBox6.Text;
            json.setjsonsetting("SubtitlesTimeFontSize", textBox6.Text);
            json.SubtitlesTimeFontColor = textBox5.Text;
            json.setjsonsetting("SubtitlesTimeFontColor", textBox5.Text);
            json.SubtitlesSrt1FontSize = textBox1.Text;
            json.setjsonsetting("SubtitlesSrt1FontSize", textBox1.Text);
            json.SubtitlesSrt1FontColor = textBox2.Text;
            json.setjsonsetting("SubtitlesSrt1FontColor", textBox2.Text);
            json.SubtitlesSrt2FontSize = textBox4.Text;
            json.setjsonsetting("SubtitlesSrt2FontSize", textBox4.Text);
            json.SubtitlesSrt2FontColor = textBox3.Text;
            json.setjsonsetting("SubtitlesSrt2FontColor", textBox3.Text);
            json.msspeech = comboBox1.SelectedIndex;
            json.setjsonsetting("msspeech", comboBox1.SelectedIndex.ToString());
            if (Convert.ToInt32(textBox8.Text)<=255)
            {
                json.SubtitlesHeight = textBox8.Text;
                json.setjsonsetting("SubtitlesHeight", textBox8.Text);
            }
            else
            {
                json.SubtitlesHeight = "255";
                json.setjsonsetting("SubtitlesHeight", "255");
            }
            NewSC.SubtitlesTimeFontSize = textBox6.Text;
            NewSC.SubtitlesTimeFontColor = textBox5.Text;
            NewSC.SubtitlesSrt1FontSize = textBox1.Text;
            NewSC.SubtitlesSrt1FontColor = textBox2.Text;
            NewSC.SubtitlesSrt2FontSize = textBox4.Text;
            NewSC.SubtitlesSrt2FontColor = textBox3.Text;
            NewSC.SubtitlesHeight = textBox8.Text;

            if (OrgSC.SubtitlesHeight != NewSC.SubtitlesHeight)
            {
                msg = true;
            }
            if (OrgSC.Subtitles != NewSC.Subtitles || OrgSC.SubtitlesSrt1FontColor != NewSC.SubtitlesSrt1FontColor)
            {
                msg = true;
            }
            else if (OrgSC.SubtitlesSrt1FontSize != NewSC.SubtitlesSrt1FontSize || OrgSC.SubtitlesSrt1FontColor != NewSC.SubtitlesSrt1FontColor)
            {
                msg = true;
            }
            else if (OrgSC.SubtitlesSrt2FontSize != NewSC.SubtitlesSrt2FontSize || OrgSC.SubtitlesSrt2FontColor != NewSC.SubtitlesSrt2FontColor)
            {
                msg = true;
            }
            else if (OrgSC.SubtitlesTimeFontSize != NewSC.SubtitlesTimeFontSize || OrgSC.SubtitlesTimeFontColor != NewSC.SubtitlesTimeFontColor)
            {
                msg = true;
            }

        }
        public void startLoad()
        {
            json.getsetting();
            SpVoice spVoice = new SpVoice();
            for (int i = 0; i < spVoice.GetVoices().Count; i++) 
            {
                var t = spVoice.GetVoices().Item(i);
                string desc = spVoice.GetVoices().Item(i).GetDescription(); 
                comboBox1.Items.Add(desc);
            }
            if ((spVoice.GetVoices().Count - 1) >= json.msspeech)
            {
                comboBox1.SelectedIndex = json.msspeech;
            }
            else
            {
                comboBox1.SelectedIndex=0;
            }
            if (json.Display=="M")
            {
                radioButton2.Checked = true;
            }
            else if (json.Display=="S")
            {
                radioButton3.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }

            textBox6.Text = json.SubtitlesTimeFontSize;
            textBox5.Text = json.SubtitlesTimeFontColor;
            textBox1.Text = json.SubtitlesSrt1FontSize;
            textBox2.Text = json.SubtitlesSrt1FontColor;
            textBox4.Text = json.SubtitlesSrt2FontSize;
            textBox3.Text = json.SubtitlesSrt2FontColor;
            textBox8.Text = json.SubtitlesHeight;
            if (json.Subtitles=="S1S2")
            {
                radioButton4.Checked = true;
            }
            else if (json.Subtitles == "S1")
            {
                radioButton5.Checked = true;
            }
            else if (json.Subtitles == "S2")
            {
                radioButton6.Checked = true;
            }
            else
            {
                radioButton7.Checked = true;
            }
            //System.Drawing.ColorTranslator.FromHtml("#FFCC66");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseColor CC = new ChooseColor(textBox5.Text);
            CC.StartPosition = FormStartPosition.CenterScreen;
            CC.ShowDialog();
            if (CC.hexcolor!="")
            {
                textBox5.Text = CC.hexcolor;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseColor CC = new ChooseColor(textBox2.Text);
            CC.StartPosition = FormStartPosition.CenterScreen;
            CC.ShowDialog();
            if (CC.hexcolor != "")
            {
                textBox2.Text = CC.hexcolor;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChooseColor CC = new ChooseColor(textBox3.Text);
            CC.StartPosition = FormStartPosition.CenterScreen;
            CC.ShowDialog();
            if (CC.hexcolor != "")
            {
                textBox3.Text = CC.hexcolor;
            }

        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            SpVoice spVoice = new SpVoice();
            spVoice.Voice = spVoice.GetVoices().Item(comboBox1.SelectedIndex);
            spVoice.Speak(textBox7.Text, SpeechVoiceSpeakFlags.SVSFDefault);
        }
    }
}
