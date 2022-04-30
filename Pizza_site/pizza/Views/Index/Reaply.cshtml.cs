
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace pizza.Views.Index
{
    public class ReaplyModel:PageModel
    {
        IHttpContextAccessor accessor;
        public ReaplyModel(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
        }
        public void submithRaitingSql(string text, int raiting)
        {
            string connectionString = "Data Source=DESKTOP-QGEEUPD;Initial Catalog=Pizza;Integrated Security=True";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO [reaplys](reaplyText,reaplyStar ) VALUES('"+ text + "', "+raiting+");", connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
