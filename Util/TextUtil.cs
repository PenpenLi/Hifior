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
    }
}
