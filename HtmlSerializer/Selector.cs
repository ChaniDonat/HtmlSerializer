using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new List<string>();
        }
        public  Selector convertQueryToSelector(string Query)
        {
            string []Querys= Query.Split(' ');
            Selector root= new Selector();
            Selector current = root;
            foreach (string Q in Querys)
            {
                string tmp = Q;
                 string[] subQ = new string[Q.Length];
                int index = 0;
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i] == '#' || tmp[i] == '.' )
                    {
                        subQ[index++] = tmp.Substring(0, i) ;
                        tmp = tmp.Substring(i);
                        i = 0;
                    }
                }
                subQ[index++] = tmp;
                for (int i = 0; i < index; i++)
                {
                    if (subQ[i].StartsWith('#'))
                    {
                        current.Id = subQ[i].Substring(1);
                    }
                    else if (subQ[i].StartsWith('.'))
                    {
                        current.Classes.Add(subQ[i].Substring(1));
                    }
                    else if (subQ[i].Length != 0)
                    {
                        if (HtmlHelper.Instance.TagsWithTagClosure.Any(subQ[i].Contains) || HtmlHelper.Instance.TagsWithoutTagClosure.Any(subQ[i].Contains))
                        {
                            current.TagName = subQ[i];
                        }
                    }

                }
                Selector child = new Selector();
                current.Child = child;
                current = child;
            }
            current=null;
            return root;
        }
    }
}
