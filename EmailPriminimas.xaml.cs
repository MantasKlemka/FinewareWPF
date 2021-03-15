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

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for EmailPriminimas.xaml
    /// </summary>
    public partial class EmailPriminimas : Window
    {
        string key;
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
            client = new FireSharp.FirebaseClient(config);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            randomCode = (rand.Next(10000, 99999)).ToString();

            Vartotojas paskyra = null;
            string key = "";
            // nuskaitom paskyras is duomenu bazes
            var response = await client.GetAsync("Paskyros/");
            Dictionary<string, Vartotojas> list = response.ResultAs<Dictionary<string, Vartotojas>>();
            // ieškome ar egzistuoja tokia paskyra duomenų bazėje
            if (list != null)
            {
                paskyra = CorrectEmail(list, out key);
            }

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nepavyko išsiusti kodo!");
                }
            }
            else
            {
                MessageBox.Show("Nerasta paskyra");
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // nuskaitom paskyras is duomenu bazes
            var response = await client.GetAsync("Paskyros/");
            Dictionary<string, Vartotojas> list = response.ResultAs<Dictionary<string, Vartotojas>>();
            // ieškome ar egzistuoja tokia paskyra duomenų bazėje
            Vartotojas paskyra = CorrectEmail(list, out key);
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
        }

        public Vartotojas CorrectEmail(Dictionary<string, Vartotojas> list, out string key)
        {
            Vartotojas paskyra = null;
            // ieskome reikiamos paskyros
            foreach (var entry in list)
            {
                Vartotojas vartotojas = entry.Value;
                if (Emailtextbox.Text == vartotojas.Epastas)
                {
                    paskyra = vartotojas;
                    key = entry.Key;
                    return paskyra;
                }
            }
            key = "";
            return paskyra;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var loginForm = new Prisijungimas();
            loginForm.Show();
            Close();
        }
    }
}
