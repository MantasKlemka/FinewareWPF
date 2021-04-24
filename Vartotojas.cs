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
        public bool IsStaff { get; set; }
        public bool NaujiNotification { get; set; } = false;
        public int ShortSecurityCode { get; set; }
        public int LongSecurityCode { get; set; }
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
        }
    }
}
