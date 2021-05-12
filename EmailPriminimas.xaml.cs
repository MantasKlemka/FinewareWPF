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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Net;
using System.Net.Mail;
using System.Windows.Media.Animation;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for EmailPriminimas.xaml
    /// </summary>
    public partial class EmailPriminimas : Window
    {
        string randomCode;
        string email;
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
        };

        public EmailPriminimas()
        {
            InitializeComponent();
            lab2.Visibility = Visibility.Hidden;
            codetextbox.Visibility = Visibility.Hidden;
            buttoncode.Visibility = Visibility.Hidden;
            underLine.Visibility = Visibility.Hidden;
            client = new FireSharp.FirebaseClient(config);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            randomCode = (rand.Next(10000, 99999)).ToString();
            Loading();
            string key = String.Join("", Encoding.ASCII.GetBytes(ValidEmail(Emailtextbox.Text)));
            // nuskaitom paskyra is duomenu bazes
            var response = await client.GetAsync("Paskyros/" + key);
            Vartotojas paskyra = response.ResultAs<Vartotojas>();

            // jei paskyra egzistuoja
            if (paskyra != null)
            {
                try
                {
                    email = (Emailtextbox.Text).ToString();
                    string messageBody = "Jūsu patvirtinimo kodas yra: " + randomCode;
                    string messageSubject = "Slaptažodžio pakeitimo kodas";
                    MailMessage message = SiustiLaiska.CreateMessage(email, Registracija.projektoEpastas, messageBody, messageSubject);
                    SiustiLaiska.SendMessage(Registracija.projektoEpastas, Registracija.projektoSlaptazodis, message);
                    lab2.Visibility = Visibility.Visible;
                    codetextbox.Visibility = Visibility.Visible;
                    buttoncode.Visibility = Visibility.Visible;
                    underLine.Visibility = Visibility.Visible;
                    Unloading();
                }
                catch
                {
                    generalEventText.Content = "Nepavyko išsiusti kodo!";
                    Unloading();
                }
            }
            else
            {
                generalEventText.Content = "Nerasta paskyra";
                Unloading();
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Loading();
            // nuskaitom paskyra is duomenu bazes
            string key = String.Join("", Encoding.ASCII.GetBytes(ValidEmail(Emailtextbox.Text)));
            var response = await client.GetAsync("Paskyros/" + key);
            Vartotojas paskyra = response.ResultAs<Vartotojas>();
            // ieškome ar egzistuoja tokia paskyra duomenų bazėje
            if(randomCode == codetextbox.Text)
            {
                string newPass = Security.RandomPassword();
                string messageBody = "Jūsu naujas slaptažodis: " + newPass;
                string messageSubject = "Naujas slaptažodis";
                MailMessage message = SiustiLaiska.CreateMessage(email, Registracija.projektoEpastas, messageBody, messageSubject);
                SiustiLaiska.SendMessage(Registracija.projektoEpastas, Registracija.projektoSlaptazodis, message);
                newPass = Security.HashingPassword(newPass);
                paskyra.Slaptazodis = newPass;
                FirebaseResponse update = await client.UpdateAsync("Paskyros/" + key.ToString(), paskyra);
                var prisijungimas = new Prisijungimas();
                prisijungimas.generalEventText.Content = "Slaptažodis pakeistas, jį gausite paštu!";
                prisijungimas.generalEventText.Foreground = Brushes.Green;
                prisijungimas.generalEventText.Visibility = Visibility.Visible;
                prisijungimas.Show();
                Close();
            }
            else
            {
                generalEventText.Content = "Neteisingas kodas!";
                Unloading();
            }
        }

        public static string ValidEmail(string email)
        {
            string emailName = "";
            for (int i = 0; i < email.Length; i++)
            {
                if (!email[i].Equals('@'))
                {
                    emailName += email[i];
                }
                else
                {
                    emailName = emailName.Replace(".", "");
                    emailName += email.Substring(i, email.Length - i);
                    i = email.Length;
                }
            }
            return emailName;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var loginForm = new Prisijungimas();
            loginForm.Show();
            Close();
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

        void Unloading()
        {
            dotProgress1.Visibility = Visibility.Hidden;
            dotProgress2.Visibility = Visibility.Hidden;
            dotProgress3.Visibility = Visibility.Hidden;
            greyedOut.Visibility = Visibility.Hidden;
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
