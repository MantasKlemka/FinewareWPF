using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Registracija.xaml
    /// </summary>
    public partial class Registracija : Window
    {
        public static string vardas;
        public static string pavarde;
        public static string epastas;
        public static string slaptazodis;

        public static string projektoEpastas = "finewareproject@gmail.com";
        public static string projektoSlaptazodis = "fineware112";
        public static string randomCode;



        public Registracija()
        {
            InitializeComponent();
        }

        private async void RegisterButton(object sender, RoutedEventArgs e)
        {
            // tikrinama ar langeliai atitinka reikalavimus
            PasswordValidation_1(sender, e);
            PasswordValidation_2(sender, e);
            NameValidation(sender, e);
            SurnameValidation(sender, e);
            EmailValidation(sender, e);
            // paimamos langelių reikšmės
            vardas = vardoTextBox.Text;
            pavarde = pavardesTextBox.Text;
            epastas = ePastoTextBox.Text;
            slaptazodis = slaptazodzioTextBox_1.Password;
            // tikrinama ar nėra klaidų susijusių su langeliais
            if (!slaptazodzioError_1.IsVisible & !slaptazodzioError_2.IsVisible & !vardoError.IsVisible & !pavardesError.IsVisible & !ePastoError.IsVisible)
            {
                Random rand = new Random();
                randomCode = (rand.Next(10000, 99999)).ToString();
                try
                {
                    string messageBody = "Jūsu patvirtinimo kodas yra: " + randomCode;
                    string messageSubject = "Registracijos patvirtinimo kodas";
                    MailMessage message = SiustiLaiska.CreateMessage(epastas, projektoEpastas, messageBody, messageSubject);
                    SiustiLaiska.SendMessage(projektoEpastas, projektoSlaptazodis, message);
                    var EmailCode = new EmailPatikrinimas();
                    EmailCode.Show();
                    Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Nepavyko išsiusti kodo!");
                }

            }
        }

        // perjungiame i prisijungimo langą
        private void LoginButton(object sender, RoutedEventArgs e)
        {
            var loginForm = new Prisijungimas();
            loginForm.Show();
            Close();
        }

        private void PasswordValidation_1(object sender, RoutedEventArgs e)
        {
            if (slaptazodzioTextBox_1.Password != "")
            {
                if (slaptazodzioTextBox_1.Password != slaptazodzioTextBox_2.Password)
                {
                    slaptazodzioError_1.Content = "Slaptažodžiai nevienodi";
                    slaptazodzioError_2.Content = "Slaptažodžiai nevienodi";
                    slaptazodzioError_1.Visibility = Visibility.Visible;
                    slaptazodzioError_2.Visibility = Visibility.Visible;
                }
                else
                {
                    slaptazodzioError_1.Visibility = Visibility.Hidden;
                    slaptazodzioError_2.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                slaptazodzioError_1.Content = "Prašome užpildyti langelį";
                slaptazodzioError_1.Visibility = Visibility.Visible;
            }
        }

        private void PasswordValidation_2(object sender, RoutedEventArgs e)
        {
            if (slaptazodzioTextBox_2.Password != "")
            {
                if (slaptazodzioTextBox_1.Password != slaptazodzioTextBox_2.Password)
                {
                    slaptazodzioError_1.Content = "Slaptažodžiai nevienodi";
                    slaptazodzioError_2.Content = "Slaptažodžiai nevienodi";
                    slaptazodzioError_1.Visibility = Visibility.Visible;
                    slaptazodzioError_2.Visibility = Visibility.Visible;
                }
                else
                {
                    slaptazodzioError_1.Visibility = Visibility.Hidden;
                    slaptazodzioError_2.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                slaptazodzioError_2.Content = "Prašome užpildyti langelį";
                slaptazodzioError_2.Visibility = Visibility.Visible;
            }
        }

        private void NameValidation(object sender, RoutedEventArgs e)
        {
            if (vardoTextBox.Text != "")
            {
                // varde tik raidės ir pirma didžioji
                if (IsLineLetterOnly(vardoTextBox.Text) && Char.IsUpper(vardoTextBox.Text[0]))
                {
                    vardoError.Visibility = Visibility.Hidden;
                }
                else
                {
                    vardoError.Visibility = Visibility.Visible;
                    vardoError.Content = "Neteisingas vardo formatas";
                }
            }
            else
            {
                vardoError.Visibility = Visibility.Visible;
                vardoError.Content = "Prašome užpildyti langelį";
            }
        }

        private void SurnameValidation(object sender, RoutedEventArgs e)
        {
            if (pavardesTextBox.Text != "")
            {
                // pavardėj tik raidės ir pirma didžioji
                if (IsLineLetterOnly(pavardesTextBox.Text) && Char.IsUpper(pavardesTextBox.Text[0]))
                {
                    pavardesError.Visibility = Visibility.Hidden;
                }
                else
                {
                    pavardesError.Visibility = Visibility.Visible;
                    pavardesError.Content = "Neteisingas pavardės formatas";
                }
            }
            else
            {
                pavardesError.Visibility = Visibility.Visible;
                pavardesError.Content = "Prašome užpildyti langelį";
            }
        }

        private void EmailValidation(object sender, RoutedEventArgs e)
        {
            if (ePastoTextBox.Text != "")
            {
                ePastoError.Visibility = Visibility.Hidden;
            }
            else
            {
                ePastoError.Visibility = Visibility.Visible;
                ePastoError.Content = "Prašome užpildyti langelį";
            }
        }

        // tikrina ar eilutėje tik raidės
        bool IsLineLetterOnly(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!Char.IsLetter(text[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
