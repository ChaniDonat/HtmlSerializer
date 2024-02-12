using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal class HtmlAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format(" {0}=\"{1}\"", Name, Value);
        }
    }
}
