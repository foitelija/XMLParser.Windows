using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_XML.Services
{
    public class ChannelService : IChannelService
    {
        string path = $"{Environment.CurrentDirectory}/data.xml"; // там где и EXE файл.
        string pathTxt = $"{Environment.CurrentDirectory}/notTxt.txt"; // там где и EXE файл.
        string pathDoc = $"{Environment.CurrentDirectory}/notTxt.txt"; // там где и EXE файл.
        string pathXls = $"{Environment.CurrentDirectory}/notTxt.txt"; // там где и EXE файл.

        public Items[] items;

        public async Task<Channel> AsyncRead()
        {
            await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Channel));
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);
                items = (xmlSerializer.Deserialize(reader) as Channel).Items;
                reader.Close();
            });

            return new Channel(); // для теста, нужно поставить останов сдесь -> this - Items [0] ... [n]. Или сразу запускать toTXT
        }

        public async Task<Items> ReadReg()
        {
            StreamReader reader = new StreamReader(path, Encoding.UTF8);
            var content = reader.ReadToEnd();

            Regex item = new Regex(@"<item>[\s\S\w\W\d\D]*?</item>");
            MatchCollection matches = item.Matches(content);

            List<Items> result = new List<Items>();
            var itemsList = new Items();

            await Task.Run(() =>
            {
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {

                        Regex Title = new Regex(@"<title>[\s\S\w\W]*?</title>");
                        MatchCollection mTitle = Title.Matches(match.Value);
                        itemsList.Title = mTitle[0].Value.Replace("<title>", "").Replace("</title>", "");

                        Regex Link = new Regex(@"<link>[\s\S\w\W*?</link>");
                        MatchCollection mLink = Link.Matches(match.Value);
                        itemsList.Link = mLink[0].Value.Replace("<link>", "").Replace("</link>", "");

                        Regex Description = new Regex(@"<description>[\s\S\w\W]*?</description>");
                        MatchCollection mDescription = Description.Matches(match.Value);
                        itemsList.Description = mDescription[0].Value.Replace("<description>", "").Replace("</description>", "");

                        Regex Category = new Regex(@"<category>[\s\S\w\W]*?</category>");
                        MatchCollection mCategory = Category.Matches(match.Value);
                        itemsList.Category = mCategory[0].Value.Replace("<category>", "").Replace("</category>", "");

                        Regex PubDate = new Regex(@"<pubDate>[\s\S\w\W\d\D]*?</pubDate>");
                        MatchCollection matchePubDate = PubDate.Matches(match.Value);
                        itemsList.PubDate = matchePubDate[0].Value.Replace("<pubDate>", "").Replace("</pubDate>", "");

                        result.Add(itemsList);
                    }
                }
                else
                {
                    MessageBox.Show("Совпадений не найдено");
                }
            });
            return new Items(); // для теста, нужно поставить точку останова сдесь
        }

        public async Task<Items[]> toTxt()
        {
            if(items == null)
            {
                await AsyncRead();
            }
            using (StreamWriter streamWriter = new StreamWriter(pathTxt, true))
            {
                await Task.Run(() =>
                {
                    foreach (Items itemsToTxt in items)
                    {
                        streamWriter.WriteLine();
                        streamWriter.WriteLine(itemsToTxt.Title);
                        streamWriter.WriteLine(itemsToTxt.Link);
                        streamWriter.WriteLine(itemsToTxt.Description);
                        streamWriter.WriteLine(itemsToTxt.Category);
                        streamWriter.WriteLine(itemsToTxt.PubDate);
                    }
                    streamWriter.Close();
                });
            }
            return items; //Async, по останову всё работает как надо
        }

        public Task<Items[]> toDocx()
        {
            throw new NotImplementedException();
        }

        public Task<Items[]> toXls()
        {
            throw new NotImplementedException();
        }
    }
}
