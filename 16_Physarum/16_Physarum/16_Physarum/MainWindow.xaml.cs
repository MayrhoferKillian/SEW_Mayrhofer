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
        byte[] pixelDaten; // Hier speichern wir die Helligkeit jedes Pixels
        double[] partikelX, partikelY, partikelWinkel;
        WriteableBitmap bitmap;
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            // Initialisiere die Leinwand (Graustufen)
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
            Spielfeld.Source = bitmap;
            pixelDaten = new byte[width * height];

            // 1000 Partikel zufällig platzieren
            partikelX = new double[1000];
            partikelY = new double[1000];
            partikelWinkel = new double[1000];

            for (int i = 0; i < 1000; i++)
            {
                partikelX[i] = rnd.Next(width);
                partikelY[i] = rnd.Next(height);
                partikelWinkel[i] = rnd.NextDouble() * Math.PI * 2;
            }

            // Startet die Schleife (läuft so oft wie der Monitor aktualisiert)
            CompositionTarget.Rendering += SimulationStep;
        }

        private void SimulationStep(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                // A. Bewegen
                partikelX[i] += Math.Cos(partikelWinkel[i]);
                partikelY[i] += Math.Sin(partikelWinkel[i]);

                // B. Ränder prüfen (wenn draußen, dann umdrehen)
                if (partikelX[i] < 0 || partikelX[i] >= width || partikelY[i] < 0 || partikelY[i] >= height)
                {
                    partikelX[i] = Math.Clamp(partikelX[i], 0, width - 1);
                    partikelY[i] = Math.Clamp(partikelY[i], 0, height - 1);
                    partikelWinkel[i] = rnd.NextDouble() * Math.PI * 2;
                }

                // C. Spur hinterlassen (Pixel hell machen)
                int index = (int)partikelY[i] * width + (int)partikelX[i];
                pixelDaten[index] = 255;

                // D. Ein bisschen zufällig drehen (damit es organisch aussieht)
                partikelWinkel[i] += (rnd.NextDouble() - 0.5) * 0.2;
            }

            // E. Alles ein bisschen verblassen lassen (Decay)
            for (int j = 0; j < pixelDaten.Length; j++)
            {
                if (pixelDaten[j] > 0) pixelDaten[j] = (byte)(pixelDaten[j] * 0.9);
            }

            // F. Das Array ins Bild zeichnen
            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixelDaten, width, 0);
        }
    }
}