using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinewareWPF
{
    public class Israsas
    {
        public string Asmuo { get; set; }
        public double Suma { get; set; }
        public string Paskirtis { get; set; }
        public DateTime Data { get; set; }
        public string Kodas { get; set; }
        public string Pavadinimas { get; set; }
        public string Tipas { get; set; }

        public Israsas()
        {

        }
        public Israsas(string asmuo, double suma, string paskirtis, DateTime data, string kodas, string pavadinimas, string tipas)
        {
            Asmuo = asmuo;
            Suma = suma;
            Paskirtis = paskirtis;
            Data = data;
            Kodas = kodas;
            Pavadinimas = pavadinimas;
            Tipas = tipas;
        }
    }
}
