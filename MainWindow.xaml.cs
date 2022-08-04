using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_XML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ChannelService _service;

        public MainWindow()
        {
            _service = new ChannelService();
            InitializeComponent();
            LoadXml();
        }

        private Task LoadXml() // если будет время, переделаю в Async (если смогу)
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml($"{Environment.CurrentDirectory}/dataDGV.xml");
            DataLoader.ItemsSource = dataSet.Tables[0].DefaultView;
            return Task.CompletedTask;
        }

        private async void ReadAsync_Click(object sender, RoutedEventArgs e)
        {
            var response = _service.AsyncRead();

            if(response != null)
            {
                MessageBox.Show("Асинхронное считывание произошло успешно.");
            }
            else 
            {
                MessageBox.Show("Егор404");
            }
        }

        private async void ReadRegular_Click(object sender, RoutedEventArgs e)
        {
            var  response = _service.ReadReg();
            {
                if(response != null)
                {
                    MessageBox.Show("Считывание при помоги RegEx произошло успешно. \n Теперь можно делать любой из экспортов.");
                }
                else
                {
                    MessageBox.Show("Егор404");
                }
            }
        }

        private async void toTxt_Click(object sender, RoutedEventArgs e)
        {
            if (_service.AsyncRead != null)
            {
                await _service.toTxt();
                await Task.Delay(1800); // ждём 1.8 секунды
                //Process.Start(@$"D:\MY_PROJECTS_ON_CSHARP\WpfApp_XML\bin\Debug\net6.0-windows\notTxt.txt"); // пофиксить.
            }
            await _service.AsyncRead();
        }

        private void toWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void toExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application exApp = new Excel.Application();

            exApp.Workbooks.Add();
            Excel.Worksheet wsh = (Excel.Worksheet)exApp.ActiveSheet;
            int i, j;
            //строки
           
            exApp.Visible = true;
        }
    }
}
