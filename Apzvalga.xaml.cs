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
        Vartotojas vartotojas;
        public string key;
        public int pagrindinesSaskNr { get; set; } = 0;
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "TBJejG2mnXINGAfpm9YPA3Ke51uWlxrf9UNTt64H",
            BasePath = "https://fineware-759f7-default-rtdb.firebaseio.com/"
    };

        public Apzvalga(Vartotojas vartotojas)
        {
            client = new FireSharp.FirebaseClient(config);
            InitializeComponent();
            this.vartotojas = vartotojas;
            pagrindinesSaskNr = 0;
            SaskaitosDrop.ItemsSource = IbanArray(vartotojas.Saskaitos);
            LikutisText.Text = vartotojas.Saskaitos[pagrindinesSaskNr].Likutis + " €";
            vardoPavardesText.Text = vartotojas.Vardas + " " + vartotojas.Pavarde;
            emailText.Text = vartotojas.Epastas;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var response = await client.GetAsync("Paskyros/" + key);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();

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
            var response = await client.GetAsync("Paskyros/" + key);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            IbanText.Content = e.AddedItems[0].ToString();
            if (vartotojas.Saskaitos.Count > 0)
            {
                pagrindinesSaskNr = FindIbanNr(vartotojas.Saskaitos, e.AddedItems[0].ToString());
                LikutisText.Text = vartotojas.Saskaitos[pagrindinesSaskNr].Likutis.ToString() + " €";
            }

        }
    }
}
