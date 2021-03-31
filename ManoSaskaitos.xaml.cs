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
    public partial class ManoSaskaitos : Window
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

        public ManoSaskaitos(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
            PrintAllBills();
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

        void PrintAllBills()
        {
            CreateBillBanner();
            for (int i = 0; i < vartotojasSaved.Saskaitos.Count; i++)
            {
                CreateBillLine(vartotojasSaved, i);
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

        private void SukurtiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            sukurtiBackround.Opacity = 0.8;
        }

        private void SukurtiButton_MouseLeave(object sender, MouseEventArgs e)
        {
            sukurtiBackround.Opacity = 1;
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

        public void CreateBillLine(Vartotojas vartotojas, int saskaitosNr)
        {
            int moveBackCof = (saskaitosNr + 1) * 100;
            int moveCof = (saskaitosNr + 1) * 50;
            var bc = new BrushConverter();
            Image background = new Image();
            background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            background.Margin = new Thickness(0, -500 + moveBackCof, 0, 0);
            saskaituGrid.Children.Add(background);
            Label pavadinimas = new Label();
            pavadinimas.Content = vartotojas.Saskaitos[saskaitosNr].Pavadinimas;
            pavadinimas.Margin = new Thickness(30, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(pavadinimas);
            Label kodas = new Label();
            kodas.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            kodas.Content = vartotojas.Saskaitos[saskaitosNr].Kodas;
            kodas.Margin = new Thickness(250, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(kodas);
            Label data = new Label();
            data.Content = vartotojas.Saskaitos[saskaitosNr].SukurimoData.ToShortDateString();
            data.Margin = new Thickness(450, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(data);
            Label likutis = new Label();
            likutis.Foreground = (Brush)bc.ConvertFrom("#FF4BC4AA");
            likutis.Content = Math.Round(vartotojas.Saskaitos[saskaitosNr].Likutis, 2) + " €";
            likutis.Margin = new Thickness(650, -23 + moveCof, 0, 0);
            saskaituGrid.Children.Add(likutis);
            if(saskaitosNr != 0)
            {
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
                veiksmai.Margin = new Thickness(820, -23 + moveCof, 0, 0);
                veiksmai.Name = "DeleteButton_" + saskaitosNr.ToString();
                veiksmai.Click += DeleteButton;
                saskaituGrid.Children.Add(veiksmai);
            }
        }

        private async void DeleteButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            IsEnabled = false;
            Loading();
            int nr = int.Parse(button.Name[13].ToString());
            vartotojasSaved.Saskaitos[0].Likutis += vartotojasSaved.Saskaitos[nr].Likutis;
            vartotojasSaved.Saskaitos.RemoveAt(nr);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
            var manoSaskaitos = new ManoSaskaitos(vartotojasSaved, keySaved);
            manoSaskaitos.Show();
            Close();
        }

        public void CreateBillBanner()
        {
            Image background = new Image();
            background.Source = new BitmapImage(new Uri("Images/Rectangle 28.png", UriKind.Relative));
            background.Margin = new Thickness(0, -500, 0, 0);
            saskaituGrid.Children.Add(background);
            Label pavadinimas = new Label();
            pavadinimas.FontWeight = FontWeights.Bold;
            pavadinimas.Content = "Sąskaitos pavadinimas";
            pavadinimas.Margin = new Thickness(30, -23, 0, 0);
            saskaituGrid.Children.Add(pavadinimas);
            Label kodas = new Label();
            kodas.FontWeight = FontWeights.Bold;
            kodas.Content = "IBAN";
            kodas.Margin = new Thickness(250, -23, 0, 0);
            saskaituGrid.Children.Add(kodas);
            Label data = new Label();
            data.FontWeight = FontWeights.Bold;
            data.Content = "Sukūrimo data";
            data.Margin = new Thickness(450, -23, 0, 0);
            saskaituGrid.Children.Add(data);
            Label likutis = new Label();
            likutis.FontWeight = FontWeights.Bold;
            likutis.Content = "Likutis";
            likutis.Margin = new Thickness(650, -23, 0, 0);
            saskaituGrid.Children.Add(likutis);
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
    }
}
