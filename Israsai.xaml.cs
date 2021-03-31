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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Israsai.xaml
    /// </summary>
    public partial class Israsai : Window
    {
        Vartotojas vartotojasSaved;
        string keySaved;
        public int pagrindinesSaskNr { get; set; } = 0;
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
        };

        public Israsai(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
            PrintAllChecks();
        }

        void PrintAllChecks()
        {
            CreateCheckBanner();
            //for (int i = 0; i < vartotojasSaved.Saskaitos.Count; i++)
            //{
            //    CreateBillLine(vartotojasSaved, i);
            //}
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            button.Opacity = 0.8;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            var button = (Button)sender;
            button.Opacity = 1;
        }


        private void ManoButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var mano = new ManoSaskaitos(vartotojasSaved, keySaved);
            mano.Show();
            Close();
        }

        private void ApzvalgaButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var apzvalga = new Apzvalga(vartotojasSaved, keySaved);
            apzvalga.Show();
            Close();
        }

        private void ValiutosButton(object sender, RoutedEventArgs e)
        {

        }

        private void PagalbaButton(object sender, RoutedEventArgs e)
        {

        }

        private void PavedimasButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var pervedimas = new Pervedimas(vartotojasSaved, keySaved);
            pervedimas.Show();
            Close();
        }

        private void GautiButton(object sender, RoutedEventArgs e)
        {

        }

        private void NustatymaiButton(object sender, RoutedEventArgs e)
        {

        }

        private void AtsijungtiButton(object sender, RoutedEventArgs e)
        {

        }

        public void CreateCheckLine(Vartotojas vartotojas, int israsoNr)
        {
            // pataisyt, kad spausdintu israsus kaip spausdina mano saskaitas
            // kodas iš mano saskaitu spausdinimo
            //int moveBackCof = (israsoNr + 1) * 100;
            //int moveCof = (israsoNr + 1) * 50;
            //var bc = new BrushConverter();
            //Image background = new Image();
            //background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            //background.Margin = new Thickness(0, -350 + moveBackCof, 0, 0);
            //saskaituGrid.Children.Add(background);
            //Label pavadinimas = new Label();
            //pavadinimas.Content = vartotojas.Saskaitos[saskaitosNr].Pavadinimas;
            //pavadinimas.Margin = new Thickness(30, -23 + moveCof, 0, 0);
            //saskaituGrid.Children.Add(pavadinimas);
            //Label kodas = new Label();
            //kodas.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            //kodas.Content = vartotojas.Saskaitos[saskaitosNr].Kodas;
            //kodas.Margin = new Thickness(250, -23 + moveCof, 0, 0);
            //saskaituGrid.Children.Add(kodas);
            //Label data = new Label();
            //data.Content = vartotojas.Saskaitos[saskaitosNr].SukurimoData.ToShortDateString();
            //data.Margin = new Thickness(450, -23 + moveCof, 0, 0);
            //saskaituGrid.Children.Add(data);
            //Label likutis = new Label();
            //likutis.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            //likutis.Content = Math.Round(vartotojas.Saskaitos[saskaitosNr].Likutis, 2) + " €";
            //likutis.Margin = new Thickness(650, -23 + moveCof, 0, 0);
            //saskaituGrid.Children.Add(likutis);
            //if (saskaitosNr != 0)
            //{
            //    Button veiksmai = new Button();
            //    veiksmai.Content = new Image
            //    {
            //        Source = new BitmapImage(new Uri("Images/delete.png", UriKind.Relative)),
            //    };
            //    veiksmai.Height = 15;
            //    veiksmai.Width = 15;
            //    veiksmai.Style = (Style)FindResource("buttonWithoutHighlight");
            //    veiksmai.BorderBrush = Brushes.Transparent;
            //    veiksmai.Background = Brushes.Transparent;
            //    veiksmai.MouseEnter += Button_MouseEnter;
            //    veiksmai.MouseLeave += Button_MouseLeave;
            //    veiksmai.VerticalAlignment = VerticalAlignment.Top;
            //    veiksmai.HorizontalAlignment = HorizontalAlignment.Left;
            //    veiksmai.Margin = new Thickness(820, -23 + moveCof, 0, 0);
            //    veiksmai.Name = "DeleteButton_" + saskaitosNr.ToString();
            //    veiksmai.Click += DeleteButton;
            //    saskaituGrid.Children.Add(veiksmai);
            //}
        }

        public void CreateCheckBanner()
        {
            Image background = new Image();
            background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            background.Margin = new Thickness(0, -350, 0, 0);
            saskaituGrid.Children.Add(background);
            Label pavadinimas = new Label();
            pavadinimas.FontWeight = FontWeights.Bold;
            pavadinimas.Content = "Data";
            pavadinimas.Margin = new Thickness(20, 35, 0, 0);
            saskaituGrid.Children.Add(pavadinimas);
            Label kodas = new Label();
            kodas.FontWeight = FontWeights.Bold;
            kodas.Content = "Laikas";
            kodas.Margin = new Thickness(120, 35, 0, 0);
            saskaituGrid.Children.Add(kodas);
            Label data = new Label();
            data.FontWeight = FontWeights.Bold;
            data.Content = "Gavėjas/Mokėtojas";
            data.Margin = new Thickness(250, 35, 0, 0);
            saskaituGrid.Children.Add(data);
            Label likutis = new Label();
            likutis.FontWeight = FontWeights.Bold;
            likutis.Content = "Mokėjimo paskirtis";
            likutis.Margin = new Thickness(450, 35, 0, 0);
            saskaituGrid.Children.Add(likutis);
            Label suma = new Label();
            suma.FontWeight = FontWeights.Bold;
            suma.Content = "Suma";
            suma.Margin = new Thickness(750, 35, 0, 0);
            saskaituGrid.Children.Add(suma);
            Label veiksmai = new Label();
            veiksmai.FontWeight = FontWeights.Bold;
            veiksmai.Content = "Veiksmai";
            veiksmai.Margin = new Thickness(850, 35, 0, 0);
            saskaituGrid.Children.Add(veiksmai);
        }

        void Loading()
        {
            dotProgress1.Visibility = Visibility.Visible;
            dotProgress2.Visibility = Visibility.Visible;
            dotProgress3.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            Storyboard loading = (Storyboard)TryFindResource("loading");
            loading.Begin();
        }
    }
}
