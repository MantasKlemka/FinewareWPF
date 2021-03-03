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
        public int ShortSecurityCode { get; set; }
        public int LongSecurityCode { get; set; }

        public Vartotojas()
        {

        }
        public Vartotojas(string vardas, string pavarde, string epastas, string slaptazodis)
        {
            Vardas = vardas;
            Pavarde = pavarde;
            Epastas = epastas;
            Slaptazodis = slaptazodis;
            IsStaff = false;
            ShortSecurityCode = 0;
            LongSecurityCode = 0;
        }
    }
}
