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
using Newtonsoft.Json;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Prisijungimas.xaml
    /// </summary>
    public partial class Prisijungimas : Window
    {
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
        };

        public Prisijungimas()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
        }

        private async void LoginButton(object sender, RoutedEventArgs e)
        {
            Vartotojas paskyra = null;
            string key = "";
            // nuskaitom paskyras is duomenu bazes
            var response = await client.GetAsync("Paskyros/");
            Dictionary<string, Vartotojas> list = response.ResultAs<Dictionary<string, Vartotojas>>();
            // ieškome ar egzistuoja tokia paskyra duomenų bazėje
            if(list != null)
            {
                paskyra = CorrectEmail(list, out key);
            }

            // jei paskyra neegzistuoja
            if (paskyra == null)
            {
                generalEventText.Content = "Tokia paskyra neegzistuoja";
                generalEventText.Foreground = Brushes.Red;
                generalEventText.Visibility = Visibility.Visible;
            }
            else
            {
                // jei tinka slaptažodis
                if (paskyra.Slaptazodis == slaptazodzioTextBox.Password)
                {
                    // prisijungus prie paskyros kuri neturi apsaugos
                    if(paskyra.ShortSecurityCode == 0 | paskyra.LongSecurityCode == 0)
                    {
                        var securityForm = new ApsaugosKodas();
                        securityForm.paskyra = paskyra;
                        securityForm.key = key;
                        securityForm.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Prisijungta");
                        generalEventText.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    generalEventText.Content = "Neteisingas slaptažodis";
                    generalEventText.Foreground = Brushes.Red;
                    generalEventText.Visibility = Visibility.Visible;
                }
            }
        }

        private void RegisterButton(object sender, RoutedEventArgs e)
        {
            var registerForm = new Registracija();
            registerForm.Show();
            Close();
        }

        // tikrina ir gražina paskyra jei ji yra duomenų bazėje
        public Vartotojas CorrectEmail(Dictionary<string, Vartotojas> list, out string key)
        {
            Vartotojas paskyra = null;
            // ieskome reikiamos paskyros
            foreach (var entry in list)
            {
                Vartotojas vartotojas = entry.Value;
                if (ePastoTextBox.Text == vartotojas.Epastas)
                {
                    paskyra = vartotojas;
                    key = entry.Key;
                    return paskyra;
                }
            }
            key = "";
            return paskyra;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sw = new EmailPriminimas();
            sw.Show();
            Close();
        }

        private void PrisijungtiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            prisijungtiLabel.Opacity = 0.5;
            prisijungtiBackground.Opacity = 0.8;

        }

        private void PrisijungtiButton_MouseLeave(object sender, MouseEventArgs e)
        {
            prisijungtiLabel.Opacity = 1;
            prisijungtiBackground.Opacity = 1;
        }

        private void RegistruotisButton_MouseEnter(object sender, MouseEventArgs e)
        {
            registruotisButton.Opacity = 0.5;
        }

        private void RegistruotisButton_MouseLeave(object sender, MouseEventArgs e)
        {
            registruotisButton.Opacity = 1;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            pamirsaiButton.Opacity = 0.5;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            pamirsaiButton.Opacity = 1;
        }
    }
}
