using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewNameMP3Files.Code
{
    public class Album
    {
        public List<string> songs = new List<string>();
        public string _name;
        public Album(string name)
        {
            _name = name;
        }
    }
}
