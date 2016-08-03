using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewNameMP3Files.Code
{
    public class Author
    {
        public List<Album> Albums = new List<Album>();
        public string Name;
        public Author(string name)
        {
            Name = name;
        }
    }
}
