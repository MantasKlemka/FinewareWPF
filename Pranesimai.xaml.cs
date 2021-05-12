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
    /// Interaction logic for Pervedimas.xaml
    /// </summary>
    public partial class Pranesimai : Window
    {
        Vartotojas vartotojasSaved;
        string keySaved;
        int puslapiuSk;
        public int pagrindinesSaskNr { get; set; } = 0;
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig

        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
        };

        public Pranesimai(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
            
            int puslapiuSkaicius = PuslapiuSkaicius(vartotojasSaved);
            puslapiuSk = puslapiuSkaicius;
            int currentPage = int.Parse((string)PuslapioNumeris.Content);
            PrintAllNotifications(currentPage);
            AtgalPuslapisButton.IsEnabled = false;
            AtgalPuslapisButton.Content = new Image
            {
                Source = new BitmapImage(new Uri("Images/disabledleftarrow.png", UriKind.Relative)),
                Height = 12,
                Width = 16
            };
            if (vartotojas.Pranesimai.Count != 0)
                NeraPranesimu.Visibility = Visibility.Hidden;

            if (vartotojas.Pranesimai.Count <= 9)
            {
                KitasPuslapisButton.IsEnabled = false;
                KitasPuslapisButton.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/disabledrightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            avatarIcon.Source = new BitmapImage(new Uri("Images/Avatars/avatar" + vartotojas.AvatarIndex + ".png", UriKind.Relative));
            vartotojasSaved.NaujiNotification = false;
            NotificationUpdate();
        }



        public int PuslapiuSkaicius(Vartotojas vartotojas)
        {
            int skaicius = 0;
            if(vartotojas.Pranesimai.Count % 9 == 0)
            {
                skaicius = vartotojas.Pranesimai.Count / 9;
            }
            else
            {
                skaicius = vartotojas.Pranesimai.Count / 9 + 1;
            }
            return skaicius;
        }

        public string CreateIban()
        {
            Random random = new Random();
            DateTime centuryBegin = new DateTime(1918, 1, 1);
            string iban = "LT";
            iban += DateTime.Now.Ticks - centuryBegin.Ticks;
            iban += random.Next(0, 9);
            return iban;
        }

        void PrintAllNotifications(int dabartinisPuslapis)
        {
            CreateBillBanner();

            int nr = 0;
            int pirmasPranesim = (dabartinisPuslapis - 1) * 9;

            // PAGE 1:
            // pirmas pranesimas = 0
            // max = 9

            // PAGE 2:
            // pirmas pranesimas = 9
            // max = 13

            int sk = 9;
            if (puslapiuSk == dabartinisPuslapis)
            {
                sk = (vartotojasSaved.Pranesimai.Count) % 9;
            }

            for (int i = pirmasPranesim; i < pirmasPranesim + sk; i++)
            {
                CreateBillLine(vartotojasSaved, i, nr);
                nr++;
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

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            closeBackground.Opacity = 0.8;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeBackground.Opacity = 1;
        }

        private void PatvirtintiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            patvirtintiBackround.Opacity = 0.8;
        }

        private void PatvirtintiButton_MouseLeave(object sender, MouseEventArgs e)
        {
            patvirtintiBackround.Opacity = 1;
        }

        private void IsrasasButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var israsas = new Israsai(vartotojasSaved, keySaved);
            israsas.Show();
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

        private void ManoButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            ManoSaskaitos manoSaskaitos = new ManoSaskaitos(vartotojasSaved, keySaved);
            manoSaskaitos.Show();
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
            var gauti = new GautiPavedima(vartotojasSaved, keySaved);
            gauti.Show();
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

        private async void NotificationUpdate()
        {
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
        }

        private async void PateiktiButton(object sender, RoutedEventArgs e)
        {

            if (vartotojasSaved.Saskaitos.Count == 9)
            {
                generalEventText.Content = "Jūs turite maksimalų skaičių sąskaitų!";
                return;
            }
            if (kodoTextBox.Password != vartotojasSaved.ShortSecurityCode.ToString())
            {
                generalEventText.Content = "Neteisingas 4-ių skaičių kodas!";
                return;
            }
            if(pavadinimoTextBox.Text == "")
            {
                generalEventText.Content = "Pavadinimas negali likti tuščias!";
                return;
            }

            IsEnabled = false;
            Loading();
            Panel.SetZIndex(greyedOut, 9);
            Saskaita naujaSaskaita = new Saskaita(pavadinimoTextBox.Text, CreateIban(), 0, DateTime.Now.Date);
            vartotojasSaved.Saskaitos.Add(naujaSaskaita);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
            closeBackground.Visibility = Visibility.Collapsed;
            closeButton.Visibility = Visibility.Collapsed;
            patvirtintiBackround.Visibility = Visibility.Collapsed;
            patvirtintiButton.Visibility = Visibility.Collapsed;
            SukurimasDescription.Visibility = Visibility.Collapsed;
            SukurtiLabel.Visibility = Visibility.Collapsed;
            sukurtiWindow.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            kodoTextBox.Visibility = Visibility.Collapsed;
            kodoDescription.Visibility = Visibility.Collapsed;
            pavadinimoDescription.Visibility = Visibility.Collapsed;
            pavadinimoTextBox.Visibility = Visibility.Collapsed;
            underLine_1.Visibility = Visibility.Collapsed;
            underLine_2.Visibility = Visibility.Collapsed;
            generalEventText.Visibility = Visibility.Collapsed;
            kodoTextBox.Clear();
            generalEventText.Content = "";
            pavadinimoTextBox.Clear();
            var manoSaskaitos = new ManoSaskaitos(vartotojasSaved, keySaved);
            manoSaskaitos.Show();
            Close();
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Collapsed;
            closeButton.Visibility = Visibility.Collapsed;
            patvirtintiBackround.Visibility = Visibility.Collapsed;
            patvirtintiButton.Visibility = Visibility.Collapsed;
            SukurimasDescription.Visibility = Visibility.Collapsed;
            SukurtiLabel.Visibility = Visibility.Collapsed;
            sukurtiWindow.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            kodoTextBox.Visibility = Visibility.Collapsed;
            kodoDescription.Visibility = Visibility.Collapsed;
            pavadinimoDescription.Visibility = Visibility.Collapsed;
            pavadinimoTextBox.Visibility = Visibility.Collapsed;
            underLine_1.Visibility = Visibility.Collapsed;
            underLine_2.Visibility = Visibility.Collapsed;
            generalEventText.Visibility = Visibility.Collapsed;
            kodoTextBox.Clear();
            pavadinimoTextBox.Clear();
            generalEventText.Content = "";
        }

        private void SukurtiNaujaButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Visible;
            patvirtintiBackround.Visibility = Visibility.Visible;
            patvirtintiButton.Visibility = Visibility.Visible;
            SukurimasDescription.Visibility = Visibility.Visible;
            SukurtiLabel.Visibility = Visibility.Visible;
            sukurtiWindow.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            kodoTextBox.Visibility = Visibility.Visible;
            kodoDescription.Visibility = Visibility.Visible;
            pavadinimoDescription.Visibility = Visibility.Visible;
            pavadinimoTextBox.Visibility = Visibility.Visible;
            underLine_1.Visibility = Visibility.Visible;
            underLine_2.Visibility = Visibility.Visible;
            generalEventText.Visibility = Visibility.Visible;
        }

        public void CreateBillLine(Vartotojas vartotojas, int id, int nr)
        {
            if(vartotojas.Pranesimai.Count != 0) 
            {
            int moveBackCof = (nr + 1) * 100;
            int moveCof = (nr + 1) * 50;
            var bc = new BrushConverter();
            Image background = new Image();
            background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            background.Margin = new Thickness(0, -500 + moveBackCof, 0, 0);
            saskaituGrid.Children.Add(background);

            Label siuntejas = new Label();
            siuntejas.Content = vartotojas.Pranesimai[id].siuntejas;
            siuntejas.Margin = new Thickness(30, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(siuntejas);

            Label gavejas = new Label();
            gavejas.Content = vartotojas.Pranesimai[id].gavejas;
            gavejas.Margin = new Thickness(200, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(gavejas);

            Label pavedimoTipas = new Label();
            pavedimoTipas.Content = vartotojas.Pranesimai[id].pavedimo_tipas;
            pavedimoTipas.Margin = new Thickness(375, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(pavedimoTipas);

            Label suma = new Label();
            suma.Foreground = (Brush)bc.ConvertFrom(SumosSpalva(vartotojas,id));
            suma.Content = SumosZenklas(vartotojas, id);
            suma.Margin = new Thickness(525, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(suma);

            Label data = new Label();
            data.Content = vartotojas.Pranesimai[id].data;
            data.Margin = new Thickness(650, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(data);

            Button veiksmai = new Button();
            veiksmai.Content = new Image
            {
                Source = new BitmapImage(new Uri("Images/delete.png", UriKind.Relative)),
            };
            veiksmai.Height = 15;
            veiksmai.Width = 15;
            veiksmai.Style = (Style)FindResource("buttonWithoutHighlight");
            veiksmai.BorderBrush = Brushes.Transparent;
            veiksmai.Background = Brushes.Transparent;
            veiksmai.MouseEnter += Button_MouseEnter;
            veiksmai.MouseLeave += Button_MouseLeave;
            veiksmai.VerticalAlignment = VerticalAlignment.Top;
            veiksmai.HorizontalAlignment = HorizontalAlignment.Left;
            veiksmai.Margin = new Thickness(820, -18 + moveCof, 0, 0);
            veiksmai.Name = "DeleteButton_" + id.ToString();
            veiksmai.Click += DeleteButton;
            saskaituGrid.Children.Add(veiksmai);
            if (vartotojas.Pranesimai[id].pavedimo_tipas == "Prašymas")
            {
                Button veiksmas = new Button();
                veiksmas.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/arrow.png", UriKind.Relative)),
                };
                veiksmas.Height = 15;
                veiksmas.Width = 15;
                veiksmas.Style = (Style)FindResource("buttonWithoutHighlight");
                veiksmas.BorderBrush = Brushes.Transparent;
                veiksmas.Background = Brushes.Transparent;
                veiksmas.MouseEnter += Button_MouseEnter;
                veiksmas.MouseLeave += Button_MouseLeave;
                veiksmas.VerticalAlignment = VerticalAlignment.Top;
                veiksmas.HorizontalAlignment = HorizontalAlignment.Left;
                veiksmas.Margin = new Thickness(840, -18 + moveCof, 0, 0);
                veiksmas.Name = "p" + id.ToString();

                veiksmas.Click += PerverstiSablonasButton;
                saskaituGrid.Children.Add(veiksmas);
            }
            }
        }
        private void PerverstiSablonasButton(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            int id = Convert.ToInt32((sender as Button).Name.Replace("p", ""));
            Loading();
            Pervedimas pervedimas = new Pervedimas(vartotojasSaved, keySaved);
            pervedimas.gavejoTextBox.Text = vartotojasSaved.Pranesimai[id].siuntejasEmail;
            pervedimas.saskaitosTextBox.Text = vartotojasSaved.Pranesimai[id].siuntejasSaskaita;
            pervedimas.sumaTextBox.Text = vartotojasSaved.Pranesimai[id].suma.ToString();
            pervedimas.paskirtisText.Text = vartotojasSaved.Pranesimai[id].Paskirtis.ToString();
            pervedimas.Show();
            Close();
        }

        public string SumosSpalva(Vartotojas vartotojas, int index)
        {
            string spalvosKodas = "";

            if(vartotojas.Pranesimai[index].pavedimo_tipas == "Gauta")
            {
                spalvosKodas = "#22B29D";
            }
            else if (vartotojas.Pranesimai[index].pavedimo_tipas == "Išsiūsta")
            {
                spalvosKodas = "#FFFF0000";
            }
            else
            {
                spalvosKodas = "#565656";
            }

            return spalvosKodas;
        }

        public string SumosZenklas(Vartotojas vartotojas, int index)
        {
            string eilute = "";

            if (vartotojas.Pranesimai[index].pavedimo_tipas == "Gauta")
            {
                eilute = "+" + Math.Round(vartotojas.Pranesimai[index].suma, 2) + " €";
            }
            else if (vartotojas.Pranesimai[index].pavedimo_tipas == "Išsiūsta")
            {
                eilute = "-" + Math.Round(vartotojas.Pranesimai[index].suma, 2) + " €";
            }
            else
            {
                eilute = Math.Round(vartotojas.Pranesimai[index].suma, 2) + " €";
            }

            return eilute;
        }

        private async void DeleteButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            IsEnabled = false;
            Loading();
            int nr = int.Parse(button.Name[13].ToString());
            vartotojasSaved.Pranesimai.RemoveAt(nr);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
            var pranesimai = new Pranesimai(vartotojasSaved, keySaved);
            pranesimai.Show();
            Close();
        }

        public void CreateBillBanner()
        {
            Image background = new Image();
            background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            background.Margin = new Thickness(0, -500, 0, 0);
            saskaituGrid.Children.Add(background);

            Label siuntejas = new Label();
            siuntejas.FontWeight = FontWeights.Bold;
            siuntejas.Content = "Siuntėjas";
            siuntejas.Margin = new Thickness(30, -23, 0, 0);
            saskaituGrid.Children.Add(siuntejas);

            Label gavejas = new Label();
            gavejas.FontWeight = FontWeights.Bold;
            gavejas.Content = "Gavėjas";
            gavejas.Margin = new Thickness(200, -23, 0, 0);
            saskaituGrid.Children.Add(gavejas);

            Label tipas = new Label();
            tipas.FontWeight = FontWeights.Bold;
            tipas.Content = "Pavedimo tipas";
            tipas.Margin = new Thickness(375, -23, 0, 0);
            saskaituGrid.Children.Add(tipas);

            Label Suma = new Label();
            Suma.FontWeight = FontWeights.Bold;
            Suma.Content = "Suma";
            Suma.Margin = new Thickness(525, -23, 0, 0);
            saskaituGrid.Children.Add(Suma);

            Label data = new Label();
            data.FontWeight = FontWeights.Bold;
            data.Content = "Data";
            data.Margin = new Thickness(650, -23, 0, 0);
            saskaituGrid.Children.Add(data);

            Label veiksmai = new Label();
            veiksmai.FontWeight = FontWeights.Bold;
            veiksmai.Content = "Veiksmai";
            veiksmai.Margin = new Thickness(820, -23, 0, 0);
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

        private void AtgalPuslapisButton_Click(object sender, RoutedEventArgs e)
        {
            saskaituGrid.Children.Clear();
            KitasPuslapisButton.IsEnabled = true;
            KitasPuslapisButton.Content = new Image
            {
                Source = new BitmapImage(new Uri("Images/rightarrow.png", UriKind.Relative)),
                Height = 12,
                Width = 16
            };
            if (int.Parse((string)PuslapioNumeris.Content)-1 == 1)
            {
                AtgalPuslapisButton.IsEnabled = false;
                AtgalPuslapisButton.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/disabledleftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            else
            {
                AtgalPuslapisButton.IsEnabled = true;
                AtgalPuslapisButton.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/leftarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            string text = (int.Parse((string)PuslapioNumeris.Content) - 1).ToString();
            PuslapioNumeris.Content = text;
            PrintAllNotifications(int.Parse(text));
        }

        private void KitasPuslapisButton_Click(object sender, RoutedEventArgs e)
        {
            saskaituGrid.Children.Clear();
            AtgalPuslapisButton.IsEnabled = true;
            AtgalPuslapisButton.Content = new Image
            {
                Source = new BitmapImage(new Uri("Images/leftarrow.png", UriKind.Relative)),
                Height = 12,
                Width = 16
            };
            if (int.Parse((string)PuslapioNumeris.Content)+1 == puslapiuSk)
            {
                KitasPuslapisButton.IsEnabled = false;
                KitasPuslapisButton.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/disabledrightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            else
            {
                KitasPuslapisButton.IsEnabled = true;
                KitasPuslapisButton.Content = new Image
                {
                    Source = new BitmapImage(new Uri("Images/rightarrow.png", UriKind.Relative)),
                    Height = 12,
                    Width = 16
                };
            }
            string text = (int.Parse((string)PuslapioNumeris.Content) + 1).ToString();
            PuslapioNumeris.Content = text;
            PrintAllNotifications(int.Parse(text));
        }
    }
}
