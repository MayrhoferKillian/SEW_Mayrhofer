using CryptoClient;
using LiveCharts;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace CryptoAPI
{
    public partial class MainWindow : Window
    {
        private HttpClient _httpClient;


        public ChartValues<decimal> PriceValues { get; set; } = new ChartValues<decimal>();
        public List<string> TimeLabels { get; set; } = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this; // Wichtig für das Data-Binding [cite: 99]
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(UrlTextBox.Text) };

            try
            {
                var prices = await _httpClient.GetFromJsonAsync<List<CryptoPrice>>("api/CryptoPrices/history");

                if (prices != null)
                {
                    PriceValues.Clear();
                    TimeLabels.Clear();

                    foreach (var price in prices)
                    {
                        PriceValues.Add(price.CurrentPrice); // Preis hinzufügen [cite: 106]
                        TimeLabels.Add(price.Timestamp.ToString("HH:mm:ss")); // Zeit hinzufügen [cite: 107]
                    }

                    // X-Achse aktualisieren
                    PriceChart.AxisX[0].Labels = TimeLabels;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message);
            }
        }
    }
}