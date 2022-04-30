using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace pizza.Classes
{
    public class Pizza 
    {
        public int id;
        public string name;
        public string image;
        public Ingradients Ingradients;
        public string type;
        public double price;
        public Pizza(){
            Ingradients = new Ingradients();
        }
        public Pizza(int _id, string _name, string _image, string rawIngradients, double _price)
        {
            id = _id;
            name = _name;
            image = _image;
            price = _price;
            Ingradients = new Ingradients(rawIngradients);
        }
        public static List<Pizza> selectFromDb()
        {

            List <Pizza> list=new List<Pizza>();

            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("select * from pizza_list", connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Pizza((int)reader[0], reader[1].ToString().Trim(), reader[2].ToString().Trim(), reader[3].ToString().Trim(), double.Parse(((Single)reader[4]).ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;

        }
        public static Pizza selectFromDb(int id)
        {

            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("select * from pizza_list where id="+id, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return (new Pizza((int)reader[0], reader[1].ToString().Trim(), reader[2].ToString().Trim(), reader[3].ToString().Trim(), double.Parse(((Single)reader[4]).ToString())));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;

        }
        public static Pizza selectFromDb(int id, SqlConnection connection)
        {

            
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand("select * from pizza_list where id=" + id, connection);

                try
                {
                Pizza p=null;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                    p = (new Pizza((int)reader[0], reader[1].ToString().Trim(), reader[2].ToString().Trim(), reader[3].ToString().Trim(), double.Parse(((Single)reader[4]).ToString())));
                    }
                    reader.Close();
                return p;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
            return null;

        }
        public static List<Pizza> selectFromDb(string where)
        {

            List<Pizza> list = new List<Pizza>();

            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlCommand command;
                // Create the Command and Parameter objects.
                if (where == "")
                {
                    command = new SqlCommand("select * from pizza_list", connection);
                }
                else
                {
                    command = new SqlCommand("select * from pizza_list where " + where, connection);
                }
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Pizza((int)reader[0], reader[1].ToString().Trim(), reader[2].ToString().Trim(), reader[3].ToString().Trim(), double.Parse(((Single)reader[4]).ToString())));
                        
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return list;

        }
    }
    public class Ingradients
    {
        public List<string> items;
        public string stringedItems;
        public Ingradients(string a)
        {
            stringedItems = a;
            items =a.Split(", ").ToList();
        }
        public Ingradients()
        {
            
        }
    }
}
