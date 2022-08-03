using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_XML.Models
{
    public class Items
    {
        [XmlElement("title")]
        public string Title { get; set; } = string.Empty;
        [XmlElement("link")]
        public string Link { get; set; } = string.Empty;
        [XmlElement("description")]
        public string Description { get; set; } = string.Empty;
        [XmlElement("category")]
        public string Category { get; set; } = string.Empty;
        [XmlElement("pubDate")]
        public string PubDate { get; set; } = string.Empty;

        public Items() { }

        public Items(string title, string link, string description, string category, string pubDate)
        {
            Title = title;
            Link = link;
            Description = description;
            Category = category;
            PubDate = pubDate;
        }

        public override string ToString()
        {
            return $"{Title} {Link} {Description} {Category} {PubDate}";
        }
    }
}
