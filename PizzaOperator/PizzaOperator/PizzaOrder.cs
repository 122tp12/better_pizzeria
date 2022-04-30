using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaOperator
{
    class PizzaOrder
    {
        public int id;
        public string size;
        public string components;
        public string richness;
        public int pizzaId;
        public string name;
        public string img;
        public string mainComponents;
        public double price;
        public double addingPrice;
        public PizzaOrder(int _id, string _size, string _c, string _r, int _pi, string _name, string _img, string _components, double _price)
        {
            price = _price;
            mainComponents = _components;
            img = _img;
            name = _name;
            id = _id;
            size = _size;
            components = _c;
            richness = _r;
            pizzaId = _pi;
            addingPrice = 0;
        }
    }
}
