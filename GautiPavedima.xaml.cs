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
using System.Globalization;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Pervedimas.xaml
    /// </summary>
    public partial class GautiPavedima : Window
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

        public GautiPavedima(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            SaskaitosDrop.ItemsSource = IbanArray(vartotojasSaved.Saskaitos);
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
            LikutisText.Content = "Jūsų sąskaitos likutis: " + vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis + " €";
            saskaitosPavadinimas.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Pavadinimas;
        }

        public string[] IbanArray(List<Saskaita> saskaitos)
        {
            string[] array = new string[saskaitos.Count];

            for (int i = 0; i < saskaitos.Count; i++)
            {
                array[i] = saskaitos[i].Kodas;
            }
            return array;
        }

        public int FindIbanNr(List<Saskaita> saskaitos, string Iban)
        {
            for (int i = 0; i < saskaitos.Count; i++)
            {
                if (saskaitos[i].Kodas == Iban) return i;
            }
            return 0;
        }

        private async void SaskaitosDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var response = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            if (vartotojas.Saskaitos.Count > 0)
            {
                pagrindinesSaskNr = FindIbanNr(vartotojas.Saskaitos, e.AddedItems[0].ToString());
            }
            LikutisText.Content = "Jūsų sąskaitos likutis: " + vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis + " €";
            saskaitosPavadinimas.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Pavadinimas;
            vartotojasSaved = vartotojas;
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
        async private void PavedimasButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var response = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            vartotojasSaved = vartotojas;
            var pavedimas = new Pervedimas(vartotojasSaved, keySaved);
            pavedimas.Show();
            Close();
        }

        private void PervestiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            patvirtintiBackround.Opacity = 0.8;
        }

        private void PervestiButton_MouseLeave(object sender, MouseEventArgs e)
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

        private void ValiutosButton(object sender, RoutedEventArgs e)
        {

        }

        private void PagalbaButton(object sender, RoutedEventArgs e)
        {

        }

        private void ManoButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var manoSaskaitos = new ManoSaskaitos(vartotojasSaved, keySaved);
            manoSaskaitos.Show();
            Close();
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

        void Unloading()
        {
            dotProgress1.Visibility = Visibility.Hidden;
            dotProgress2.Visibility = Visibility.Hidden;
            dotProgress3.Visibility = Visibility.Hidden;
            greyedOut.Visibility = Visibility.Hidden;
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

        private async void PervestiButton(object sender, RoutedEventArgs e)
        {
            if (gavejoTextBox.Text == "" || sumaTextBox.Text == "")
            {
                generalEventText.Content = "Prašome užpildyti visus privalomus langelius!";
                return;
            }

            if (!IsDigitsOnly(sumaTextBox.Text))
            {
                generalEventText.Content = "Sumą turi sudaryti tik skaičiai, kurie gali būti atskirti tašku!";
                return;
            }
            Loading();
            // nuskaitom paskyras is duomenu bazes
            string gavejoKey = String.Join("", Encoding.ASCII.GetBytes(gavejoTextBox.Text));
            var responseSiuntejas = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas siuntejas = responseSiuntejas.ResultAs<Vartotojas>();
            var responseGavejas = await client.GetAsync("Paskyros/" + gavejoKey);
            Vartotojas gavejas = responseGavejas.ResultAs<Vartotojas>();

            // ieškome ar egzistuoja tokia paskyra duomenų bazėje
            if (gavejas != null)
            {
                Unloading();
            }
            else
            {
                Unloading();
                generalEventText.Content = "Nėra tokio gavėjo!";
                return;
            }
            IsEnabled = false;
            Loading();
            if (siuntejas.Epastas == gavejas.Epastas)
            {
                //await client.UpdateAsync("Paskyros/" + keySaved, siuntejas);

                Pranesimas pranesimas_siuntejui = new Pranesimas(siuntejas.Vardas + " " + siuntejas.Pavarde, siuntejas.Vardas + " " + siuntejas.Pavarde, "Prašymas", double.Parse(sumaTextBox.Text, CultureInfo.InvariantCulture), DateTime.Now, siuntejas.Epastas, siuntejas.Saskaitos[pagrindinesSaskNr].Kodas, paskirtisText.Text);
                siuntejas.NaujiNotification = true;


                siuntejas.Pranesimai.Insert(0,pranesimas_siuntejui);

                await client.UpdateAsync("Paskyros/" + keySaved, siuntejas);

                vartotojasSaved = siuntejas;
                var apzvalga = new Apzvalga(vartotojasSaved, keySaved);
                apzvalga.Show();
                Close();
            }
            else
            {

                Pranesimas pranesimas_gavejui = new Pranesimas(gavejas.Vardas + " " + gavejas.Pavarde, siuntejas.Vardas + " " + siuntejas.Pavarde, "Prašymas", double.Parse(sumaTextBox.Text, CultureInfo.InvariantCulture), DateTime.Now, siuntejas.Epastas, siuntejas.Saskaitos[pagrindinesSaskNr].Kodas, paskirtisText.Text);
                gavejas.NaujiNotification = true;
                gavejas.Pranesimai.Insert(0,pranesimas_gavejui);

                await client.UpdateAsync("Paskyros/" + gavejoKey, gavejas);

                vartotojasSaved = siuntejas;
                var apzvalga = new Apzvalga(vartotojasSaved, keySaved);
                apzvalga.Show();
                Close();
            }
        }

        bool IsDigitsOnly(string suma)
        {
            int dotCount = 0;
            foreach (char c in suma)
            {
                if (!char.IsDigit(c))
                {
                    if (!c.Equals('.'))
                    {
                        return false;
                    }
                    else
                    {
                        if (dotCount == 1)
                        {
                            return false;
                        }
                        dotCount++;
                    }
                }
            }
            return true;
        }
    }
}
