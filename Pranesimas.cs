using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinewareWPF
{
    public class Pranesimas
    {
        public Vartotojas siuntejas { get; set; }
        public Vartotojas gavejas { get; set; }
        public string pavedimo_tipas { get; set; }
        public double suma { get; set; }
        public DateTime data { get; set; }

        public Pranesimas(Vartotojas siunt, Vartotojas gavej, string tipas, double sum, DateTime date)
        {
            siuntejas = siunt;
            gavejas = gavej;
            pavedimo_tipas = tipas;
            suma = sum;
            data = date;
        }
    }
}
