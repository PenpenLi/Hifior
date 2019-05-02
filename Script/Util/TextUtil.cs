using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public class TextUtil
    {
        public static string[] SpliteString(char splitBy, string input)
        {
            return input.Split(splitBy);
        }
        public static int[] SpliteStringToIntArray(char splitBy, string input)
        {
            if (input == null || input == "")
                return null;
            string[] s = SpliteString(splitBy, input);
            return ArrayStringToInt(s);
        }

        public static int[] ArrayStringToInt(string[] input)
        {
            int[] ret = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                ret[i] = int.Parse(input[i]);
            }
            return ret;
        }
        public static string GetStringBeforeChar(char before, string input)
        {
            return input.Substring(0, input.IndexOf(before));
        }
        public static string GetResourcesFullPath(string fileName, params string[] folders)
        {
            string ret = "";
            foreach (string foldName in folders)
            {
                ret += foldName + "/";
            }
            ret += fileName;
            return ret;
        }
        public static string GetResourcesFullPath(string fileName, string folder)
        {
            string ret = "";
            ret += folder + "/";
            ret += fileName;
            return ret;
        }
        /// <summary>
        /// 返回List中的String用于Debug用
        /// </summary>
        /// <returns></returns>
        public static string GetListString<T>(List<T> InList)
        {
            return GetListString<T>(InList.ToArray());
        } /// <summary>
          /// 返回数组中的String用于Debug用
          /// </summary>
          /// <returns></returns>
        public static string GetListString<T>(T[] InList)
        {
            string r = string.Empty;
            foreach (object s in InList)
                r += (s.ToString() + "\n");
            return r;
        }
        public static string GetColorString(string Content, Color c)
        {
            int r = (int)(c.r * 255);
            int g = (int)(c.g * 255);
            int b = (int)(c.b * 255);
            int a = (int)(c.a * 255);
            return "<color=#" + r.ToString("x2") + g.ToString("x2") + b.ToString("x2") + a.ToString("x2") + ">" + Content + "</color>";
        }
        public static string GetBoldString(string Content)
        {
            return "<b>" + Content + "</b>";
        }
        public static string GetItalicString(string Content)
        {
            return "<i>" + Content + "</i>";
        }
        public static string GetItalicString(string Content, int Size)
        {
            return "<size=" + Size + ">" + Content + "</size>";
        }
        public static string GetStandardDataTime(System.DateTime dataTime)
        {
            return dataTime.ToString("yyyy-MM-dd hh:mm:ss");
        }
        public static string GetStandardDataTime()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
