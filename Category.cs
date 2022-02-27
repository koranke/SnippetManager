using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetManager
{
    internal class Category
    {
        public string Name { get; set; }
        public List<Snippet> Snippets { get; set; } = new List<Snippet>();
    }
}
