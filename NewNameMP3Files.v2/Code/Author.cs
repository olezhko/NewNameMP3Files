using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewNameMP3Files.Code
{
    public class Author
    {
        public List<Album> albums = new List<Album>();
        public string _name;
        public Author(string name)
        {
            _name = name;
        }
    }
}
