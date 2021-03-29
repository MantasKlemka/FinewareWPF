using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinewareWPF
{
    public class Saskaita
    {
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public double Likutis { get; set; }
        public DateTime SukurimoData { get; set; }

        public Saskaita()
        {

        }
        public Saskaita(string pavadinimas, string kodas, double likutis, DateTime sukurimoData)
        {
            Pavadinimas = pavadinimas;
            Kodas = kodas;
            Likutis = likutis;
            SukurimoData = sukurimoData;
        }
    }
}
