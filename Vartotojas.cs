using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinewareWPF
{
    public class Vartotojas
    {
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Epastas { get; set; }
        public string Slaptazodis { get; set; }
        public string Salis { get; set; }
        public string Adresas { get; set; }
        public string TelefonoNumeris { get; set; }
        public bool IsStaff { get; set; }
        public bool NaujiNotification { get; set; } = false;
        public int ShortSecurityCode { get; set; }
        public int LongSecurityCode { get; set; }
        public int AvatarIndex { get; set; }

        // Vartotojo pranesimai
        public bool Notification_Gavimas { get; set; }
        public bool Notification_Siuntimas { get; set; }
        public bool Notification_Prasymas { get; set; }

        // Paskyros ištrynimas
        public bool ToDelete { get; set; }
        public DateTime Istrynimo_Data { get; set; }

        // Taupymas
        public bool Taupyti { get; set; }
        public double Sutaupyta_suma { get; set; }

        public int MinSuma { get; set; } //minimali suma, kuria virsijus darant pavedimus, prasoma ivesti 6-iu skaitmenu koda
        public List<Saskaita> Saskaitos = new List<Saskaita>();
        public List<Pranesimas> Pranesimai = new List<Pranesimas>();


        public Vartotojas()
        {

        }
        public Vartotojas(string vardas, string pavarde, string epastas, string slaptazodis, List<Saskaita> saskaitos)
        {
            Vardas = vardas;
            Pavarde = pavarde;
            Epastas = epastas;
            Slaptazodis = slaptazodis;
            IsStaff = false;
            ShortSecurityCode = 0;
            LongSecurityCode = 0;
            Saskaitos = saskaitos;
            MinSuma = 0;
            ToDelete = false;
            AvatarIndex = 6;
            Sutaupyta_suma = 0;
        }
    }
}
