using Microsoft.Win32;
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

namespace _17_MusicPlayer
{
    public partial class MainWindow : Window
    {
        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Dateien (*.mp3;*.wav)|*.mp3;*.wav";

            if (openFileDialog.ShowDialog() == true)
            {
                Playlist.Items.Add(openFileDialog.FileName);
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Playlist.SelectedItem != null)
            {
                string path = Playlist.SelectedItem.ToString();
                _mediaPlayer.Open(new Uri(path));
                _mediaPlayer.Play();
            }
            else
            {
                MessageBox.Show("Wähle erst einen Song aus der Liste aus!");
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Stop();
        }
    }
}