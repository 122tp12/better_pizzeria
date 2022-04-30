using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Classes
{
    public class ExchangesForecast
    {
        public DateTime Date { get; set; }

        public string nameFrom { get; set; }

        public string nameTo { get; set; }
        
        public double coefficient { get; set; }
    }
}
