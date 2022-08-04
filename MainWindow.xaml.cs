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
        }

        private async Task<DataSet> LoadXml() // Тут я по пути меньшего сопротивления пошёл.
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml($"{Environment.CurrentDirectory}/Import/dataDGV.xml");
            DataLoader.ItemsSource = dataSet.Tables[0].DefaultView;
            return dataSet;
        }

        private async void ReadAsync_Click(object sender, RoutedEventArgs e)
        {
            var response = await _service.AsyncRead();

            if(response != null)
            {
                MessageBox.Show("Асинхронное считывание произошло успешно. \nМожно делать экспорт.");
                await LoadXml();
            }
            else 
            {
                MessageBox.Show("Егор404");
            }
        }

        private async void ReadRegular_Click(object sender, RoutedEventArgs e)
        {
            var  response = await _service.ReadReg();
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
            await _service.toTxt();
        }

        private async void toWord_Click(object sender, RoutedEventArgs e)
        {
            await _service.toDocx();
        }

        private async void toExcel_Click(object sender, RoutedEventArgs e)
        {
            await _service.toXls();
        }
    }
}
