using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<HtmlAttribute> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string tagName)
        {
            Name = tagName;
            Attributes = new List<HtmlAttribute>();
            Classes = new List<string>();
            Children=new List<HtmlElement>();
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<");
            builder.Append(Name);

            if (Id != null)
            {
                builder.Append(" id=\"");
                builder.Append(Id);
                builder.Append("\"");
            }

            for (int i = 0; i < Attributes.Count; i++)
            {
                builder.Append(" ");
                builder.Append(Attributes[i].ToString());
            }

            if (Classes.Count > 0)
            {
                builder.Append(" class=\"");
                builder.Append(string.Join(" ", Classes));
                builder.Append("\"");
            }

            builder.Append(">");

            if (InnerHtml != null)
            {
                builder.Append(InnerHtml);
            }

            builder.Append("</");
            builder.Append(Name);
            builder.Append(">");

            return builder.ToString();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                yield return element;
                foreach (HtmlElement child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this.Parent;
            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
        }
        public IEnumerable<HtmlElement> FindBySelector(Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();

            return FindBySelector(selector, results);

            IEnumerable<HtmlElement> FindBySelector(Selector currentSelector, HashSet<HtmlElement> results)
            {
                foreach (HtmlElement descendant in Descendants())
                {
                    bool matches = MatchesSelector(descendant, currentSelector);

                    if (matches)
                    {
                        if (currentSelector.Child == null)
                        {
                            results.Add(descendant);
                        }
                        else
                        {
                            FindBySelector(currentSelector.Child, results);
                        }
                    }
                }
                return results;
            }
            bool MatchesSelector(HtmlElement element, Selector selector)
            {
                if (selector.TagName != null && selector.TagName != element.Name)
                {
                    return false;
                }
                if (selector.Id != null && selector.Id != element.Id)
                {
                    return false;
                }
                if (selector.Classes != null && !selector.Classes.All(c => element.Classes.Contains(c)))
                {
                    return false;
                }
                return true;
            }
        }
    }




}
