using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Classes
{
    public class PizzaOrder
    {
        public Pizza pizza;
        public string size;
        public string richness;
        public Ingradients components;
        public double addingPrice;
        public PizzaOrder(Pizza _pizza, string _size, string _richness, string _components)
        {
            addingPrice = 0;
            pizza = _pizza;
            size = _size;
            richness = _richness;
            components = new Ingradients(_components);
            if (_components!="") {
                foreach (var i in _components.Split(", "))
                {
                    addingPrice += 10;
                }
            }
        }

    }
}
