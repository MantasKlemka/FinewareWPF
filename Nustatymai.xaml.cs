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
    public partial class Nustatymai : Window
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

        public Nustatymai(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
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

        private void SlaptazodisButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Visible;
            patvirtintiBackround.Visibility = Visibility.Visible;
            patvirtintiButton.Visibility = Visibility.Visible;
            KeistiDescription.Text = "Užpildykite reikiamus laukus kad pakeistumėte slaptažodį";
            KeistiLabel.Content = "Slaptažodžio keitimas";
            KeistiDescription.Visibility = Visibility.Visible;
            KeistiLabel.Visibility = Visibility.Visible;
            KeistiWindow.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            kodoTextBox.Visibility = Visibility.Visible;
            kodoLabel.Visibility = Visibility.Visible;
            passLabel.Visibility = Visibility.Visible;
            PassTextBox.Visibility = Visibility.Visible;
            newPassLabel.Visibility = Visibility.Visible;
            NewPasswordTextBox.Visibility = Visibility.Visible;
            newPassAgainLabel.Visibility = Visibility.Visible;
            NewPasswordAgainTextBox.Visibility = Visibility.Visible;
            passUnderLine.Visibility = Visibility.Visible;
            newPassUnderline.Visibility = Visibility.Visible;
            kodoUnderline.Visibility = Visibility.Visible;
            newPassAgainUnderline.Visibility = Visibility.Visible;
            generalEventText.Visibility = Visibility.Visible;
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Collapsed;
            closeButton.Visibility = Visibility.Collapsed;
            patvirtintiBackround.Visibility = Visibility.Collapsed;
            patvirtintiButton.Visibility = Visibility.Collapsed;
            KeistiDescription.Visibility = Visibility.Collapsed;
            KeistiLabel.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            KeistiWindow.Visibility = Visibility.Collapsed;
            kodoTextBox.Visibility = Visibility.Collapsed;
            kodoLabel.Visibility = Visibility.Collapsed;
            passLabel.Visibility = Visibility.Collapsed;
            PassTextBox.Visibility = Visibility.Collapsed;
            newPassLabel.Visibility = Visibility.Collapsed;
            NewPasswordTextBox.Visibility = Visibility.Collapsed;
            newPassAgainLabel.Visibility = Visibility.Collapsed;
            NewPasswordAgainTextBox.Visibility = Visibility.Collapsed;
            passUnderLine.Visibility = Visibility.Collapsed;
            newPassUnderline.Visibility = Visibility.Collapsed;
            kodoUnderline.Visibility = Visibility.Collapsed;
            newPassAgainUnderline.Visibility = Visibility.Collapsed;
            generalEventText.Visibility = Visibility.Collapsed;
            kodoTextBox.Clear();
            NewPasswordAgainTextBox.Clear();
            NewPasswordTextBox.Clear();
            PassTextBox.Clear();
            generalEventText.Content = "";
        }

        private async void PateiktiButton(object sender, RoutedEventArgs e)
        {
            if (!Security.PasswordMatch(PassTextBox.Password, vartotojasSaved.Slaptazodis))
            {
                generalEventText.Content = "Neteisingas slaptažodis!";
                return;
            }
            if (kodoTextBox.Password != vartotojasSaved.ShortSecurityCode.ToString())
            {
                generalEventText.Content = "Neteisingas 4-ių skaičių kodas!";
                return;
            }
            if (NewPasswordTextBox.Password != NewPasswordAgainTextBox.Password)
            {
                generalEventText.Content = "Nauji slaptažodžiai turi sutapti!";
                return;
            }
            if (Security.PasswordMatch(NewPasswordTextBox.Password, vartotojasSaved.Slaptazodis))
            {
                generalEventText.Content = "Naujas slaptažodis negali sutapti su senuoju!";
                return;
            }

            IsEnabled = false;
            Loading();
            Panel.SetZIndex(greyedOut, 9);           
            vartotojasSaved.Slaptazodis = Security.HashingPassword(NewPasswordTextBox.Password);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
            CloseButton(sender, e);
            var nustatymai = new Nustatymai(vartotojasSaved, keySaved);
            nustatymai.Show();
            Close();
        }

        private void PatvirtintiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            patvirtintiBackround.Opacity = 0.8;
        }

        private void PatvirtintiButton_MouseLeave(object sender, MouseEventArgs e)
        {
            patvirtintiBackround.Opacity = 1;
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            closeBackground.Opacity = 0.8;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeBackground.Opacity = 1;
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

        private void GautiButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var gauti = new GautiPavedima(vartotojasSaved, keySaved);
            gauti.Show();
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
