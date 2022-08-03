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

        private async void LoadXml()
        {
            var GetDataList = await Task.Run(() => _service.AsyncRead());
            DataLoader.DataContext = GetDataList;
        }

        private async void ReadAsync_Click(object sender, RoutedEventArgs e)
        {
            var response = _service.AsyncRead();

            if(response != null)
            {
                MessageBox.Show("Асинхронное считывание произошло успешно.");
            }
        }

        private async void ReadRegular_Click(object sender, RoutedEventArgs e)
        {
            _service.ReadReg();
        }
    }
}
