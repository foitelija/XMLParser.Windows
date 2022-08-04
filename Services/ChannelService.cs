namespace WpfApp_XML.Services
{
    public class ChannelService : IChannelService
    {
        string path = $"{Environment.CurrentDirectory}/Import/data.xml"; // там где и EXE файл -> Папка Import.
        string pathTxt = $"{Environment.CurrentDirectory}/Export/notTxt.txt"; // там где и EXE файл -> Папка Export.
        string pathDoc = $"{Environment.CurrentDirectory}/Export/Minecraft.docx"; // там где и EXE файл -> Папка Export.
        string pathXls = $"{Environment.CurrentDirectory}/Export/GenshinImpact.xlsx"; // там где и EXE файл -> Папка Export. 

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

                        Regex Link = new Regex(@"<link>[\s\S\w\W]*?</link>");
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
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(pathTxt, true))
                {
                    await Task.Run(() =>
                    {
                        foreach (Items itemsToTxt in items)
                        {
                            streamWriter.WriteLine();
                            streamWriter.WriteLine(itemsToTxt.Category);
                            streamWriter.WriteLine(itemsToTxt.Title);
                            streamWriter.WriteLine(itemsToTxt.Description);
                            streamWriter.WriteLine(itemsToTxt.PubDate);
                            streamWriter.WriteLine(itemsToTxt.Link);
                        }
                        streamWriter.Close();
                    });
                }
                MessageBox.Show("Bingo!!! Документ успешно создан, с вас хубабубабубабубабуба.");
                try
                {
                    Process.Start("C:\\Windows\\System32\\notepad.exe", pathTxt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Путь к блокноту не найден, автомат. открытие недоступно." + "\n" + ex.Message);
                }
            }
            return items; //Async, по останову всё работает как надо
        }

        public async Task<Items[]> toDocx()
        {
            if (items == null)
            {
                await AsyncRead();
            }
            else
            {
                Word.Application application = new Word.Application();
                Word.Document document = application.Documents.Add();
                //работаем с коллекцией для абзацев текста  http://wladm.narod.ru 
                Word.Paragraph textParagraph = document.Content.Paragraphs.Add();

                await Task.Run(() =>
                {
                    foreach (Items itemList in items)
                    {
                        //Это кстати прикол какой-то, без Envr.NL не работает.......
                        textParagraph.Range.Text = Environment.NewLine;
                        //типизированные строки
                        textParagraph.Range.Text = $"{itemList.Category}{Environment.NewLine}"; 
                        textParagraph.Range.Text = $"{itemList.Title}{Environment.NewLine}";
                        textParagraph.Range.Text = $"{itemList.Description}{Environment.NewLine}";
                        textParagraph.Range.Text = $"{itemList.PubDate}{Environment.NewLine}";
                        textParagraph.Range.Text = $"{itemList.Link}{Environment.NewLine}";
                    }
                    document.SaveAs2(pathDoc);
                    document.Close();
                    application.Quit();
                });
                MessageBox.Show("Bingo!!! Документ успешно создан, с вас 5$.");
                //открываем ворд после манипуляций с ним.
                try
                {
                    application.Visible = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("RPC сервер недоступен. Требуется ручная перезагрузка через Пуск или Перазгрузка компьютера.", ex.Message);
                }
            }
            return items;
        }

        public async Task<Items[]> toXls()
        {
            if (items == null)
            {
                await AsyncRead();
            }
            else
            {
                await Task.Run(() =>
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using(ExcelPackage pck = new ExcelPackage())
                    {
                        pck.Workbook.Worksheets.Add("Channels").Cells[1, 1].LoadFromCollection(items, true);
                        pck.SaveAs(new FileInfo(pathXls));
                    }
                });

                MessageBox.Show("Экспорт в эксел произошёл успешно.");
            }
            
            return items;
        }
    }
}
