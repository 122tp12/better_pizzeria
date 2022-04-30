using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pizza.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Views.Cart
{
        public class PayForOrderModel : PageModel
        {
            IHttpContextAccessor accessor;
        public double globalPrice;

        public List<PizzaOrder> listOfRequeredPizzas;
        public PayForOrderModel(IHttpContextAccessor _accessor)
            {
            listOfRequeredPizzas = new List<PizzaOrder>();
                accessor = _accessor;
            }
        public void payConfirm(int idCustomer)
        {
            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE customers SET payed=1 WHERE id="+idCustomer, connection);
                command.ExecuteNonQuery();
            }
        }
            public void getOrder()
        {

            double rubCurs = 3;//TODO: get from online
            double dolCurs = 0.27;//TODO: get from online
            globalPrice = 0;
            string val = accessor.HttpContext.Session.GetString("Valute");
            for (int i = 0; true; i++)
            {
                if (accessor.HttpContext.Session.GetString(i.ToString()) != null)
                {
                    string a = accessor.HttpContext.Session.GetString(i.ToString());
                    string[] tmpMas = a.Split("||");
                    Pizza p;
                    PizzaOrder order;
                    if (tmpMas[0] != "-1")
                    {
                        p = Pizza.selectFromDb(int.Parse(tmpMas[0]));
                        order = new PizzaOrder(p, tmpMas[1], tmpMas[3], tmpMas[2]);
                    }
                    else
                    {
                        p = new Pizza(-1, "Your pizza", "", tmpMas[3], -1);
                        order = new PizzaOrder(p, tmpMas[1], tmpMas[3], tmpMas[2]);


                    }
                    if (val == null || val == "uah")
                    {

                    }
                    else if (val == "dol")
                    {
                        order.pizza.price *= dolCurs;
                    }
                    else if (val == "rub")
                    {
                        order.pizza.price *= rubCurs;
                    }

                    order.pizza.price = Math.Round(order.pizza.price, 2);


                    if (order.components.stringedItems != "")
                    {
                        foreach (var eqwwqe in order.components.stringedItems.Split(", "))
                        {
                            order.addingPrice += 10;
                        }
                    }
                    if (order.richness == "Rich")
                    {
                        order.addingPrice += order.pizza.price / 2;
                    }
                    if (order.size == "27")
                    {
                        order.addingPrice += order.pizza.price / 4;
                    }
                    else if (order.size == "30")
                    {
                        order.addingPrice += order.pizza.price / 2;
                    }

                    if (order.pizza.price != -1)
                    {
                        globalPrice += order.pizza.price + order.addingPrice;
                    }

                    listOfRequeredPizzas.Add(order);
                    //accessor.HttpContext.Session.SetString(i.ToString(), id.ToString() + "||" + size + "||" + components + "||" + richnes);

                }
                else
                {
                    break;
                }
            }
        }
        }
}
