using System;
using System.Collections.Generic;
using System.Text;

namespace abe.core
{
    public static class StringExt
    {
        public static string Name(this string path)
        {
            int i = path.LastIndexOf('/');
            return i < 0 ? path : path.Substring(i + 1);
        }

        public static int IndexOfWhitespace(this string s)
        {
            int length = s.Length;
            
            for (int i = 0; i < length; i++)
            {
                if (Char.IsWhiteSpace(s[i]))
                    return i;
            }

            return -1;
        }
    }
}
