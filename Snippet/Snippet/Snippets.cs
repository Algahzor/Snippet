using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Snippet
{
    class Snippets
    {
        public Dictionary<string, string> snippets;
        readonly string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Snippets";
        readonly string indexPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Snippets\\_index.txt";

        public Snippets()
        {
            snippets = new Dictionary<string, string>();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(indexPath))
            {
                return;
            }

            RefreshSnippets();
            SetSnippets();
        }

        private void RefreshSnippets()
        {
            snippets = new Dictionary<string, string>();
            using (var sr = new StreamReader(indexPath))
            {
                while (!sr.EndOfStream)
                {
                    snippets.Add(sr.ReadLine() ?? "null", "");
                }
            }
        }

        private void SetSnippets()
        {
            var snipkeys = snippets;
            foreach (var snippet in snipkeys.Keys.ToList())
            {
                if (!File.Exists(directoryPath + "\\" + snippet + ".txt")) continue;
                using (var sr = new StreamReader(directoryPath + "\\" + snippet + ".txt"))
                {
                    snippets[snippet] = sr.ReadToEnd();
                }
            }
        }

        public void AddSnippet(KeyValuePair<string, string> snip)
        {
            if (snippets.ContainsKey(snip.Key)) return;
            snippets.Add(snip.Key, snip.Value);

            using (var sw = new StreamWriter(indexPath, true))
            {
                sw.WriteLine(snip.Key);
            }

            using (var sw = new StreamWriter(directoryPath + "\\" + snip.Key + ".txt"))
            {
                sw.Write(snip.Value);
            }
        }

        public void DeleteFile(string key)
        {
            File.Delete(directoryPath + "\\" + key + ".txt");
            snippets.Remove(key);
            using (var sw = new StreamWriter(indexPath))
            {
                foreach (var snip in snippets)
                {
                    sw.WriteLine(snip.Key);
                }
            }
            RefreshSnippets();
            SetSnippets();
        }
    }
}
