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
using System.Net;
using System.Net.Mail;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Pervedimas.xaml
    /// </summary>
    public partial class Nustatymai : Window
    {
        Vartotojas vartotojasSaved;
        string keySaved;
        public static string projektoEpastas = "finewareproject@gmail.com";
        public static string projektoSlaptazodis = "fineware112";
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
            UzkrautiCheckboxPasirinkimus();
            if(vartotojasSaved.ToDelete == false)
            {
                Atsaukti.IsEnabled = false;
                Istrinti.IsEnabled = true;
                IstrynimopPranesimas.Visibility = Visibility.Hidden;
            }
            else
            {
                Atsaukti.IsEnabled = true;
                Istrinti.IsEnabled = false;
                IstrynimopPranesimas.Content = "Ši paskyra bus ištrinta " + vartotojasSaved.Istrynimo_Data.ToShortDateString();
                IstrynimopPranesimas.Foreground = Brushes.Red;
                IstrynimopPranesimas.Visibility = Visibility.Visible;
            }
            imageComboBox.SelectedIndex = vartotojas.AvatarIndex;
            avatarIcon.Source = new BitmapImage(new Uri("Images/Avatars/avatar" + vartotojas.AvatarIndex + ".png", UriKind.Relative));

            // Užkraunama 'profilio informacija' lentelės informacija
            vardasTextbox.Text = vartotojas.Vardas;
            pavardeTextbox.Text = vartotojas.Pavarde;
            salisTextbox.Text = vartotojas.Salis;
            adresasTextbox.Text = vartotojas.Adresas;
            telefononumerisTextbox.Text = vartotojas.TelefonoNumeris;

            //taupymas
            SutaupytaText.Text = vartotojasSaved.Sutaupyta_suma.ToString() + " €";
        }

        private async void IssaugotiPranesimus_Click(object sender, RoutedEventArgs e)
        {
            Loading();

            if (PajamuGavimas.IsChecked == true)
                vartotojasSaved.Notification_Gavimas = true;
            else vartotojasSaved.Notification_Gavimas = false;

            if (PajamuIssiuntimas.IsChecked == true)
                vartotojasSaved.Notification_Siuntimas = true;
            else vartotojasSaved.Notification_Siuntimas = false;

            if (PrasymuGavimas.IsChecked == true)
                vartotojasSaved.Notification_Prasymas = true;
            else vartotojasSaved.Notification_Prasymas = false;
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);

            Unloading();
        }

        public void UzkrautiCheckboxPasirinkimus()
        {
            if (vartotojasSaved.Notification_Gavimas == true)
                PajamuGavimas.IsChecked = true;

            if (vartotojasSaved.Notification_Prasymas == true)
                PrasymuGavimas.IsChecked = true;

            if (vartotojasSaved.Notification_Siuntimas == true)
                PajamuIssiuntimas.IsChecked = true;

            if (vartotojasSaved.Taupyti == true)
                Taupymas.IsChecked = true;
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

        private void MinSumaButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Visible;
            closeButton2.Visibility = Visibility.Visible;
            KeistiMinWindow.Visibility = Visibility.Visible;
            KeistiLabel2.Visibility = Visibility.Visible;
            KeistiDescription2.Visibility = Visibility.Visible;
            patvirtintiButton2.Visibility = Visibility.Visible;
            patvirtintiBackround2.Visibility = Visibility.Visible;
            LimitText.Visibility = Visibility.Visible;
            MinTextBox.Visibility = Visibility.Visible;
            MinTextBox.Text = vartotojasSaved.MinSuma.ToString();
            generalEventText2.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            Border.Visibility = Visibility.Visible;
        }

        private void EmailButton(object sender, RoutedEventArgs e)
        {
            closeBackground3.Visibility = Visibility.Visible;
            closeButton3.Visibility = Visibility.Visible;
            patvirtintiBackround3.Visibility = Visibility.Visible;
            patvirtintiButton3.Visibility = Visibility.Visible;
            KeistiDescription3.Text = "Užpildykite reikiamus laukus kad pakeistumėte El. paštą";
            KeistiLabel3.Content = "El. pašto keitimas";
            KeistiDescription3.Visibility = Visibility.Visible;
            KeistiLabel3.Visibility = Visibility.Visible;
            KeistiEmailWindow.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            kodoTextBox3.Visibility = Visibility.Visible;
            kodoLabel3.Visibility = Visibility.Visible;
            passLabel3.Visibility = Visibility.Visible;
            PassTextBox3.Visibility = Visibility.Visible;
            newEmailLabel.Visibility = Visibility.Visible;
            NewEmailTextBox.Visibility = Visibility.Visible;
            passUnderLine3.Visibility = Visibility.Visible;
            newEmailUnderline.Visibility = Visibility.Visible;
            kodoUnderline3.Visibility = Visibility.Visible;
            generalEventText3.Visibility = Visibility.Visible;
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

        private void CloseButton2(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Collapsed;
            closeButton2.Visibility = Visibility.Collapsed;
            KeistiMinWindow.Visibility = Visibility.Collapsed;
            KeistiLabel2.Visibility = Visibility.Collapsed;
            KeistiDescription2.Visibility = Visibility.Collapsed;
            patvirtintiButton2.Visibility = Visibility.Collapsed;
            patvirtintiBackround2.Visibility = Visibility.Collapsed;
            LimitText.Visibility = Visibility.Collapsed;
            MinTextBox.Visibility = Visibility.Collapsed;
            generalEventText2.Content = "";
            greyedOut.Visibility = Visibility.Collapsed;
            Border.Visibility = Visibility.Collapsed;
        }

        private void CloseButton3(object sender, RoutedEventArgs e)
        {
            closeBackground3.Visibility = Visibility.Collapsed;
            closeButton3.Visibility = Visibility.Collapsed;
            patvirtintiBackround3.Visibility = Visibility.Collapsed;
            patvirtintiButton3.Visibility = Visibility.Collapsed;
            KeistiDescription3.Visibility = Visibility.Collapsed;
            KeistiLabel3.Visibility = Visibility.Collapsed;
            KeistiDescription3.Visibility = Visibility.Collapsed;
            KeistiLabel3.Visibility = Visibility.Collapsed;
            KeistiEmailWindow.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            kodoTextBox3.Visibility = Visibility.Collapsed;
            kodoLabel3.Visibility = Visibility.Collapsed;
            passLabel3.Visibility = Visibility.Collapsed;
            PassTextBox3.Visibility = Visibility.Collapsed;
            newEmailLabel.Visibility = Visibility.Collapsed;
            NewEmailTextBox.Visibility = Visibility.Collapsed;
            passUnderLine3.Visibility = Visibility.Collapsed;
            newEmailUnderline.Visibility = Visibility.Collapsed;
            kodoUnderline3.Visibility = Visibility.Collapsed;
            generalEventText3.Visibility = Visibility.Collapsed;
            kodoTextBox3.Clear();
            NewEmailTextBox.Clear();
            PassTextBox3.Clear();
            generalEventText3.Content = "";
        }
        private void CloseButton4(object sender, RoutedEventArgs e)
        {
            closeBackground4.Visibility = Visibility.Collapsed;
            closeButton4.Visibility = Visibility.Collapsed;
            patvirtintiBackround4.Visibility = Visibility.Collapsed;
            patvirtintiButton4.Visibility = Visibility.Collapsed;
            TrintiDescription.Visibility = Visibility.Collapsed;
            TrintiLabel.Visibility = Visibility.Collapsed;
            DeleteWindow.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            passLabel4.Visibility = Visibility.Collapsed;
            PassTextBox4.Visibility = Visibility.Collapsed;
            passUnderLine4.Visibility = Visibility.Collapsed;
            generalEventText4.Visibility = Visibility.Collapsed;
            PassTextBox4.Clear();
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

        private async void PateiktiButton2(object sender, RoutedEventArgs e)
        {
            if (MinTextBox.Text == "")
            {
                generalEventText2.Content = "Būtina įvesti limitą! (0 - 1 000 000)";
                return;
            }

            if (!IsDigitsOnly(MinTextBox.Text))
            {
                generalEventText2.Content = "Būtina pateikti tik skaičius!";
                return;
            }

            if (MinTextBox.Text.Length > 6)
            {
                generalEventText2.Content = "1 000 000 - yra maximalus limitas!";
                return;
            }

            if (MinTextBox.Text.Contains("."))
            {
                generalEventText2.Content = "Būtina įvesti sveiką skaičių!";
                return;
            }

            IsEnabled = false;
            Loading();
            Panel.SetZIndex(greyedOut, 9);
            vartotojasSaved.MinSuma = Convert.ToInt32(MinTextBox.Text);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
            CloseButton2(sender, e);
            var nustatymai = new Nustatymai(vartotojasSaved, keySaved);
            nustatymai.Show();
            Close();
        }

        private void PateiktiButton3(object sender, RoutedEventArgs e)
        {
            if (!Security.PasswordMatch(PassTextBox3.Password, vartotojasSaved.Slaptazodis))
            {
                generalEventText3.Content = "Neteisingas slaptažodis!";
                return;
            }
            if (kodoTextBox3.Password != vartotojasSaved.ShortSecurityCode.ToString())
            {
                generalEventText3.Content = "Neteisingas 4-ių skaičių kodas!";
                return;
            }
            if (NewEmailTextBox.Text == vartotojasSaved.Epastas)
            {
                generalEventText3.Content = "Naujas El. paštas negali sutapti su senuoju!";
                return;
            }
            if (NewEmailTextBox.Text == "")
            {
                generalEventText3.Content = "Tusčias el. pašto laukas";
                return;
            }

            IsEnabled = false;
            Loading();
            Panel.SetZIndex(greyedOut, 9);
            Random rand = new Random();
            string randomCode = (rand.Next(10000, 99999)).ToString();
            string messageBody = "Jūsu el. pašto pakeitimo kodas yra : " + randomCode;
            string messageSubject = "El. pašto pakeitimo kodas";
            MailMessage message = SiustiLaiska.CreateMessage(NewEmailTextBox.Text, projektoEpastas, messageBody, messageSubject);
            SiustiLaiska.SendMessage(projektoEpastas, projektoSlaptazodis, message);

            var EmailCode = new EmailPatikrinimas2(vartotojasSaved, keySaved, NewEmailTextBox.Text);
            EmailCode.generalEventText.Content = "Išsiunteme jums kodą į El. paštą!";
            EmailCode.generalEventText.Foreground = Brushes.Green;
            EmailCode.generalEventText.Visibility = Visibility.Visible;
            EmailCode.randomCode = randomCode;
            EmailCode.Show();
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
        private void IstrintiButton(object sender, RoutedEventArgs e)
        {
            closeBackground4.Visibility = Visibility.Visible;
            closeButton4.Visibility = Visibility.Visible;
            patvirtintiBackround4.Visibility = Visibility.Visible;
            patvirtintiButton4.Visibility = Visibility.Visible;
            TrintiDescription.Text = "Užpildykite reikiamus laukus kad patvirtumente paskyros ištrynimą";
            TrintiLabel.Content = "Paskyros ištrynimas";
            TrintiDescription.Visibility = Visibility.Visible;
            TrintiLabel.Visibility = Visibility.Visible;
            DeleteWindow.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            passLabel4.Visibility = Visibility.Visible;
            PassTextBox4.Visibility = Visibility.Visible;
            passUnderLine4.Visibility = Visibility.Visible;
            generalEventText4.Visibility = Visibility.Visible;
        }


        private async void PateiktiButton4(object sender, RoutedEventArgs e)
        {
            if (!Security.PasswordMatch(PassTextBox4.Password, vartotojasSaved.Slaptazodis))
            {
                generalEventText.Content = "Neteisingas slaptažodis!";
                return;
            }

            IsEnabled = false;
            Loading();
            Panel.SetZIndex(greyedOut, 9);

            vartotojasSaved.ToDelete = true;
            vartotojasSaved.Istrynimo_Data = DateTime.Now.AddDays(30);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);

            CloseButton4(sender, e);
            var nustatymai = new Nustatymai(vartotojasSaved, keySaved);
            nustatymai.Show();
            Close();
        }

        private async void AtsauktiButton(object sender, RoutedEventArgs e)
        {
            Atsaukti.IsEnabled = false;
            Istrinti.IsEnabled = true;
            vartotojasSaved.ToDelete = false;
            vartotojasSaved.Istrynimo_Data = DateTime.Now;
            IstrynimopPranesimas.Content = "Paskyros ištrynimas atšauktas";
            IstrynimopPranesimas.Foreground = Brushes.Red;
            IstrynimopPranesimas.Visibility = Visibility.Visible;
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = imageComboBox.SelectedIndex;
            string choosen = "Images/Avatars/avatar" + Convert.ToInt32(selectedIndex) + ".png";
            localImage.Source = new BitmapImage(new Uri(choosen, UriKind.Relative));
        }

        private async void KeistiDuomenisButton(object sender, RoutedEventArgs e)
        {
            if (vardasTextbox.Text != "" && pavardeTextbox.Text != "")
            {
                Loading();
                IsEnabled = false;
                vartotojasSaved.AvatarIndex = imageComboBox.SelectedIndex;
                vartotojasSaved.Vardas = vardasTextbox.Text;
                vartotojasSaved.Pavarde = pavardeTextbox.Text;
                vartotojasSaved.Salis = salisTextbox.Text;
                vartotojasSaved.Adresas = adresasTextbox.Text;
                vartotojasSaved.TelefonoNumeris = telefononumerisTextbox.Text;
                await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
                var nustatymai = new Nustatymai(vartotojasSaved, keySaved);
                nustatymai.Show();
                Close();
            }
            else if(vardasTextbox.Text == "" && pavardeTextbox.Text == "")
            {
                klaidaLabel.Content = "Privalote įrašyti vartotojo vardą ir pavardę!";
            }
            else if (vardasTextbox.Text != "" && pavardeTextbox.Text == "")
            {
                klaidaLabel.Content = "Privalote įrašyti vartotojo pavardę!";
            }
            else if (vardasTextbox.Text == "" && pavardeTextbox.Text != "")
            {
                klaidaLabel.Content = "Privalote įrašyti vartotojo vardą!";
            }
        }

        private async void Atsiimti_Click(object sender, RoutedEventArgs e)
        {
            Loading();

            vartotojasSaved.Saskaitos[0].Likutis += vartotojasSaved.Sutaupyta_suma;
            vartotojasSaved.Sutaupyta_suma = 0;
            SutaupytaText.Text = vartotojasSaved.Sutaupyta_suma.ToString() + " €";

            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);

            Unloading();
        }

        private async void Patvirtinti_Click(object sender, RoutedEventArgs e)
        {
            Loading();

            if (Taupymas.IsChecked == true)
                vartotojasSaved.Taupyti = true;
            else vartotojasSaved.Taupyti = false;

            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);

            Unloading();
        }
    }
}