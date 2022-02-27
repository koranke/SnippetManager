using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetManager
{
    internal class Categories
    {
        public List<Category> CategoryList { get; set; } = new List<Category>();

        public void loadCategoriesFromFile(String fileName)
        {
            var lines = System.IO.File.ReadLines(fileName);
            Category currentCategory = new Category();
            Snippet currentSnippet = new Snippet();
            bool readingSnippet = false;


            foreach (var line in lines)
            {
                if (line.StartsWith("##CAT##"))
                {
                    currentCategory = new Category();
                    CategoryList.Add(currentCategory);
                    currentCategory.Name = line.Substring(7);

                }
                else if (line.StartsWith("##SNIP##"))
                {
                    currentSnippet = new Snippet();
                    currentCategory.Snippets.Add(currentSnippet);
                    currentSnippet.Description = line.Substring(8);
                    readingSnippet = true;
                }
                else if (line.StartsWith("##SNIPEND##"))
                {
                    readingSnippet = false;
                }
                else if (readingSnippet)
                {
                    currentSnippet.Content += line;
                }
            }
        }
    }
}
