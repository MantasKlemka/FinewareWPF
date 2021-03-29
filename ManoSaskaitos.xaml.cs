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
using System.Windows.Media.Animation;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Pervedimas.xaml
    /// </summary>
    public partial class ManoSaskaitos : Window
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

        public ManoSaskaitos(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            keySaved = key;
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
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

        private void SukurtiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            sukurtiBackround.Opacity = 0.8;
        }

        private void SukurtiButton_MouseLeave(object sender, MouseEventArgs e)
        {
            sukurtiBackround.Opacity = 1;
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            closeBackground.Opacity = 0.8;
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            closeBackground.Opacity = 1;
        }

        private void PatvirtintiButton_MouseEnter(object sender, MouseEventArgs e)
        {
            patvirtintiBackround.Opacity = 0.8;
        }

        private void PatvirtintiButton_MouseLeave(object sender, MouseEventArgs e)
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

        private void PavedimasButton(object sender, RoutedEventArgs e)
        {
            var pervedimas = new Pervedimas(vartotojasSaved, keySaved);
            pervedimas.Show();
            Close();
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


        private async void PateiktiButton(object sender, RoutedEventArgs e)
        {
            if(kodoTextBox.Password != vartotojasSaved.ShortSecurityCode.ToString())
            {
                return;
            }
            if(pavadinimoTextBox.Text == "")
            {
                return;
            }

            Saskaita naujaSaskaita = new Saskaita(pavadinimoTextBox.Text, CreateIban(), 0, DateTime.Now.Date);
            vartotojasSaved.Saskaitos.Add(naujaSaskaita);
            await client.UpdateAsync("Paskyros/" + keySaved, vartotojasSaved);
            closeBackground.Visibility = Visibility.Collapsed;
            closeButton.Visibility = Visibility.Collapsed;
            patvirtintiBackround.Visibility = Visibility.Collapsed;
            patvirtintiButton.Visibility = Visibility.Collapsed;
            SukurimasDescription.Visibility = Visibility.Collapsed;
            SukurtiLabel.Visibility = Visibility.Collapsed;
            sukurtiWindow.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            kodoTextBox.Visibility = Visibility.Collapsed;
            kodoDescription.Visibility = Visibility.Collapsed;
            pavadinimoDescription.Visibility = Visibility.Collapsed;
            pavadinimoTextBox.Visibility = Visibility.Collapsed;
            underLine_1.Visibility = Visibility.Collapsed;
            underLine_2.Visibility = Visibility.Collapsed;
            kodoTextBox.Clear();
            pavadinimoTextBox.Clear();
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Collapsed;
            closeButton.Visibility = Visibility.Collapsed;
            patvirtintiBackround.Visibility = Visibility.Collapsed;
            patvirtintiButton.Visibility = Visibility.Collapsed;
            SukurimasDescription.Visibility = Visibility.Collapsed;
            SukurtiLabel.Visibility = Visibility.Collapsed;
            sukurtiWindow.Visibility = Visibility.Collapsed;
            greyedOut.Visibility = Visibility.Collapsed;
            kodoTextBox.Visibility = Visibility.Collapsed;
            kodoDescription.Visibility = Visibility.Collapsed;
            pavadinimoDescription.Visibility = Visibility.Collapsed;
            pavadinimoTextBox.Visibility = Visibility.Collapsed;
            underLine_1.Visibility = Visibility.Collapsed;
            underLine_2.Visibility = Visibility.Collapsed;
            kodoTextBox.Clear();
            pavadinimoTextBox.Clear();

        }

        private void SukurtiNaujaButton(object sender, RoutedEventArgs e)
        {
            closeBackground.Visibility = Visibility.Visible;
            closeButton.Visibility = Visibility.Visible;
            patvirtintiBackround.Visibility = Visibility.Visible;
            patvirtintiButton.Visibility = Visibility.Visible;
            SukurimasDescription.Visibility = Visibility.Visible;
            SukurtiLabel.Visibility = Visibility.Visible;
            sukurtiWindow.Visibility = Visibility.Visible;
            greyedOut.Visibility = Visibility.Visible;
            kodoTextBox.Visibility = Visibility.Visible;
            kodoDescription.Visibility = Visibility.Visible;
            pavadinimoDescription.Visibility = Visibility.Visible;
            pavadinimoTextBox.Visibility = Visibility.Visible;
            underLine_1.Visibility = Visibility.Visible;
            underLine_2.Visibility = Visibility.Visible;
        }
    }
}
