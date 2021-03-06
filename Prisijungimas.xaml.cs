﻿using System;
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
using catpcha;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System.Windows.Media.Animation;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Prisijungimas.xaml
    /// </summary>
    public partial class Prisijungimas : Window
    {
        public int counter = 0;
            
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
            Loading();
            prisijungtiButton.IsEnabled = false;
            string key = String.Join("", Encoding.ASCII.GetBytes(ePastoTextBox.Text));
            // nuskaitom paskyra is duomenu bazes
            var response = await client.GetAsync("Paskyros/" + key);
            Vartotojas paskyra = response.ResultAs<Vartotojas>();

            // jei paskyra neegzistuoja
            if (paskyra == null)
            {
                Unloading();
                generalEventText.Content = "Tokia paskyra neegzistuoja";
                generalEventText.Foreground = Brushes.Red;
                generalEventText.Visibility = Visibility.Visible;
                prisijungtiButton.IsEnabled = true;
                counter++;
            }
            else
            {
                //Tikrina ar paskyra neturi buti istrinta
                if (paskyra.ToDelete == true && DateTime.Now > paskyra.Istrynimo_Data)
                {
                    client.Delete("Paskyros/" + key);
                    Unloading();
                    generalEventText.Content = "Tokia paskyra neegzistuoja";
                    generalEventText.Foreground = Brushes.Red;
                    generalEventText.Visibility = Visibility.Visible;
                    prisijungtiButton.IsEnabled = true;
                    counter++;
                }
                else
                {
                    // jei tinka slaptažodis
                    if (Security.PasswordMatch(slaptazodzioTextBox.Password, paskyra.Slaptazodis))
                    {
                        // prisijungus prie paskyros kuri neturi apsaugos
                        if (paskyra.ShortSecurityCode == 0 | paskyra.LongSecurityCode == 0)
                        {
                            var securityForm = new ApsaugosKodas();
                            securityForm.paskyra = paskyra;
                            securityForm.key = key;
                            securityForm.Show();
                            Close();
                        }
                        else
                        {
                            var apzvalga = new Apzvalga(paskyra, key);
                            generalEventText.Visibility = Visibility.Hidden;
                            prisijungtiButton.IsEnabled = true;
                            apzvalga.Show();
                            Close();
                        }
                    }
                    else
                    {
                        Unloading();
                        generalEventText.Content = "Neteisingas slaptažodis";
                        generalEventText.Foreground = Brushes.Red;
                        generalEventText.Visibility = Visibility.Visible;
                        prisijungtiButton.IsEnabled = true;
                        counter++;
                    }
                }
            }

            if(counter >= 5)
            {
                var captcha = new Captcha();
                captcha.prisijungimas = this;
                ePastoTextBox.IsReadOnly = true;
                slaptazodzioTextBox.IsEnabled = false;
                prisijungtiButton.IsEnabled = false;
                captcha.Show();
              
            }
            
        }

        private void RegisterButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
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
            Loading();
            IsEnabled = false;
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
