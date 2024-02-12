using HtmlSerializer;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Xml.Linq;
 async static Task<string> LoadHtml(string url)
{
    Serializer serializer = new Serializer();
    var html = await serializer.Load(url);
    return html;
}
 static string[] SplitHtmlToLines(string html)
{
    var htmlLines = new Regex("<(.*?)>").Split(html).Where(line => line.Trim().Length > 0).ToArray();
    return htmlLines;
}
static List<string> checkAttribute(Match Attribute,HtmlElement element, Match[] attributes)
{
    if (Attribute != null)
    {
        var AttributeName = Attribute.Value.Split("=")[0];
        attributes = attributes.Where(attr => attr != Attribute).ToArray();
        Match match = Regex.Match(Attribute.Value, $@"{AttributeName}=(?<value>.*)");
        string AttributeValue = match.Groups["value"].Value;
        AttributeValue = AttributeValue.Replace("\"", string.Empty);
        List<string> AttributeValues = new List<string>(AttributeValue.Split(' '));
        return AttributeValues;
    }
    return new List<string>();
}
 static HtmlElement CreateHtmlElement(string line, HtmlElement tmp)
{
    var firstWord = line.Split(" ")[0];
    var lineWithoutFirstWord = line.Substring(line.IndexOf(" ") + 1);
    HtmlElement newHtmlElement = new HtmlElement(firstWord);
    Match[] attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line).ToArray();
        if (attributes.Length > 0)
        { 
        var classAttribute = attributes.FirstOrDefault(attribute => Regex.Matches(attribute.Value, @"class=(?<value>.*)").Count > 0);
        newHtmlElement.Classes = checkAttribute(classAttribute,tmp, attributes);
        attributes = attributes.Where(attr => attr != classAttribute).ToArray();
        var idAttribute = attributes.FirstOrDefault(attribute => Regex.Matches(attribute.Value, @"^id").Count > 0);
            if (idAttribute != null)
                newHtmlElement.Id= checkAttribute(idAttribute, tmp, attributes)[0];
        attributes = attributes.Where(attr => attr != idAttribute).ToArray();

        foreach (var item in attributes)
        {
            Match match = Regex.Match(item.ToString(), @"(?<name>.*)=(?<value>.*)");
            var name = match.Groups["name"].Value;
            var value = match.Groups["value"].Value;
            HtmlAttribute a = new HtmlAttribute(name, value);
            newHtmlElement.Attributes.Add(a);
        }
    }
    return newHtmlElement;
}
 bool IsSelfClosing(string line,string firstWord)
{
    return line.EndsWith("/") || HtmlHelper.Instance.TagsWithoutTagClosure.Any(firstWord.Contains);
}

string html = "https://learn.malkabruk.co.il/practicode/projects/pract-2/#html-serializer";
String myUrl =await LoadHtml(html);
var htmlLines = SplitHtmlToLines(myUrl);
HtmlElement root = new HtmlElement("!doctypehtml");
HtmlElement tmp = root;
foreach (var htmlLine in htmlLines)
{
    var firstWord = htmlLine.Split(" ")[0];

    if (firstWord == "/html")//finish
    {
    }
    else if (firstWord.StartsWith("/"))//לעלות רמה בעץ
    {
        tmp = tmp.Parent;
    }
    else if (HtmlHelper.Instance.TagsWithTagClosure.Any(firstWord.Contains)|| HtmlHelper.Instance.TagsWithoutTagClosure.Any(firstWord.Contains))
    {
        HtmlElement newHtmlElement = CreateHtmlElement(htmlLine, tmp);
        newHtmlElement.Parent=tmp;
        tmp.Children.Add(newHtmlElement);
        //Console.WriteLine(newHtmlElement.ToString());
        if (!IsSelfClosing(htmlLine, firstWord))
        {
            tmp = newHtmlElement;
        }
    }
    else//innerHtml Element
    {
        tmp.InnerHtml = htmlLine;
    }
    //if(tmp!=null)
      //Console.WriteLine(tmp.ToString());
}
Selector selector = new Selector();
Selector selector2 = new Selector();
Selector selector3 = new Selector();
selector = selector.convertQueryToSelector("div#myId1.MyClass1.Myclass2 .class.class2 div");
selector2 = selector.convertQueryToSelector("html.no-js");
selector3 = selector.convertQueryToSelector("div");
IEnumerable<HtmlElement> results = root.FindBySelector(selector3);

IEnumerable<HtmlElement> results3=root.FindBySelector(selector3);
foreach (var res in results)
{
    Console.WriteLine(res);

}


