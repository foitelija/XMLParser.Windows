namespace WpfApp_XML.Models
{
    [Serializable]
    [XmlRoot("channel")]
    public class Channel
    {
        [XmlElement("item")]
        public Items[] Items { get; set; } //Массив объектов, а не лист, как было до этого.
    }
}
