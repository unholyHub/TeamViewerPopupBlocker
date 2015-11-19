using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace TeamViewerPopupBlocker.Classes
{
    public static class FileOperations
    {
        public static void AddTextToFile(string filePath, string content)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                writer.WriteLine(content);
            }
        }

        public static void WriteWindowNamesToFile(string filePath, Collection<string> collection)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                if (collection == null)
                {
                    return;
                }

                foreach (string s in collection)
                {
                    writer.WriteLine(s);
                }
            }
        }

        public static Collection<string> ReadTextFromFile(string filePath)
        {
            string line = String.Empty;
            Collection<string> Collection = new Collection<string>();

            if (!File.Exists(filePath))
            {
                return Collection;
            }

            using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    Collection.Add(line);
                }
            }

            return Collection;
        }

        public static void AddTextToFile(string filePath, Collection<string> collection)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                if (collection == null)
                {
                    return;
                }

                foreach (string s in collection)
                {
                    writer.WriteLine(s);
                }
            }
        }
    }
}
