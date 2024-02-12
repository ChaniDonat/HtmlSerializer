using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace HtmlSerializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance=> _instance;
        public List<string> TagsWithTagClosure { get; set; }
        public List<string> TagsWithoutTagClosure { get; set; }

        private HtmlHelper() {
            this.TagsWithTagClosure = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("HtmlTags.json"));
            this.TagsWithoutTagClosure = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("HtmlVoidTags.json"));
        }
    }
}
