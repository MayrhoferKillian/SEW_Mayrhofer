using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _14_WpfSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            async Task<int> Add(int x, int y)
            {
                await Task.Delay(3000);
                return x + y; 
            }



            private async void Button_Click(object sender, RoutedEventArgs e)
            {
                lblOutput.Content = "Start";
                int a = 10;
                int b = 15;
            int result = await Add(a, b).Result;
            lblOutput.Content = result;
            }
        }
    }
}