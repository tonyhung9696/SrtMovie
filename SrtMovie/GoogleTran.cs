﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSScriptControl;
namespace SrtMovie
{
    public class GoogleTrans
    {
        public static string GoogleTranslate(string text, string fromLanguage, string toLanguage)
        {
            try
            {
                CookieContainer cc = new CookieContainer();
                string GoogleTransBaseUrl = "https://translate.google.com/";
                var BaseResultHtml = GetResultHtml(GoogleTransBaseUrl, cc, "");
                Regex re = new Regex(@"(?<=tkk:')(.*?)(?=')");
                var TKK = re.Match(BaseResultHtml).ToString();
                string jspath = Application.StartupPath + "\\gettk.js";
                var GetTkkJS = System.IO.File.ReadAllText(jspath);
                var tk = ExecuteScript("tk(\"" + text + "\",\"" + TKK + "\")", GetTkkJS);
                string googleTransUrl = "https://translate.google.com/translate_a/single?client=t&sl=" + fromLanguage + "&tl=" + toLanguage + "&hl=en&dt=at&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&ie=UTF-8&oe=UTF-8&otf=1&ssel=0&tsel=0&kc=1&tk=" + tk + "&q=" + HttpUtility.UrlEncode(text);
                var ResultHtml = GetResultHtml(googleTransUrl, cc, "https://translate.google.com/");
                dynamic TempResult = Newtonsoft.Json.JsonConvert.DeserializeObject(ResultHtml);
                string ResultText = Convert.ToString(TempResult[0][0][0]);
                return ResultText;
            }
            catch
            {
                return "";
            }
        }
        public static string ExecuteScript(string sExpression, string sCode)
        {
            MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            scriptControl.UseSafeSubset = true;
            scriptControl.Language = "JScript";
            scriptControl.AddCode(sCode);
            try
            {
                string str = scriptControl.Eval(sExpression).ToString();
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }
        public static string GetResultHtml(string url, CookieContainer cookie, string refer)
        {
            try
            {
                var html = "";
                var webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.Method = "GET";
                webRequest.CookieContainer = cookie;
                webRequest.Referer = refer;
                webRequest.Timeout = 20000;
                webRequest.Headers.Add("X-Requested-With:XMLHttpRequest");
                webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                webRequest.UserAgent = " Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";
                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        html = reader.ReadToEnd();
                        reader.Close();
                        webResponse.Close();
                    }
                }
                return html;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
