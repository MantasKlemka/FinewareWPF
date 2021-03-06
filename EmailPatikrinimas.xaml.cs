﻿using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
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
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Windows.Media.Animation;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for EmailPatikrinimas.xaml
    /// </summary>
    public partial class EmailPatikrinimas : Window
    {
        public string randomCode;
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
        };

        public EmailPatikrinimas()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            patvirtintiButton.IsEnabled = false;
            Loading();
            if (randomCode == EmailBox.Text)
            {
                //sukuriamas vartotojas
                List<Saskaita> saskaitos = new List<Saskaita>();
                string email = String.Join("", Encoding.ASCII.GetBytes(Registracija.epastas));
                Saskaita saskaita = new Saskaita("Pagrindinė sąskaita", CreateIban(), 0, DateTime.Now.Date);
                saskaitos.Add(saskaita);
                Vartotojas vartotojas = new Vartotojas(Registracija.vardas, Registracija.pavarde, Registracija.epastas, Registracija.slaptazodis, saskaitos);
                //vartotojas ikeliamas į duomenų bazę
                SetResponse response = await client.SetAsync("Paskyros/" + email, vartotojas);
                //perjungiame į prisijungimo langą
                var loginForm = new Prisijungimas();
                loginForm.generalEventText.Content = "Paskyra sukurta, galite prisijungti!";
                loginForm.generalEventText.Foreground = Brushes.Green;
                loginForm.generalEventText.Visibility = Visibility.Visible;
                loginForm.Show();
                Close();
            }
            else
            {
                Unloading();
                generalEventText.Content = "Neteisingas kodas!";
                generalEventText.Foreground = Brushes.Red;
                generalEventText.Visibility = Visibility.Visible;
                patvirtintiButton.IsEnabled = true;
            }
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

        private void PatvirtintiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            patvirtintiLabel.Opacity = 0.5;
            patvirtintiBackround.Opacity = 0.8;

        }

        private void PatvirtintiButton_MouseLeave(object sender, MouseEventArgs e)
        {
            patvirtintiLabel.Opacity = 1;
            patvirtintiBackround.Opacity = 1;
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
