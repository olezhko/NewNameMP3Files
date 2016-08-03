using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewNameMP3Files.Code
{
    public class Album
    {
        public List<string> Songs = new List<string>();
        public string Name;
        public Album(string name)
        {
            Name = name;
        }
    }
}
