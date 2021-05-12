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
        public int index = 0;
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
            //int index = 0;
            PrintAllChecks(index);
            puslapiuNumeris.Content ="1";
            if ((string)puslapiuNumeris.Content == "1")
            {
                atgal.IsEnabled = false;
                atgal.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/disabledleftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
                if (vartotojas.Saskaitos[pagrindinesSaskNr].Israsai.Count < 8)
                {
                    toliau.IsEnabled = false;
                    toliau.Content = new Image
                    {
                        Source = new BitmapImage(new Uri("Images/disabledrightarrow.png", UriKind.Relative)),
                        Height = 12,
                        Width = 16
                    };
                }
            }

            avatarIcon.Source = new BitmapImage(new Uri("Images/Avatars/avatar" + vartotojas.AvatarIndex + ".png", UriKind.Relative));
            //vartotojas.Saskaitos[pagrindinesSaskNr].Israsai.Sort();
        }

        public int PuslapiuSkaicius(Vartotojas vartotojas)
        {
            int skaicius = 0;
            if(vartotojas.Saskaitos[pagrindinesSaskNr].Israsai.Count % 7 == 0)
            {
                skaicius = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai.Count / 7;
            }
            else
            {
                skaicius = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai.Count / 7 + 1;
            }
            return skaicius;
        }

        // --------------------- SENAS 'PrintAllChecks' metodas ----------------------
        //void PrintAllChecks(int index)
        //{
        //    int sk = 0;
        //    int pslKiekis = PuslapiuSkaicius(vartotojasSaved); // israsu puslapiu kiekis
        //    int count = 0;
        //    CreateCheckBanner();
        //    for (int i = index; i < vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai.Count; i++)
        //    {
        //        if (count < 7)
        //        {
        //            CreateCheckLine(vartotojasSaved, i,sk);
        //            sk++;
        //        }
        //        count++;
        //    }
        //}

        void PrintAllChecks(int index)
        {
            int sk = 0;
            int pslKiekis = PuslapiuSkaicius(vartotojasSaved); // israsu puslapiu kiekis
            int count = 0;
            CreateCheckBanner();
            for (int i = vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai.Count-1; i > index; i--)
            {
                if (count < 7)
                {
                    CreateCheckLine(vartotojasSaved, i, sk);
                    sk++;
                }
                count++;
            }
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
            Loading();
            IsEnabled = false;
            GautiPavedima gautiPavedima = new GautiPavedima(vartotojasSaved, keySaved);
            gautiPavedima.Show();
            Close();
        }

        private void NustatymaiButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var nustatymai = new Nustatymai(vartotojasSaved, keySaved);
            nustatymai.Show();
            Close();
        }

        private void AtsijungtiButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var prisijungimas = new Prisijungimas();
            prisijungimas.Show();
            Close();
        }

        public void CreateCheckLine(Vartotojas vartotojas, int israsoNr, int sk)
        {
            int moveBackCof;
            int moveCof;

            moveBackCof = (sk + 1) * 100;
            moveCof = (sk + 1) * 50;

            var bc = new BrushConverter();
            Image background = new Image();
            background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            background.Margin = new Thickness(0, -350 + moveBackCof, 0, 0);
            saskaituGrid.Children.Add(background);
            Label pavadinimas = new Label();
            pavadinimas.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Data.ToShortDateString();
            pavadinimas.Margin = new Thickness(10, 35 + moveCof, 0, 0);
            saskaituGrid.Children.Add(pavadinimas);
            Label kodas = new Label();
            kodas.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            kodas.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Data.ToShortTimeString();
            kodas.Margin = new Thickness(116, 35 + moveCof, 0, 0);
            saskaituGrid.Children.Add(kodas);
            Label data = new Label();
            data.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Asmuo;
            data.Margin = new Thickness(250, 35 + moveCof, 0, 0);
            saskaituGrid.Children.Add(data);
            Label likutis = new Label();
            likutis.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            likutis.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Paskirtis;
            likutis.Margin = new Thickness(450, 35 + moveCof, 200, 0);
            saskaituGrid.Children.Add(likutis);
            Label paskirtis = new Label();
            if(vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Tipas == "Siuncia")
            {
                paskirtis.Foreground = (Brush)bc.ConvertFrom("#FFFF0000");
                paskirtis.Content = "-" +vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Suma + " €";
            }
            else if (vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Tipas == "Gauna")
            {
                paskirtis.Foreground = (Brush)bc.ConvertFrom("#008000");
                paskirtis.Content = "+" + vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Suma + " €";
            }
            paskirtis.Margin = new Thickness(750, 35 + moveCof, 0, 0);
            saskaituGrid.Children.Add(paskirtis);
            Label veiksmai = new Label();
            veiksmai.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            veiksmai.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai[israsoNr].Tipas;
            veiksmai.Margin = new Thickness(850, 35 + moveCof, 0, 0);
            saskaituGrid.Children.Add(veiksmai);
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

        private void Toliau_Click(object sender, RoutedEventArgs e)
        {
            puslapiuNumeris.Content = (Convert.ToInt32(puslapiuNumeris.Content) + 1).ToString();
            int pslKiekis = PuslapiuSkaicius(vartotojasSaved);
            if(pslKiekis.ToString() == puslapiuNumeris.Content.ToString())
            {
                toliau.IsEnabled = false;
                toliau.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/disabledrightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
                atgal.IsEnabled = true;
                atgal.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/leftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            else
            {
                toliau.IsEnabled = true;
                toliau.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/rightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
                atgal.IsEnabled = true;
                atgal.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/leftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }

            index = index + 7;
            saskaituGrid.Children.Clear();
            PrintAllChecks(index);
        }

        private void Pirmas_Click(object sender, RoutedEventArgs e)
        {
            atgal.IsEnabled = false;
            atgal.Content = new Image
            {
                Source = new BitmapImage(new Uri("Images/disabledleftarrow.png", UriKind.Relative)),
                Height = 12,
                Width = 16
            };
            toliau.IsEnabled = true;
            toliau.Content = new Image
            {
                Source = new BitmapImage(new Uri("Images/rightarrow.png", UriKind.Relative)),
                Height = 12,
                Width = 16
            };
            puslapiuNumeris.Content = "1";
            index = 0;
            saskaituGrid.Children.Clear();
            PrintAllChecks(index);
        }

        private void Atgal_Click(object sender, RoutedEventArgs e)
        {
            puslapiuNumeris.Content = (Convert.ToInt32(puslapiuNumeris.Content) - 1).ToString();
            if (puslapiuNumeris.Content.ToString() == "1")
            {
                atgal.IsEnabled = false;
                atgal.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/disabledleftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
                toliau.IsEnabled = true;
                toliau.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/rightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            else
            {
                atgal.IsEnabled = true;
                atgal.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/leftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
                toliau.IsEnabled = true;
                toliau.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/rightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            
            index = index - 7;
            saskaituGrid.Children.Clear();
            PrintAllChecks(index);
        }
    }
}
