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
using FireSharp.Config;
using FireSharp.Interfaces;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;


namespace FinewareWPF
{
    /// <summary>
    /// Interaction logic for Apzvalga.xaml
    /// </summary>
    public partial class Apzvalga : Window
    {
        Vartotojas vartotojasSaved;
        string keySaved;
        DateTime kairiojiData = DateTime.Now.AddMonths(-1);
        DateTime desiniojiData = DateTime.Now;
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
            IbanText.Content = vartotojasSaved.Saskaitos[pagrindinesSaskNr].Kodas;
            LikutisText.Text = vartotojasSaved.Saskaitos[pagrindinesSaskNr].Likutis + " €";
            vardoPavardesText.Text = vartotojasSaved.Vardas + " " + vartotojasSaved.Pavarde;
            saskaitosPavadinimas.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Pavadinimas;
            emailText.Text = vartotojasSaved.Epastas;
            Israsas paskutinisSiustas = LastSent(vartotojasSaved, pagrindinesSaskNr);
            Israsas paskutinisGautas = LastRecieved(vartotojasSaved, pagrindinesSaskNr);
            if (paskutinisSiustas != null)
            {
                issiustaKodas.Content = paskutinisSiustas.Kodas;
                issiustaPavadinimas.Content = paskutinisSiustas.Pavadinimas;
                issiustaSuma.Text = "-" + paskutinisSiustas.Suma.ToString() + " €";
            }
            if (paskutinisGautas != null)
            {
                gautaKodas.Content = paskutinisGautas.Kodas;
                gautaPavadinimas.Content = paskutinisGautas.Pavadinimas;
                gautaSuma.Text = "+" + paskutinisGautas.Suma.ToString() + " €";
            }
            pradineData.DisplayDate = kairiojiData;
            pradineData.Text = kairiojiData.ToShortDateString();
            galineData.DisplayDate = desiniojiData;
            galineData.Text = desiniojiData.ToShortDateString();
            canGraph.Children.Clear();
            DrawGraph();
            if (vartotojasSaved.NaujiNotification)
            {
                notificationCircle.Visibility = Visibility.Visible;
            }
            avatarIcon.Source = new BitmapImage(new Uri("Images/Avatars/avatar" + vartotojas.AvatarIndex + ".png", UriKind.Relative));
        }

        private void galineData_ValueChanged(object sender, SelectionChangedEventArgs e)
        {
            desiniojiData = galineData.SelectedDate.Value;
            BendraGauta(vartotojasSaved);
            BendraSiusta(vartotojasSaved);
            canGraph.Children.Clear();
            DrawGraph();
        }

        private void pradineData_ValueChanged(object sender, SelectionChangedEventArgs e)
        {
            kairiojiData = pradineData.SelectedDate.Value;
            BendraGauta(vartotojasSaved);
            BendraSiusta(vartotojasSaved);
            canGraph.Children.Clear();
            DrawGraph();
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

        double MaxIsrasuReiksme()
        {
            List<Israsas> israsai = vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai;
            double max = 0;
            for (int i = 0; i < israsai.Count; i++)
            {
                // ar įeina į pasirinktą datą
                if (israsai[i].Data.Date <= desiniojiData.Date && israsai[i].Data.Date >= kairiojiData.Date)
                {
                    if (israsai[i].Suma > max)
                    {
                        max = israsai[i].Suma;
                    }
                }
            }
            return max;
        }

        void DrawGraph()
        {
            const double margin = 10;
            double xmax = canGraph.Width - margin;
            double ymax = canGraph.Height - margin;
            double step = 0;
            int maxDays = Convert.ToInt32((desiniojiData - kairiojiData).TotalDays / 31);
            int aCount = maxDays;
            if ((desiniojiData - kairiojiData).TotalDays <= 0)
            {
                step = xmax;
            }
            else
            {
                step = xmax / (Convert.ToInt32((desiniojiData - kairiojiData).TotalDays));
            }
            int maxSkaicius = Convert.ToInt32(MaxIsrasuReiksme() / 10);
            int stepas = Convert.ToInt32(ymax / 10);
            int maxas = Convert.ToInt32(ymax) + 30;
            for(int i = 0 ; i < 11; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 10;
                textBlock.FontWeight = FontWeights.Bold;
                textBlock.RenderTransform = new RotateTransform(0);
                textBlock.Text = ((maxSkaicius) * i).ToString();
                textBlock.Foreground = Brushes.DarkGray;
                Canvas.SetLeft(textBlock, 0);
                Canvas.SetTop(textBlock, maxas-=stepas);
                canGraph.Children.Add(textBlock);
            }

            DateTime pradineSpausdinimui = kairiojiData;
            aCount = maxDays;
            double countA = 0;
            GeometryGroup xaxis_geom = new GeometryGroup();
            while (pradineSpausdinimui.Date <= desiniojiData.Date)
            {
                if (aCount == 0)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.FontSize = 10;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.RenderTransform = new RotateTransform(45);
                    textBlock.Text = pradineSpausdinimui.Month + "-" + pradineSpausdinimui.Day;
                    textBlock.Foreground = Brushes.DarkGray;
                    Canvas.SetLeft(textBlock, countA);
                    Canvas.SetTop(textBlock, ymax+2);
                    canGraph.Children.Add(textBlock);
                    aCount = maxDays;
                }
                else
                {
                    aCount--;
                }
                countA += step;
                pradineSpausdinimui = pradineSpausdinimui.AddDays(1);
            }
            if ((desiniojiData - kairiojiData).TotalDays < 0) return;
            // Make some data sets.
            for (int j = 0; j < 2; j++)
            {
                PointCollection points = new PointCollection();
                DateTime pradine = kairiojiData;
                string tipas = "Siuncia";
                double count = 0;
                Brush brush = Brushes.Red;
                while (pradine.Date <= desiniojiData.Date)
                {
                    if (j == 1)
                    {
                        tipas = "Gauna";
                        brush = Brushes.Green;
                    }

                    bool rado = false;
                    for (int i = 0; i < vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai.Count; i++)
                    {
                        if (vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai[i].Data.Date == pradine.Date && vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai[i].Tipas == tipas)
                        {
                            double aa = (1 - (vartotojasSaved.Saskaitos[pagrindinesSaskNr].Israsai[i].Suma) / MaxIsrasuReiksme()) * ymax;
                            points.Add(new Point(count, aa));
                            rado = true;
                        }
                    }
                    if (!rado)
                    {
                        points.Add(new Point(count, ymax));
                    }
                    pradine = pradine.AddDays(1);
                    count += step;
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 2;
                polyline.Stroke = brush;
                polyline.Points = points;
                canGraph.Children.Add(polyline);

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
            Loading();
            ClearGetSentInfo();
            var response = await client.GetAsync("Paskyros/" + keySaved);
            Vartotojas vartotojas = response.ResultAs<Vartotojas>();
            IbanText.Content = e.AddedItems[0].ToString();
            if (vartotojas.Saskaitos.Count > 0)
            {
                pagrindinesSaskNr = FindIbanNr(vartotojas.Saskaitos, e.AddedItems[0].ToString());
                LikutisText.Text = vartotojas.Saskaitos[pagrindinesSaskNr].Likutis.ToString() + " €";
                saskaitosPavadinimas.Content = vartotojas.Saskaitos[pagrindinesSaskNr].Pavadinimas;
                Israsas paskutinisSiustas = LastSent(vartotojasSaved, pagrindinesSaskNr);
                Israsas paskutinisGautas = LastRecieved(vartotojasSaved, pagrindinesSaskNr);
                if (paskutinisSiustas != null)
                {
                    issiustaKodas.Content = paskutinisSiustas.Kodas;
                    issiustaPavadinimas.Content = paskutinisSiustas.Pavadinimas;
                    issiustaSuma.Text = "-" + paskutinisSiustas.Suma.ToString() + " €";
                }
                if (paskutinisGautas != null)
                {
                    gautaKodas.Content = paskutinisGautas.Kodas;
                    gautaPavadinimas.Content = paskutinisGautas.Pavadinimas;
                    gautaSuma.Text = "+" + paskutinisGautas.Suma.ToString() + " €";
                }
            }
            pradineData.DisplayDate = DateTime.Now.AddMonths(-1);
            pradineData.Text = (DateTime.Now.AddMonths(-1)).ToShortDateString();
            galineData.DisplayDate = DateTime.Now;
            galineData.Text = DateTime.Now.ToShortDateString();
            galineData_ValueChanged(sender, e);
            pradineData_ValueChanged(sender, e);
            Unloading();
            vartotojasSaved = vartotojas;

        }

        void BendraGauta(Vartotojas vartotojas)
        {
            List<Israsas> israsai = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai;
            double bendraSuma = 0;
            for(int i = 0; i < israsai.Count; i++)
            {   
                // ar įeina į pasirinktą datą
                if (israsai[i].Data.Date <= desiniojiData.Date && israsai[i].Data.Date >= kairiojiData.Date)
                {
                    if (israsai[i].Tipas == "Gauna")
                    {
                        bendraSuma += israsai[i].Suma;
                    }
                }
            }
            bendraGauta.Text = "+" + bendraSuma.ToString() + " €";
        }

        void BendraSiusta(Vartotojas vartotojas)
        {
            List<Israsas> israsai = vartotojas.Saskaitos[pagrindinesSaskNr].Israsai;
            double bendraSuma = 0;
            for (int i = 0; i < israsai.Count; i++)
            {
                // ar įeina į pasirinktą datą
                if(israsai[i].Data.Date <= desiniojiData.Date && israsai[i].Data.Date >= kairiojiData.Date)
                {
                    if (israsai[i].Tipas == "Siuncia")
                    {
                        bendraSuma += israsai[i].Suma;
                    }
                } 
            }
            bendraIsleista.Text = "-" + bendraSuma.ToString() + " €";
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
            Loading();
            IsEnabled = false;
            var israsas = new Israsai(vartotojasSaved, keySaved);
            israsas.Show();
            Close();
        }

        async private void PavedimasButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
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
            Loading();
            IsEnabled = false;
            ManoSaskaitos manoSaskaitos = new ManoSaskaitos(vartotojasSaved, keySaved);
            manoSaskaitos.Show();
            Close();
        }

        private void GautiButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            GautiPavedima gautiPavedima = new GautiPavedima(vartotojasSaved, keySaved);
            gautiPavedima.Show();
            Close();
        }

        private void NustatymaiButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var nustatymai = new Nustatymai(vartotojasSaved, keySaved);
            nustatymai.Show();
            Close();
        }

        private void AtsijungtiButton(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            var prisijungimas = new Prisijungimas();
            prisijungimas.Show();
            Close();
        }

        private void PranesimaiButton(object sender, RoutedEventArgs e)
        {

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

        void Unloading()
        {
            dotProgress1.Visibility = Visibility.Hidden;
            dotProgress2.Visibility = Visibility.Hidden;
            dotProgress3.Visibility = Visibility.Hidden;
            greyedOut.Visibility = Visibility.Hidden;
        }
        Israsas LastSent(Vartotojas vartotojas, int saskaitosNr)
        {
            Israsas paskutinisIssiustas = null;
            if (vartotojas.Saskaitos[saskaitosNr].Israsai == null) return paskutinisIssiustas;
            for(int i = vartotojas.Saskaitos[saskaitosNr].Israsai.Count - 1; i >= 0; i--)
            {
                if(vartotojas.Saskaitos[saskaitosNr].Israsai[i].Tipas == "Siuncia")
                {
                    paskutinisIssiustas = vartotojas.Saskaitos[saskaitosNr].Israsai[i];
                    return paskutinisIssiustas;
                }
            }
            return paskutinisIssiustas;
        }

        Israsas LastRecieved(Vartotojas vartotojas, int saskaitosNr)
        {
            Israsas paskutinisGautas = null;
            if (vartotojas.Saskaitos[saskaitosNr].Israsai == null) return paskutinisGautas;
            for (int i = vartotojas.Saskaitos[saskaitosNr].Israsai.Count - 1; i >= 0; i--)
            {
                if (vartotojas.Saskaitos[saskaitosNr].Israsai[i].Tipas == "Gauna")
                {
                    paskutinisGautas = vartotojas.Saskaitos[saskaitosNr].Israsai[i];
                    return paskutinisGautas;
                }
            }
            return paskutinisGautas;
        }
       
        void ClearGetSentInfo()
        {
            issiustaKodas.Content = "";
            issiustaPavadinimas.Content = "";
            issiustaSuma.Text = "";
            gautaKodas.Content = "";
            gautaPavadinimas.Content = "";
            gautaSuma.Text = "";
        }

        private void Pranesimas_Button(object sender, RoutedEventArgs e)
        {
            Loading();
            IsEnabled = false;
            Pranesimai pranesimai = new Pranesimai(vartotojasSaved, keySaved);
            pranesimai.Show();
            Close();
        }
    }
}
