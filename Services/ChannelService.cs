﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_XML.Services
{
    public class ChannelService : IChannelService
    {
        Channel? records = new Channel();
        Items[] list;
        public async Task<Channel> AsyncRead()
        {
            string path = $"{Environment.CurrentDirectory}/data.xml";

            await Task.Run(() =>
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Channel));
                FileStream fs = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);

                records = (Channel)xmlSerializer.Deserialize(reader);
            });
            return new Channel(); // для теста, нужно поставить останов сдесь
        }

        public async Task<Items> ReadReg()
        {
            string path = $"{Environment.CurrentDirectory}/data.xml";

            StreamReader reader = new StreamReader(path, Encoding.UTF8);
            var content = reader.ReadToEnd();

            Regex item = new Regex(@"<item>[\s\S\w\W\d\D]*?</item>");
            MatchCollection matches = item.Matches(content);

            List<Items> result = new List<Items>();
            var channel = new Items();

            await Task.Run(() =>
            {
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {

                        Regex Title = new Regex(@"<title>[\s\S\w\W\d\D]*?</title>");
                        MatchCollection mTitle = Title.Matches(match.Value);
                        channel.Title = mTitle[0].Value.Replace("<title>", "").Replace("</title>", "");

                        Regex Link = new Regex(@"<link>[\s\S\w\W\d\D]*?</link>");
                        MatchCollection mLink = Link.Matches(match.Value);
                        channel.Link = mLink[0].Value.Replace("<link>", "").Replace("</link>", "");

                        Regex Description = new Regex(@"<description>[\s\S\w\W\d\D]*?</description>");
                        MatchCollection mDescription = Description.Matches(match.Value);
                        channel.Description = mDescription[0].Value.Replace("<description>", "").Replace("</description>", "");

                        Regex Category = new Regex(@"<category>[\s\S\w\W\d\D]*?</category>");
                        MatchCollection mCategory = Category.Matches(match.Value);
                        channel.Category = mCategory[0].Value.Replace("<category>", "").Replace("</category>", "");

                        Regex regexPubDate = new Regex(@"<pubDate>[\s\S\w\W\d\D]*?</pubDate>");
                        MatchCollection matchePubDate = regexPubDate.Matches(match.Value);
                        channel.PubDate = matchePubDate[0].Value.Replace("<pubDate>", "").Replace("</pubDate>", "");

                        result.Add(channel);
                    }
                }
                else
                {
                    MessageBox.Show("Совпадений не найдено");
                }
            });
            return new Items(); // для теста, нужно поставить останова сдесь
        }
    }
}