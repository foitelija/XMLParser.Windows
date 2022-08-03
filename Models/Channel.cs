using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_XML.Models
{
    [Serializable]
    [XmlRoot("channel")]
    public class Channel
    {
        [XmlElement("item")]
        public List<Items> ChannelsList { get; set; } = new List<Items>();

        public override string ToString()
        {
            return $"{ChannelsList}";
        }
    }
}
