using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrtMovie
{
    public class staticvalue
    {
        public static sqliteProtocol sqlite = new sqliteProtocol();
    }
    public class Srt
    {
        public int id = 0;
        public string time = "";
        public string starttime = "";
        public string endtime = "";
        public string srt1line1 = "";
        public string srt1line2 = "";
        public string srt1 = "";
        public string srt2line1 = "";
        public string srt2line2 = "";
        public string srt2 = "";
        public double startsec = 0;  //*100
        public double endsec = 0;  //*100
    }
    public class WordDefine
    {
        public int id = 0;
        public string a = "";
        public string define = "";
        public string sentence = "";
    }
    public class WordSynonym
    {
        public int id = 0;
        public string a = "";
        public string define = "";
    }
    public class Word
    {
        public int id = 0;
        public string a = "";
        public string define = "";
        public string otherword = "";
    }
    public class SettingConfig
    {
        public string Display = "";//MS,M,S
        public string SubtitlesTimeFontSize = "";
        public string SubtitlesTimeFontColor = "";
        public string SubtitlesSrt1FontSize = "";
        public string SubtitlesSrt1FontColor = "";
        public string SubtitlesSrt2FontSize = "";
        public string SubtitlesSrt2FontColor = "";
        public string SubtitlesHeight;
        public string MovieSrt1FontSize = "";
        public string Subtitles = "";//S1S2,S1,S2,N
    }
}
