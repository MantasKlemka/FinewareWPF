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
using System.Windows.Media.Animation;

namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Apzvalga.xaml
    /// </summary>
    public partial class Apzvalga : Window
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

        public Apzvalga(Vartotojas vartotojas, string key)
        {
            client = new FireSharp.FirebaseClient(config);
            keySaved = key;
            InitializeComponent();
            vartotojasSaved = vartotojas;
            pagrindinesSaskNr = 0;
            SaskaitosDrop.ItemsSource = IbanArray(vartotojasSaved.Saskaitos);
            LikutisText.Text = vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis + " €";
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            emailText.Text = vartotojasSaved.Epastas;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            vartotojasSaved = vartotojas;
            if(vartotojas.Saskaitos.Count > 0)
            {
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 1;
                animation.To = 0.5;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                animation.AutoReverse = true;
                LikutisText.BeginAnimation(OpacityProperty, animation);
                LikutisText.Text = vartotojas.Saskaitos[pagrindinesSaskNr].Likutis.ToString() + " €";
            }
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

        private async void SaskaitosDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var response = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            IbanText.Content = e.AddedItems[0].ToString();
            if (vartotojas.Saskaitos.Count > 0)
            {
                pagrindinesSaskNr = FindIbanNr(vartotojas.Saskaitos, e.AddedItems[0].ToString());
                LikutisText.Text = vartotojas.Saskaitos[pagrindinesSaskNr].Likutis.ToString() + " €";
            }

            vartotojasSaved = vartotojas;

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

        private void IsrasasButton(object sender, RoutedEventArgs e)
        {

        }

        async private void PavedimasButton(object sender, RoutedEventArgs e)
        {
            var response = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            vartotojasSaved = vartotojas;
            var pavedimas = new Pervedimas(vartotojasSaved, keySaved);
            pavedimas.Show();
            Close();
        }

        private void ValiutosButton(object sender, RoutedEventArgs e)
        {

        }

        private void PagalbaButton(object sender, RoutedEventArgs e)
        {

        }

        private void ManoButton(object sender, RoutedEventArgs e)
        {
            var manoSaskaitos = new ManoSaskaitos(vartotojasSaved, keySaved);
            manoSaskaitos.Show();
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
    }
}
