using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinewareWPF
{
    public class Pranesimas
    {
        public string siuntejas { get; set; }
        public string siuntejasEmail { get; set; }
        public string Paskirtis { get; set; }
        public string siuntejasSaskaita { get; set; }
        public string gavejas { get; set; }
        public string pavedimo_tipas { get; set; }
        public double suma { get; set; }
        public DateTime data { get; set; }

        public Pranesimas(string siunt, string gavej, string tipas, double sum, DateTime date, string siuntejasE, string siuntejasS, string paskirtis)
        {
            siuntejas = siunt;
            gavejas = gavej;
            pavedimo_tipas = tipas;
            suma = sum;
            data = date;
            siuntejasEmail = siuntejasE;
            siuntejasSaskaita = siuntejasS;
            Paskirtis = paskirtis;
        }
    }
}
