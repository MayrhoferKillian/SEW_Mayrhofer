using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace _19_Kaffee_Konfigurator
{
    // Implementiert das Interface INotifyPropertyChanged
    public class MainViewModel : INotifyPropertyChanged
    {
        // Hält eine Instanz des Models
        private CoffeeOrder _order = new CoffeeOrder();
        private string _selectedSize;
        private bool _isExtraShot;
        private double _totalPrice;

        public MainViewModel()
        {
            // Implementiert ein ICommand für den Bestellen-Button
            // Deaktivieren über CanExecute, wenn keine Größe gewählt
            OrderCommand = new RelayCommand(ResetOrder, () => !string.IsNullOrEmpty(SelectedSize));
        }

        // Properties für die UI
        public string SelectedSize
        {
            get => _selectedSize;
            set
            {
                _selectedSize = value;
                UpdatePrice(); // Berechnet Preis im Setter
                OnPropertyChanged();
            }
        }

        public bool IsExtraShot
        {
            get => _isExtraShot;
            set
            {
                _isExtraShot = value;
                UpdatePrice(); // Berechnet Preis im Setter
                OnPropertyChanged();
            }
        }

        // Zeigt den aktuellen Gesamtpreis an
        public double TotalPrice
        {
            get => _totalPrice;
            private set { _totalPrice = value; OnPropertyChanged(); }
        }

        public ICommand OrderCommand { get; }

        private void UpdatePrice()
        {
            double basePrice = SelectedSize switch
            {
                "Klein" => 2.50,
                "Mittel" => 3.50,
                "Groß" => 4.50,
                _ => 0
            };

            // Gesamtpreis = Grundpreis + Extra Schuss
            TotalPrice = basePrice + (IsExtraShot ? _order.ExtraShotPrice : 0);
        }

        private void ResetOrder()
        {
            // Button „Bestellen" setzt den Konfigurator zurück
            SelectedSize = null;
            IsExtraShot = false;
            TotalPrice = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}