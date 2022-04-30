using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Classes
{
    public class Banner
    {
        public int id { get; set; }
        public string description { get; set; }
        public string pic { get; set; }
        public Banner()
        {

        }
        public Banner(int _id, string _desc, string _pic)
        {
            id = _id;
            description = _desc;
            pic = _pic;
        }
        public static List<Banner> getFromDB()
        {
            List<Banner> l = new List<Banner>();

            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection conn=new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("select * from banners", conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        l.Add(new Banner((int)reader[0], ((string)reader[2]).Trim(), ((string)reader[1]).Trim()));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return l;
        }
    }
}
