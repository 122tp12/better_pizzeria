using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using pizza.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace pizza.Views.Cart
{
    public class OrderConfirmModel: PageModel
    {
        IHttpContextAccessor accessor;
        public double globalPrice;

        public List<PizzaOrder> listOfRequeredPizzas;
        public OrderConfirmModel(IHttpContextAccessor _accessor)
        {
            listOfRequeredPizzas = new List<PizzaOrder>();

            accessor = _accessor;
        }
        public void getAllPizzasFromSessions()
        {

            double rubCurs = 3;
            double dolCurs = 0.27;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44342/");
                //HTTP GET
                var responseTask = client.GetAsync("Exchanges");
                try
                {
                    responseTask.Wait();
               
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsStringAsync();
                        readTask.Wait();

                        string students = readTask.Result;

                        JArray json = JArray.Parse(students);
                        for (int i = 0; i < json.Count; i++)
                        {
                            if (json[i]["nameTo"].ToObject<string>() == "RUB")
                            {
                                rubCurs = json[i]["coefficient"].ToObject<double>();
                            }
                            else if (json[i]["nameTo"].ToObject<string>() == "DOL")
                            {
                                dolCurs = json[i]["coefficient"].ToObject<double>();
                            }
                        }

                    }
                    else
                    {
                        rubCurs = 3;
                        dolCurs = 0.27;
                        Console.WriteLine("Api error");
                    }
                }
                catch
                {

                    rubCurs = 3;
                    dolCurs = 0.27;
                    Console.WriteLine("Api error");
                }
            }
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
                        order.addingPrice += order.pizza.price/2;
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
                        globalPrice += order.pizza.price+order.addingPrice;
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
