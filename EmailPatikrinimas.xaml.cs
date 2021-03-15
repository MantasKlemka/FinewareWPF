using FireSharp.Config;
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
            if (randomCode == EmailBox.Text)
            {
                //sukuriamas vartotojas
                Vartotojas vartotojas = new Vartotojas(Registracija.vardas, Registracija.pavarde, Registracija.epastas, Registracija.slaptazodis);
                //vartotojas ikeliamas į duomenų bazę
                PushResponse response = await client.PushAsync("Paskyros/", vartotojas);
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
                generalEventText.Content = "Neteisingas kodas!";
                generalEventText.Foreground = Brushes.Red;
                generalEventText.Visibility = Visibility.Visible;
                patvirtintiButton.IsEnabled = true;
            }
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
    }
}
