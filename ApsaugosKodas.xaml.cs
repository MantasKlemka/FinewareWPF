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

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for ApsaugosKodas.xaml
    /// </summary>
    public partial class ApsaugosKodas : Window
    {
        public Vartotojas paskyra { get; set; }
        public string key { get; set; }

        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
        };
        public ApsaugosKodas()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
        }

        private async void CreateButton(object sender, RoutedEventArgs e)
        {
            // tikrina ar įvesti kodai atitinka reikalavimus
            FourDigitValidation(sender, e);
            SixDigitValidation(sender, e);
            // tikrina ar nėra klaidų pranešimų dėl kodų
            if (!fourDigitError.IsVisible & !sixDigitError.IsVisible)
            {
                // paskyrai priskiriami kodai
                paskyra.ShortSecurityCode = Int32.Parse(fourDigitTextBox.Password);
                paskyra.LongSecurityCode = Int32.Parse(sixDigitTextBox.Password);
                // paskyra atnaujinama duomenų bazėje
                FirebaseResponse response = await client.UpdateAsync("Paskyros/" + key.ToString(), paskyra);
                Close();
                MessageBox.Show("Prisijungta");
            }
        }

        private void FourDigitValidation(object sender, RoutedEventArgs e)
        {
            if (fourDigitTextBox.Password != fourDigitTextBox_2.Password)
            {
                fourDigitError.Visibility = Visibility.Visible;
                fourDigitError.Text = "Kodai turi sutapti";
            }
            else if(fourDigitTextBox.Password.Length != 4)
            {
                fourDigitError.Visibility = Visibility.Visible;
                fourDigitError.Text = "Kodas turi būti 4-ių skaičių";
            }
            else if (!fourDigitTextBox.Password.All(char.IsDigit))
            {
                fourDigitError.Visibility = Visibility.Visible;
                fourDigitError.Text = "Kodą turi sudaryti tik skaičiai";
            }
            else
            {
                fourDigitError.Visibility = Visibility.Hidden;
            }
        }

        private void SixDigitValidation(object sender, RoutedEventArgs e)
        {
            if (sixDigitTextBox.Password != sixDigitTextBox_2.Password)
            {
                sixDigitError.Visibility = Visibility.Visible;
                sixDigitError.Text = "Kodai turi sutapti";
            }
            else if (sixDigitTextBox.Password.Length != 6)
            {
                sixDigitError.Visibility = Visibility.Visible;
                sixDigitError.Text = "Kodas turi būti 6-ių skaičių";
            }
            else if (!sixDigitTextBox.Password.All(char.IsDigit))
            {
                sixDigitError.Visibility = Visibility.Visible;
                sixDigitError.Text = "Kodą turi sudaryti tik skaičiai";
            }
            else
            {
                sixDigitError.Visibility = Visibility.Hidden;
            }
        }

    }
}
