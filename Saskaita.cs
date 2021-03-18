using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinewareWPF
{
    public class Saskaita
    {
        public string Kodas { get; set; }
        public double Likutis { get; set; }

        public Saskaita()
        {

        }
        public Saskaita(string kodas, double likutis)
        {
            Kodas = kodas;
            Likutis = likutis;
        }
    }
}
