using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetManager
{
    class Snippets
    {
        public ObservableCollection<Snippet> SnippetList { get; set; } = new ObservableCollection<Snippet>();
        public ObservableCollection<String> Categories
        {
            get { return new ObservableCollection<String>(SnippetList.Select(o => o.Category).ToList().Distinct()); }
        }

        public void saveToFile(String fileName)
        {
            List<Snippet> sortedSnippets = SnippetList.OrderBy(x => x.Category).ThenBy(y => y.Description).ToList();
            try
            {
                StreamWriter sw = new StreamWriter(fileName);

                String currentCategory = "";
                foreach (Snippet snippet in sortedSnippets)
                {
                    if (!currentCategory.Equals(snippet.Category)) 
                    { 
                        currentCategory = snippet.Category;
                        sw.WriteLine("##CAT##" + snippet.Category);
                        sw.WriteLine("##SNIP##" + snippet.Description);
                        sw.WriteLine(snippet.Content);
                        sw.WriteLine("##SNIPEND##");
                        sw.WriteLine("");
                    }
                    else
                    {
                        sw.WriteLine("##SNIP##" + snippet.Description);
                        sw.WriteLine(snippet.Content);
                        sw.WriteLine("##SNIPEND##");
                        sw.WriteLine("");
                    }
                }
                sw.Close();
            }
            catch (Exception e)
            {
                    throw new Exception(e.Message);
            }
        }

        public void loadFromFile(String fileName)
        {
            var lines = System.IO.File.ReadLines(fileName);
            string currentCategory = "";
            Snippet currentSnippet = new Snippet();
            bool readingSnippet = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("##CAT##"))
                {
                    currentCategory = line.Substring(7);

                }
                else if (line.StartsWith("##SNIP##"))
                {
                    currentSnippet = new Snippet();
                    SnippetList.Add(currentSnippet);
                    currentSnippet.Category = currentCategory;
                    currentSnippet.Description = line.Substring(8);
                    readingSnippet = true;
                }
                else if (line.StartsWith("##SNIPEND##"))
                {
                    currentSnippet.Content = currentSnippet.Content.TrimEnd('\r', '\n');
                    readingSnippet = false;
                }
                else if (readingSnippet)
                {
                    currentSnippet.Content += line + Environment.NewLine;
                }
            }
        }

        public SortedSet<string> getCategories()
        {
            SortedSet<string> categories = new SortedSet<string>();
            foreach (Snippet snippet in SnippetList)
            {
                categories.Add(snippet.Category);
            }

            return categories;
        }

    }
}
