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

namespace _16_Physarum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 1. Einstellungen
        int width = 400; int height = 300;
        byte[] pixelDaten; // Helligkeit jedes Pixels 0-255
        double[] partikelX, partikelY, partikelWinkel;
        WriteableBitmap bitmap;
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            // Initialisiere die Leinwand (Graustufen)
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null); //neue Instanz von Objekt Writablebitmap
            Spielfeld.Source = bitmap; //Spielfeld im xaml
            pixelDaten = new byte[width * height]; //Speicherplatz reserviert

            // 1000 Partikel zufällig platzieren
            partikelX = new double[1000];
            partikelY = new double[1000];
            partikelWinkel = new double[1000];

            for (int i = 0; i < 1000; i++)
            {
                partikelX[i] = rnd.Next(width);
                partikelY[i] = rnd.Next(height);
                partikelWinkel[i] = rnd.NextDouble() * Math.PI * 2; //startposition/winkel des partikels 
            }

            // Startet die Schleife (läuft so oft wie der Monitor aktualisiert)
            CompositionTarget.Rendering += SimulationStep;
        }

        private void SimulationStep(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
            
                partikelX[i] += Math.Cos(partikelWinkel[i]); //berechnet wie weit sich der partikel bewegt anhand vom  winkel
                partikelY[i] += Math.Sin(partikelWinkel[i]);

                // Ränder prüfen
                if (partikelX[i] < 0 || partikelX[i] >= width || partikelY[i] < 0 || partikelY[i] >= height)
                {
                    partikelX[i] = Math.Clamp(partikelX[i], 0, width - 1); //setzt den pixel auf den rand zurück und 
                    partikelY[i] = Math.Clamp(partikelY[i], 0, height - 1);
                    partikelWinkel[i] = rnd.NextDouble() * Math.PI * 2; //schickt den pixel in eine rnd richtung
                }

                // Spur hinterlassen 
                int index = (int)partikelY[i] * width + (int)partikelX[i]; //2D in 1D für array umrechnen
                pixelDaten[index] = 255; //ausgewählter index auf 255 gesetzt
            }

            // Das Array ins Bild zeichnen
            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelDaten, width, 0);
        }
    }
}