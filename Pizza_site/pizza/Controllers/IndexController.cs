using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pizza.Views;
using pizza.Views.Index;
using System.Data.SqlClient;
using pizza.Classes;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Threading;

namespace pizza.Controllers
{
   
    public class IndexController:Controller
    {
        public IHttpContextAccessor accessor;
        //Just taking accessor for session
        public IndexController(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
        }

        //This funktion return standart components of pizza
        private List<string> funk(IHttpContextAccessor accessor)
        {
            List<string> l = new List<string>();



            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("select * from [standart_choise]", connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        l.Add(reader[1].ToString().Trim());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            List<string> end = new List<string>();
            for(int i=0;i<l.Count ;i++ )
            {
                if (accessor.HttpContext.Request.Query[l[i]].ToString()=="on")
                {
                    end.Add(l[i]);
                }
            }
            return end;
        }

        //This funk return list of pizzas exeptiReaplyModelns
        private List<string> funk2(IHttpContextAccessor accessor)
        {
            List<string> l = new List<string>();



            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("select * from [pizza_exeptions]", connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        l.Add(reader[1].ToString().Trim());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            List<string> end = new List<string>();
            for (int i = 0; i < l.Count; i++)
            {
                if (accessor.HttpContext.Request.Query[l[i]].ToString() == "on")
                {
                    end.Add(l[i]);
                }
            }
            return end;
        }
        
        public IActionResult Reaply()
        {
            ViewBagFiller filler = new ViewBagFiller(this, accessor);
            filler.fillViewBag("Index/Reaply"); // fill ViewBag for for redirec to main in layout

            return View();
        }
        [HttpPost]
        public IActionResult ReaplyConfirm(int? rating, string? text)
        {
            try
            {
                ReaplyModel model = new ReaplyModel(accessor);
                model.submithRaitingSql(text, rating.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Redirect("~/");
        }
        public IActionResult Index(int? from, int? to)
        {
            IndexModel model = new IndexModel(accessor);

            ViewBagFiller filler = new ViewBagFiller(this, accessor);
            filler.fillViewBag(""); // fill ViewBag for for redirec to main in layout

            int fromE = 0;
            int toE = 999999;
            List<string> list;
            list = funk(accessor);

            List<string> list2;
            list2 = funk2(accessor);
            if (from != null)
            {
                fromE = from.Value;
                model.selectedMin = fromE;
            }
            if (to != null)
            {
                toE = to.Value;
                model.selectedMax = toE;
            }
            model.selectedParams = list;
             model.selectByParam(list, fromE, toE, list2);

            return View(model);
        }
        public IActionResult OwnChoise()
        {
            OwnChoiseModel model = new OwnChoiseModel();

            ViewBagFiller filler = new ViewBagFiller(this, accessor);
            filler.fillViewBag("Index/OwnChoise");  // fill ViewBag for for redirec to own choise in layout

            return View(model);
        }
        [HttpPost]
        public IActionResult SaveGlobalOrder(string email, string telephone, string name, string surname, string adress)
        {
            #region setings
            string login = "udhdj055@gmail.com";
            string password = "Ivan254478";
            string smtp = "smtp.gmail.com";
            int port = 587;
            #endregion
            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            List<PizzaOrder> lTmp = new List<PizzaOrder>();
            int idCustomer=-1;
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("insert into customers(email, telephone, customerName, adress, dateOrder, payed) values('" + email+"','"+telephone+"','"+name+" "+surname+"','"+adress+ "', GETDATE(), 0);", connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    command = new SqlCommand("SELECT TOP 1 * FROM customers ORDER BY ID DESC", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();

                    idCustomer = (int)reader[0];
                    reader.Close();
                    
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
                                p = Pizza.selectFromDb(int.Parse(tmpMas[0]), connection);
                                order = new PizzaOrder(p, tmpMas[1], tmpMas[3], tmpMas[2]);
                                
                            }
                            else
                            {
                                p = new Pizza(-1, "Your pizza", "", tmpMas[3], -1);
                                order = new PizzaOrder(p, tmpMas[1], tmpMas[3], tmpMas[2]);


                            }
                            lTmp.Add(order);
                            Console.WriteLine(idCustomer);
                            command = new SqlCommand("insert into orders(customerId, pizzaId, components, richness, size) values("+ idCustomer+", "+order.pizza.id+", '"+order.components.stringedItems+"','"+order.richness+"','"+order.size+ "')", connection);
                            command.ExecuteNonQuery();


                           

                        }
                        else
                        {
                            break;
                        }
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Thread t = new Thread(()=> { 
            try
            {
                SmtpClient smtpClient = new SmtpClient(smtp, port);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;

                smtpClient.Credentials = new System.Net.NetworkCredential(login, password);
                
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                mail.Subject = "Pizza order";
                   
                mail.Body = "Your name: " + name + " " + surname + "<br/>" +
                    "Telephone: " + telephone + "<br/>" +
                    "Date: "+ DateTime.Now+"<br/>"+
                    "Adress: " + adress + "<br/><hr/><table>";
                mail.Body += "<tr><td>Name</td><td>Size</td><td>Richness</td><td>Components</td><td>Price</td></tr>";
                    //mail.Body += "<tr><td>Name</td><td>Size</td><td>Richness</td><td>Components</td><td>Price</td></tr>";
                    double global = 0;
                foreach (var item in lTmp)
                {
                    double p = 0;

                    if (item.components.stringedItems != "")
                    {
                        foreach (var eqwwqe in item.components.stringedItems.Split(", "))
                        {
                            p += 10;
                        }
                    }
                    if (item.richness == "Rich")
                    {
                        p += item.pizza.price / 2;
                    }
                    if (item.size == "27")
                    {
                        p += item.pizza.price / 4;
                    }
                    else if (item.size == "30")
                    {
                        p += item.pizza.price / 2;
                    }
                    mail.Body += "<tr>";
                    mail.Body += "<td>"+item.pizza.name+"</td>";
                    mail.Body += "<td>" + item.size + "</td>";
                    mail.Body += "<td>" + item.richness + "</td>";
                    mail.Body += "<td>" + item.components.stringedItems + "</td>";
                    mail.Body += "<td>" + item.pizza.price+"+"+ p + "</td>";
                    mail.Body += "</tr>";
                    global += p + item.pizza.price;
                }
                mail.Body += "<tr><td></td><td></td><td></td><td></td><td>"+global+"</td></tr>";
                mail.Body += "</table>";
                    mail.Body += "<form method=\"post\" action=\"https://localhost:5001/Cart/PayForOrder\">" +
                    "<input type=\"hidden\" name=\"id\" value=\""+ idCustomer + "\"/>"+
                    "<input type=\"submit\" value=\"I vant pay now\"/>"+
                    "</form>";
                mail.IsBodyHtml = true;

                //Setting From , To and CC
                mail.From = new MailAddress("udhdj055@gmail.com", "Pizza");
                mail.To.Add(new MailAddress(email));
                //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                smtpClient.Send(mail);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            });
            t.Start();

            accessor.HttpContext.Session.Clear();

            return Redirect("~/");
        }
        [HttpPost]
        public IActionResult SaveOrder(int id, string richnes, string size, string components)
        {
            for (int i = 0; true; i++)
            {
                if (accessor.HttpContext.Session.GetString(i.ToString()) == null)
                {
                    accessor.HttpContext.Session.SetString(i.ToString(), id.ToString() + "||" + size + "||" + components + "||" + richnes);
                    Console.WriteLine(id.ToString() + "||" + size + "||" + components + "||" + richnes);
                    break;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; accessor.HttpContext.Session.GetString(i.ToString()) != null; i++)
            {
                Console.WriteLine(accessor.HttpContext.Session.GetString(i.ToString()));
            }
            Console.WriteLine();
            Console.WriteLine();

            return Redirect("~/"); 
        }
        [HttpPost]
        public IActionResult SaveOwnOrder(string size, string components, string richnes)
        {
            for (int i = 0; true; i++)
            {
                if (accessor.HttpContext.Session.GetString(i.ToString()) == null)
                {
                    accessor.HttpContext.Session.SetString(i.ToString(), "-1||"+ size + "||" + components + "||" + richnes);
                    break;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; accessor.HttpContext.Session.GetString(i.ToString()) != null; i++)
            {
                Console.WriteLine(accessor.HttpContext.Session.GetString(i.ToString()));
            }
            Console.WriteLine();
            Console.WriteLine();

            
            return Redirect("~/");
        }
    }
}
