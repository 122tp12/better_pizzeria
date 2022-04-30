using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PizzaOperator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            l = new List<PizzaOrder>();
            blockedId = new List<int>();
            InitializeComponent();
        }
        public int? id;
        public string customerName;
        public string email;
        public string telephone;
        public string adress;
        public bool payed;
        public DateTime? date;
        List<PizzaOrder> l;
        List<int> blockedId; 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread t=new Thread(()=> {
                SqlConnection connection = new SqlConnection("Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                
                connection.Open();

                SqlCommand command = new SqlCommand("select * from [customers] order by [dateOrder]", connection);

                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    if (((string)reader[1]).Trim() != "test")
                    {
                        id = (int)reader[0];
                        customerName = (string)reader[1];
                        email = (string)reader[2];
                        telephone = (string)reader[3];
                        adress = (string)reader[4];
                        date = (DateTime)reader[5];
                        try
                        {
                            payed = (int)reader[6] == 1 ? true : false;
                        }
                        catch
                        {
                            payed = false;
                        }
                        if (blockedId.Find(n => n == id)!= id)
                        {
                            break;
                        }
                        else
                        {
                            id = null;
                            customerName = null;
                            email = null;
                            telephone = null;
                            adress = null;
                            date = null;
                            l = new List<PizzaOrder>();
                        }
                    }
                    
                }
                
                reader.Close();
                if (id != null)
                {
                    command = new SqlCommand("select [orders].*, pizza_list.* from [orders], pizza_list where [orders].pizzaId=pizza_list.id and customerId=" + id, connection);

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        l.Add(new PizzaOrder((int)reader[0], ((string)reader[2]).Trim(), ((string)reader[3]).Trim(),
                            ((string)reader[4]).Trim(), (int)reader[5], ((string)reader[7]).Trim(), ((string)reader[8]).Trim(), ((string)reader[9]).Trim(), (float)reader[10]));

                        if (l.Last().richness == "Rich")
                        {
                            l.Last().addingPrice += l.Last().price / 2;
                        }
                        if (l.Last().size == "27")
                        {
                            l.Last().addingPrice += l.Last().price / 4;
                        }
                        else if (l.Last().size == "30")
                        {
                            l.Last().addingPrice += l.Last().price / 2;
                        }
                    }
                    Dispatcher.Invoke(() =>
                    {
                        this.name.Content = "Customer name: " + customerName;
                        this.telephoneL.Content = "Telephone: " + telephone;
                        this.adressL.Content = "Adress: " + adress;
                        this.emailL.Content = "Email: " + email;
                        this.dateL.Content = "Date: " + date;

                        this.orderL.Content = "Order: \n";
                        foreach (var i in l)
                        {
                            this.orderL.Content += "Name: " + i.name + " | Components: " + i.mainComponents + " | Price: " + i.price + " | Richness: " + i.richness + " | Size:" + i.size + " | Adding components: " + i.components + "\n";
                        }
                    });
                }
                else
                {
                    MessageBox.Show("No orders");
                }
                connection.Close();
            });
            t.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Thread t2 = new Thread(() => {
                #region setings
                string login = "udhdj055@gmail.com";
                string password = "Ivan254478";
                string smtp = "smtp.gmail.com";
                int port = 587;
                #endregion
                try
                {
                    SmtpClient smtpClient = new SmtpClient(smtp, port);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;

                    smtpClient.Credentials = new System.Net.NetworkCredential(login, password);

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    MailMessage mail = new MailMessage();
                    mail.Subject = "Pizza order confirm";
                    mail.Body = "<h2>Order confirmed</h2>";
                    mail.Body = "<h2>Your order:</h2>";
                    mail.Body = "Your name: " + customerName + "<br/>" +
                        "Telephone: " + telephone + "<br/>" +
                        "Date: " + DateTime.Now + "<br/>" +
                        "Adress: " + adress + "<br/><hr/><table>";
                    mail.Body += "<tr><td>Name</td><td>Size</td><td>Richness</td><td>Components</td><td>Price</td></tr>";
                    double global = 0;
                    foreach (var item in l)
                    {
                        double p = 0;

                        if (item.components != "")
                        {
                            foreach (var eqwwqe in item.components.Split(", ".ToCharArray()))
                            {
                                p += 10;
                            }
                        }
                        if (item.richness == "Rich")
                        {
                            p += item.price / 2;
                        }
                        if (item.size == "27")
                        {
                            p += item.price / 4;
                        }
                        else if (item.size == "30")
                        {
                            p += item.price / 2;
                        }
                        mail.Body += "<tr>";
                        mail.Body += "<td>" + item.name + "</td>";
                        mail.Body += "<td>" + item.size + "</td>";
                        mail.Body += "<td>" + item.richness + "</td>";
                        mail.Body += "<td>" + item.components + "</td>";
                        mail.Body += "<td>" + item.price + "+" + p + "</td>";
                        mail.Body += "</tr>";
                        global += p + item.price;
                    }
                    mail.Body += "<tr><td></td><td></td><td></td><td></td><td>" + global + "</td></tr>";
                    mail.Body += "</table>";
                    mail.Body += "<a href=\"https://localhost:5001/Index/Reaply\">You can write reaply here</a>";
                    mail.IsBodyHtml = true;

                    //Setting From , To and CC
                    mail.From = new MailAddress("udhdj055@gmail.com", "Pizza");
                    mail.To.Add(new MailAddress(email));
                    //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            t2.Start();
            Thread t = new Thread(new ParameterizedThreadStart((object o)=> {   
                try
                {
                    
                    SqlConnection connection = (SqlConnection)o;

                    SqlCommand command = new SqlCommand("DELETE FROM [customers] WHERE id=" + id, connection);
                    lock (connection)
                    {
                        command.ExecuteNonQuery();
                    }
                    for (int i = 0; i < l.Count; i++)
                    {
                        command = new SqlCommand("DELETE FROM [orders] WHERE id=" + l[i].id, connection);
                        lock (connection)
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }));

            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();
                t.Start(connection);

                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("insert into confirmedCustomers(email, telephone, customerName, adress, dateOrder, payed) values('" + email + "','" + telephone + "','" + customerName + "','" + adress + "', GETDATE(), "+ (payed==true?1:0) + ");", connection);

                try
                {
                    
                    lock (connection)
                    {
                        command.ExecuteNonQuery();
                    }
                    command.ExecuteNonQuery();
                    command = new SqlCommand("SELECT TOP 1 * FROM confirmedCustomers ORDER BY ID DESC", connection);
                    lock (connection)
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        int id = (int)reader[0];
                        reader.Close();
                    }
                    foreach (var i in l)
                    {
                        lock (connection)
                        {
                            command = new SqlCommand("insert into confirmedOrders(customerId, pizzaId, components, richness, size) values(" + id + ", " + i.pizzaId + ", '" + i.components + "','" + i.richness + "','" + i.size + "')", connection);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex){
                    MessageBox.Show(ex.Message);
                }
                t.Join();
            }

            id = null;
            customerName = null;
            email = null;
            telephone = null;
            adress = null;
            date = null;
            l = new List<PizzaOrder>();

            Dispatcher.Invoke(() =>
            {
                this.name.Content = "";
                this.telephoneL.Content = "";
                this.adressL.Content = "";
                this.emailL.Content = "";
                this.dateL.Content = "";
                this.orderL.Content = "";
            });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            id=null;
            customerName=null;
            email=null;
            telephone=null;
            adress=null;
            date = null;
            l=null;
        Thread t = new Thread(() => {
            try
            {
                SqlConnection connection = new SqlConnection("Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM [customers] WHERE id=" + id, connection);
                command.ExecuteNonQuery();
                for (int i = 0; i < l.Count; i++)
                {
                    command = new SqlCommand("DELETE FROM [orders] WHERE id=" + l[i].id, connection);
                    command.ExecuteNonQuery();
                }
                Dispatcher.Invoke(() =>
                {
                    this.name.Content = "";
                    this.telephoneL.Content = "";
                    this.adressL.Content = "";
                    this.emailL.Content = "";
                    this.dateL.Content = "";
                    this.orderL.Content = "";
                });
            }
            catch 
            {
                
            }
            });
            t.Start();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                blockedId.Add(id.Value);

                id = null;
                customerName = null;
                email = null;
                telephone = null;
                adress = null;
                date = null;
                l = new List<PizzaOrder>();

                Dispatcher.Invoke(() =>
                {
                    this.name.Content = "";
                    this.telephoneL.Content = "";
                    this.adressL.Content = "";
                    this.emailL.Content = "";
                    this.dateL.Content = "";
                    this.orderL.Content = "";
                });
            });
            t.Start();
        }
    }
}
