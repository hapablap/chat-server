using System;

namespace ChatServer.Extension
{
    public static class StringExtension
    {
        public static int WordCount(this string content)
        {
            return content.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static bool IsOneWord(this string content)
        {
            return content.WordCount() == 1;
        }
    }
}
