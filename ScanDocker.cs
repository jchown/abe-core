using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace abe.core
{
    public class ScanDocker : IScanner
    {
        bool IScanner.CanScanFile(string filename)
        {
            return filename.Name() == "Dockerfile";
        }

        private static readonly Regex COPY = new Regex(@"COPY\s+.*\s+\S+");

        FileNameReferences IScanner.ScanFile(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var refs = new FileNameReferences();

            for (int i = 0; i < lines.Length; i++)
            {
                var references = ScanLine(lines[i]);
                if (references != null)
                {
                    foreach (var reference in references)
                       refs.Add(i, reference);
                }
            }

            return refs;
        }

        List<string> ScanLine(string line)
        {
            if (line.Length == 0)
                return null;

            if (!line.StartsWith("COPY"))
                return null;

            string copyArgs = line.Substring(4).Trim();
            if (copyArgs.StartsWith("--chown"))
                copyArgs = copyArgs.Substring(copyArgs.IndexOfWhitespace()).Trim();

            bool quote = false;
            var current = new StringBuilder(copyArgs.Length);
            var preWhitespace = new List<string>();

            int i = 0;
            for (; i < copyArgs.Length; i++)
            {
                char c = copyArgs[i];
                if (char.IsWhiteSpace(c) && !quote)
                    break;

                if (c == '"')
                {
                    quote = !quote;
                }
                else if (c == ',' && !quote)
                {
                    preWhitespace.Add(Unescape(current.ToString()));
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0)
                preWhitespace.Add(Unescape(current.ToString()));

            return preWhitespace;
        }

        public static string Unescape(string s)
        {
            throw new NotImplementedException();
        }
    }
}
