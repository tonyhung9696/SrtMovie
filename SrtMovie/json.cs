using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SrtMovie
{
    public class json
    {
        public static string Display = "";//MS,M,S
        public static string SubtitlesTimeFontSize = "";
        public static string SubtitlesTimeFontColor = "";
        public static string SubtitlesSrt1FontSize = "";
        public static string SubtitlesSrt1FontColor = "";
        public static string SubtitlesSrt2FontSize = "";
        public static string SubtitlesSrt2FontColor = "";
        public static string MovieSrt1FontSize = "";
        public static string SubtitlesHeight = "";
        public static string Subtitles = "";//S1S2,S1,S2,N
        public static int msspeech = 0;
        public static void setup_json()
        {
            string startup_path = Application.StartupPath;
            string file = startup_path + "\\setting.json";
            if (!File.Exists(file))
            {
                dynamic setting = new JObject();
                setting.Display = "MS";
                setting.SubtitlesBackColor = "15";
                setting.SubtitlesTimeFontSize = "15";
                setting.SubtitlesTimeFontColor = "#FFFFFF";
                setting.SubtitlesSrt1FontSize = "15";
                setting.SubtitlesSrt1FontColor = "#FFFFFF";
                setting.SubtitlesSrt2FontSize = "15";
                setting.SubtitlesSrt2FontColor = "#FFFFFF";
                setting.SubtitlesHeight = "#87CEEB";
                setting.MovieSrt1FontSize = "15";
                setting.Subtitles = "S1S2";
                setting.msspeech = 0;
                string convertString2 = Convert.ToString(setting);
                File.WriteAllText(file, convertString2);
            }
        }
        public static void setjsonsetting(string key, string value)
        {
            string startup_path = Application.StartupPath;
            string file = startup_path + "\\setting.json";
            string str = File.ReadAllText(file);
            JObject jobject = JObject.Parse(str);
            jobject[key] = value;
            string convertString2 = Convert.ToString(jobject);
            File.WriteAllText(file, convertString2);
        }
        public static void getsetting()
        {
            try
            {
                string startup_path = Application.StartupPath;
                string file = startup_path + "\\setting.json";
                string str = File.ReadAllText(file);
                JObject jobject = JObject.Parse(str);
                Display = jobject["Display"].ToString();
                SubtitlesTimeFontSize = jobject["SubtitlesTimeFontSize"].ToString();
                SubtitlesTimeFontColor = jobject["SubtitlesTimeFontColor"].ToString();
                SubtitlesSrt1FontSize = jobject["SubtitlesSrt1FontSize"].ToString();
                SubtitlesSrt1FontColor = jobject["SubtitlesSrt1FontColor"].ToString();
                SubtitlesSrt2FontSize = jobject["SubtitlesSrt2FontSize"].ToString();
                SubtitlesSrt2FontColor = jobject["SubtitlesSrt2FontColor"].ToString();
                MovieSrt1FontSize = jobject["MovieSrt1FontSize"].ToString();
                Subtitles = jobject["Subtitles"].ToString();
                SubtitlesHeight = jobject["SubtitlesHeight"].ToString();
                msspeech = Convert.ToInt32(jobject["msspeech"].ToString());


            }
            catch (Exception ex)
            {
                MessageBox.Show("刪除program位置setting.json,重新setting" + ex);
            }

        }
    }
}
