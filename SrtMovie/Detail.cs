using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Threading;

namespace SrtMovie
{
    public partial class Detail : Form
    {
        List<string> Englishs = new List<string>();
        int CurrentPostion = 0;
        bool drag;
        int mousex;
        int mousey;
        string checkword = "";
        List<WordDefine> WordDefines = new List<WordDefine>();
        List<WordSynonym> WordSynonyms = new List<WordSynonym>();
        List<Word> Words = new List<Word>();
        List<string> sentences = new List<string>();
        public Detail(string s)
        {
            InitializeComponent();
            Englishs.Add(s);
            textBox1.Text = s;
            checkword = s;
            string translation = "";
            try
            {
                translation = GoogleTrans.GoogleTranslate(s, "en", "zh-TW");
            }
            catch (Exception ex)
            {
                translation = GoogleTrans.GoogleTranslate(s, "en", "zh-TW");
            }
            textBox7.Text = translation;
            backgroundWorker1.RunWorkerAsync();
        }

        private void Detail_Load(object sender, EventArgs e)
        {

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

        private void button5_Click(object sender, EventArgs e)
        {
            if (Englishs.Count()-1>CurrentPostion)
            {
                clearListBox();
                CurrentPostion = CurrentPostion + 1;
                checkword = Englishs[CurrentPostion];
                label1.Text = checkword;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                
            }
            else
            {
                MessageBox.Show("NO");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CurrentPostion!=0)
            {
                clearListBox();
                CurrentPostion = CurrentPostion - 1;
                checkword = Englishs[CurrentPostion];
                label1.Text = checkword;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                
            }
            else
            {
                MessageBox.Show("NO");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] texts = checkword.Split(' ');
            int countspace = texts.Length;
            Start.CW.g.tranText2(checkword);
            Thread.Sleep(150);
            Start.CW.g.ShowAllList();
            if (countspace <= 3)
            {
                staticvalue.sqlite.countcmd(String.Format("select count(*) from [Translation] where [Word]='{0}' COLLATE NOCASE", checkword.Replace("'", "''")));
                int countDB = staticvalue.sqlite.getcount();
                if (countDB<=0)
                {
                    staticvalue.sqlite.cmds.Clear();
                    WordDefines = Start.CW.g.readDefine();
                    WordSynonyms = Start.CW.g.readsynonym();
                    Words = Start.CW.g.readWord();
                    sentences = Start.CW.g.readsentence();
                    foreach (WordDefine WD in WordDefines)
                    {
                        string cmd = String.Format("insert into [Define] ([Word],[PartOfSpeech],[Define],[example]) values ('{0}','{1}','{2}','{3}')", checkword.Replace("'","''"),WD.a.Replace("'", "''"), WD.define.Replace("'", "''"), WD.sentence.Replace("'", "''"));
                        staticvalue.sqlite.cmds.Add(cmd);
                    }
                    foreach (WordSynonym WD in WordSynonyms)
                    {
                        string cmd = String.Format("insert into [Synonym] ([Word],[PartOfSpeech],[example]) values ('{0}','{1}','{2}')", checkword.Replace("'", "''"), WD.a.Replace("'", "''"), WD.define.Replace("'", "''"));
                        staticvalue.sqlite.cmds.Add(cmd);
                    }
                    foreach (Word word in Words)
                    {
                        string cmd = String.Format("insert into [Translation] ([Word],[PartOfSpeech],[ChineseMean],[EnglishMean]) values ('{0}','{1}','{2}','{3}')", checkword.Replace("'", "''"), word.a.Replace("'", "''"), word.define.Replace("'", "''"), word.otherword.Replace("'", "''"));
                        staticvalue.sqlite.cmds.Add(cmd);
                    }
                    foreach (string s in sentences)
                    {
                        string cmd = String.Format("insert into [Sentence] ([Word],[example]) values ('{0}','{1}')", checkword.Replace("'", "''"), s.Replace("'", "''"));
                        staticvalue.sqlite.cmds.Add(cmd);
                    }
                    staticvalue.sqlite.batchcmd();
                }
                else
                {
                    WordDefines = getWordDefines();
                    WordSynonyms = getWordSynonyms();
                    Words = getWords();
                    sentences = getsentences();
                }
                WriteDataToForm();
            }
        }
        public void WriteDataToForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(WriteDataToForm));
            }
            else
            {
                listView1.Items.Clear();
                aListBox1.Items.Clear();
                bListBox1.Items.Clear();
                cListBox1.Items.Clear();
                var WordDefineList = WordDefines.OrderBy(o => o.a);
                var WordSynonymList = WordSynonyms.OrderBy(o => o.a);
                foreach (WordDefine WD in WordDefineList)
                {
                    aListBox1.Items.Add(new AListBoxItem(1, WD.a, WD.define, WD.sentence));
                }
                string WordSynonymPartOfSpeech = "";
                foreach (WordSynonym WD in WordSynonymList)
                {
                    if (WordSynonymPartOfSpeech != WD.a)
                    {
                        WordSynonymPartOfSpeech = WD.a;
                        listView1.Items.Add(WD.a);
                    }
                    else
                    {
                        listView1.Items.Add(WD.define);
                    }
                }
                foreach (Word word in Words)
                {
                    cListBox1.Items.Add(new CListBoxItem(1, word.a, word.define, word.otherword));
                }
                foreach (string s in sentences)
                {
                    bListBox1.Items.Add(new BListBoxItem(1, s));
                }
            }
        }
        public List<WordDefine> getWordDefines()
        {
            List<WordDefine> result = new List<WordDefine>();
            string cmd = String.Format("select count(*) from [Define] where [Word=]'{0}'",checkword);
            staticvalue.sqlite.countcmd(cmd);
            int count = staticvalue.sqlite.getcount();
            if (count>0)
            {
                cmd = String.Format("select * from [Define] where [Word]='{0}'", checkword);
                staticvalue.sqlite.selectcmd(cmd);
                SQLiteDataReader reader = staticvalue.sqlite.getreader();
                while (reader.Read())
                {
                    WordDefine WD = new WordDefine();
                    WD.a = reader["PartOfSpeech"].ToString();
                    WD.define = reader["Define"].ToString();
                    WD.sentence = reader["example"].ToString();
                    result.Add(WD);
                }
            }
            return result;
        }
        public List<WordSynonym> getWordSynonyms()
        {
            List<WordSynonym> result = new List<WordSynonym>();
            string cmd = String.Format("select count(*) from [Synonym] where [Word]='{0}'", checkword);
            staticvalue.sqlite.countcmd(cmd);
            int count = staticvalue.sqlite.getcount();
            if (count > 0)
            {
                cmd = String.Format("select * from [Synonym] where [Word]='{0}'", checkword);
                staticvalue.sqlite.selectcmd(cmd);
                SQLiteDataReader reader = staticvalue.sqlite.getreader();
                while (reader.Read())
                {
                    WordSynonym WS = new WordSynonym();
                    WS.a = reader["PartOfSpeech"].ToString();
                    WS.define = reader["example"].ToString();
                    result.Add(WS);
                }
            }
            return result;
        }
        public List<Word> getWords()
        {
            List<Word> result = new List<Word>();
            string cmd = String.Format("select * from [Translation] where [Word]='{0}'", checkword);
            staticvalue.sqlite.selectcmd(cmd);
            SQLiteDataReader reader = staticvalue.sqlite.getreader();
            while (reader.Read())
            {
                Word word = new Word();
                word.a = reader["PartOfSpeech"].ToString();
                word.define = reader["ChineseMean"].ToString();
                word.otherword= reader["EnglishMean"].ToString();
                result.Add(word);
            }
            return result;
        }
        public List<string> getsentences()
        {
            List<string> result = new List<string>();
            string cmd = String.Format("select count(*) from [Sentence] where [Word]='{0}'", checkword);
            staticvalue.sqlite.countcmd(cmd);
            int count = staticvalue.sqlite.getcount();
            if (count > 0)
            {
                cmd = String.Format("select * from [Sentence] where [Word]='{0}'", checkword);
                staticvalue.sqlite.selectcmd(cmd);
                SQLiteDataReader reader = staticvalue.sqlite.getreader();
                while (reader.Read())
                {
                    result.Add(reader["example"].ToString());
                }
            }
            return result;
        }
        public void clearListBox()
        {
            aListBox1.DataSource = null;
            bListBox1.DataSource = null;
            cListBox1.DataSource = null;
            listView1.Items.Clear();
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectword = "";
            int selectlen = 0;
            foreach (Control c in panel2.Controls)
            {
                if (c is TextBox && selectlen<=0)
                {
                    TextBox t = (TextBox)c;
                    selectlen= t.SelectionLength;
                    if (selectlen > 0)
                    {
                        selectword = t.Text.Substring(t.SelectionStart, t.SelectionLength).Replace(".","").Replace(",","").Trim();
                        Englishs.Add(selectword);
                        checkword = selectword;
                        label1.Text = selectword;
                        CurrentPostion = Englishs.Count - 1;
                    }
                }
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string selectword = "";
            int selectlen = textBox1.SelectionLength;
            if (selectlen>0)
            {
                selectword = textBox1.Text.Substring(textBox1.SelectionStart, textBox1.SelectionLength).Replace(".", "").Replace(",", "").Trim();
                Englishs.Add(selectword);
                checkword = selectword;
                label1.Text = selectword;
                CurrentPostion = Englishs.Count - 1;
            }
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch(Exception ex)
            {

            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectword = "";
            int selectlen = 0;
            foreach (Control c in panel3.Controls)
            {
                if (c is TextBox && selectlen <= 0)
                {
                    TextBox t = (TextBox)c;
                    selectlen = t.SelectionLength;
                    if (selectlen > 0)
                    {
                        selectword = t.Text.Substring(t.SelectionStart, t.SelectionLength).Replace(".", "").Replace(",", "").Trim();
                        Englishs.Add(selectword);
                        checkword = selectword;
                        label1.Text = selectword;
                        CurrentPostion = Englishs.Count - 1;
                    }
                }
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selectword = "";
            int selectlen = 0;
            foreach (Control c in panel7.Controls)
            {
                if (c is TextBox && selectlen <= 0)
                {
                    TextBox t = (TextBox)c;
                    selectlen = t.SelectionLength;
                    if (selectlen > 0)
                    {
                        selectword = t.Text.Substring(t.SelectionStart, t.SelectionLength).Replace(".", "").Replace(",", "").Trim();
                        Englishs.Add(selectword);
                        checkword = selectword;
                        label1.Text = selectword;
                        CurrentPostion = Englishs.Count - 1;
                    }
                }
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selectword = "";
            int selectlen = 0;
            foreach (Control c in panel9.Controls)
            {
                if (c is TextBox && selectlen <= 0)
                {
                    TextBox t = (TextBox)c;
                    selectlen = t.SelectionLength;
                    if (selectlen > 0)
                    {
                        selectword = t.Text.Substring(t.SelectionStart, t.SelectionLength).Replace(".", "").Replace(",", "").Trim();
                        Englishs.Add(selectword);
                        checkword = selectword;
                        label1.Text = selectword;
                        CurrentPostion = Englishs.Count - 1;
                    }
                }
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void aListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentP = aListBox1.SelectedIndex;
            string define = ((AListBoxItem)aListBox1.Items[currentP]).define;
            string sentence=((AListBoxItem)aListBox1.Items[currentP]).sentence;
            textBox2.Text = define;
            textBox3.Text = sentence;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string defineTran = GoogleTrans.GoogleTranslate(textBox2.Text, "en", "zh-TW");
            label4.Text = defineTran;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string sentenceTran = GoogleTrans.GoogleTranslate(textBox3.Text, "en", "zh-TW");
            label5.Text = sentenceTran;
        }

        private void bListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentP = bListBox1.SelectedIndex;
            string example = ((BListBoxItem)bListBox1.Items[currentP]).sentence;
            textBox5.Text = example;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox4.Text = listView1.SelectedItems[0].Text;
            }
            catch { }
        }

        private void cListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentP = cListBox1.SelectedIndex;
            string example = ((CListBoxItem)cListBox1.Items[currentP]).c;
            textBox6.Text = example;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string example = GoogleTrans.GoogleTranslate(textBox5.Text, "en", "zh-TW");
            label6.Text = example;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string example = GoogleTrans.GoogleTranslate(textBox4.Text, "en", "zh-TW");
            label7.Text = example;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string example = GoogleTrans.GoogleTranslate(textBox6.Text, "en", "zh-TW");
            label8.Text = example;
        }
    }
}
