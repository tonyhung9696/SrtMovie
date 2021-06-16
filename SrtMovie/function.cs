using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrtMovie
{
    public class function
    {
        public static string findPastOfSpeech(string all, string text, List<string> speechs)
        {
            string result = speechs[0];
            int bigId = 0;
            foreach (string speech in speechs)
            {
                int speechId = all.IndexOf(speech);
                int textId = all.IndexOf(text);
                if (speechId >= bigId && textId > speechId)
                {
                    bigId = speechId;
                    result = speech;
                }
            }
            return result;

        }
        public static bool fileExist(string path)
        {
            bool result = false;
            if (path=="")
            {
                result = true;
            }
            else if (File.Exists(path))
            {
                result = true;
            }
            return result; 
        }
    }
}
