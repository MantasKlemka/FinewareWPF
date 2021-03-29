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
    public partial class Pervedimas : Window
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

        public Pervedimas(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            SaskaitosDrop.ItemsSource = IbanArray(vartotojasSaved.Saskaitos);
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
            LikutisText.Content = "Sąskaitos likutis: " + vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis;
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
            LikutisText.Content = "Sąskaitos likutis: " + vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis;
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

        }

        private void ApzvalgaButton(object sender, RoutedEventArgs e)
        {
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

        private async void PervestiButton(object sender, RoutedEventArgs e)
        {
            if(gavejoTextBox.Text == "" || saskaitosTextBox.Text == "" || sumaTextBox.Text == "" || kodoTextBox.Password == "")
            {
                generalEventText.Content = "Prašome užpildyti visus privalomus langelius!";
                return;
            }

            if (kodoTextBox.Password == vartotojasSaved.ShortSecurityCode.ToString())
            {
                if (!IsDigitsOnly(sumaTextBox.Text))
                {
                    generalEventText.Content = "Sumą turi sudaryti tik skaičiai, kurie gali būti atskirti kableliu!";
                    return;
                }
                if (double.Parse(sumaTextBox.Text) <= vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis)
                {
                    // nuskaitom paskyras is duomenu bazes
                    var response = await client.GetAsync("Paskyros/");
                    Dictionary<string, Vartotojas> list = response.ResultAs<Dictionary<string, Vartotojas>>();
                    var responseSiuntejas = await client.GetAsync("Paskyros/" + keySaved);
                    Vartotojas siuntejas = responseSiuntejas.ResultAs<Vartotojas>();
                    Vartotojas gavejas = null;
                    string gavejoKey = "";
                    int gavejoSaskaitosNr = -1;
                    // ieškome ar egzistuoja tokia paskyra duomenų bazėje
                    if (list != null)
                    {
                        gavejas = GetUser(list, out gavejoKey);
                    }
                    else
                    {
                        generalEventText.Content = "Nėra tokio gavėjo!";
                        return;
                    }
                    if (gavejas != null)
                    {
                        gavejoSaskaitosNr = GetBillNumber(gavejas);

                    }
                    else
                    {
                        generalEventText.Content = "Nėra tokio gavėjo!";
                        return;
                    }
                    if (gavejoSaskaitosNr != -1)
                    {
                        if (siuntejas.Epastas == gavejas.Epastas)
                        {
                            siuntejas.Saskaitos[pagrindinesSaskNr].Likutis -= double.Parse(sumaTextBox.Text);
                            siuntejas.Saskaitos[pagrindinesSaskNr].Likutis = Math.Round(siuntejas.Saskaitos[pagrindinesSaskNr].Likutis, 2);
                            siuntejas.Saskaitos[gavejoSaskaitosNr].Likutis += double.Parse(sumaTextBox.Text);
                            siuntejas.Saskaitos[pagrindinesSaskNr].Likutis = Math.Round(siuntejas.Saskaitos[pagrindinesSaskNr].Likutis, 2);
                            await client.UpdateAsync("Paskyros/" + keySaved, siuntejas);
                            vartotojasSaved = siuntejas;
                            var apzvalga = new Apzvalga(vartotojasSaved, keySaved);
                            apzvalga.Show();
                            Close();
                        }
                        else
                        {
                            siuntejas.Saskaitos[pagrindinesSaskNr].Likutis -= double.Parse(sumaTextBox.Text);
                            siuntejas.Saskaitos[pagrindinesSaskNr].Likutis = Math.Round(siuntejas.Saskaitos[pagrindinesSaskNr].Likutis, 2);
                            gavejas.Saskaitos[gavejoSaskaitosNr].Likutis += double.Parse(sumaTextBox.Text);
                            siuntejas.Saskaitos[pagrindinesSaskNr].Likutis = Math.Round(siuntejas.Saskaitos[pagrindinesSaskNr].Likutis, 2);
                            await client.UpdateAsync("Paskyros/" + keySaved, siuntejas);
                            await client.UpdateAsync("Paskyros/" + gavejoKey, gavejas);
                            vartotojasSaved = siuntejas;
                            var apzvalga = new Apzvalga(vartotojasSaved, keySaved);
                            apzvalga.Show();
                            Close();
                        }
                    }
                    else
                    {
                        generalEventText.Content = "Nėra tokio gavėjo sąskaitos!";
                        return;
                    }
                }
                else
                {
                    generalEventText.Content = "Nepakankamas sąskaitos likutis!";
                    return;
                }
            }
            else
            {
                generalEventText.Content = "Neteisingas 4-ių skaičių kodas!";
                return;
            }
        }

        // tikrina ir gražina Vartotoją jei ji yra duomenų bazėje
        public Vartotojas GetUser(Dictionary<string, Vartotojas> list, out string key)
        {
            Vartotojas paskyra = null;
            // ieskome reikiamos paskyros
            foreach (var entry in list)
            {
                Vartotojas vartotojas = entry.Value;
                if (gavejoTextBox.Text == vartotojas.Epastas)
                {
                    paskyra = vartotojas;
                    key = entry.Key;
                    return paskyra;
                }
            }
            key = "";
            return paskyra;
        }

        public int GetBillNumber(Vartotojas gavejas)
        {
            for(int i = 0; i < gavejas.Saskaitos.Count; i++)
            {
                if (gavejas.Saskaitos[i].Kodas == saskaitosTextBox.Text)
                {
                    return i;
                }
            }
            return -1;
        }

        bool IsDigitsOnly(string suma)
        {
            int commaCount = 0;
            foreach (char c in suma)
            {
                if (!char.IsDigit(c))
                {
                    if (!c.Equals(','))
                    {
                        return false;
                    }
                    else
                    {
                        if (commaCount == 1)
                        {
                            return false;
                        }
                        commaCount++;
                    }
                }
            }
            return true;
        }
    }
}
