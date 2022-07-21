using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RazorCrud.Pages.Clients
{
    public class Edit : PageModel
    {
        private readonly ILogger<Edit> _logger;

        public Edit(ILogger<Edit> logger)
        {
            _logger = logger;
        }

        public ClientInfo cliente = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                string connectioString = "Initial Catalog=Client; Data Source=localhost,1433;"
                + "Persist Security Info=true; User ID=sa; Password=1q2w3e4r@#$";

                using (SqlConnection connection = new SqlConnection(connectioString))
                {
                    connection.Open();

                    string sql = "select * from Client where id=@id;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {                                
                                    cliente.id = "" + reader.GetInt32(0);
                                    cliente.name = reader.GetString(1);
                                    cliente.email = reader.GetString(2);
                                    cliente.phone = reader.GetString(3);
                                    cliente.address = reader.GetString(4);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }

        public void OnPost()
        {
            cliente.id = Request.Form["id"];
            cliente.name = Request.Form["name"];
            cliente.email = Request.Form["email"];
            cliente.phone = Request.Form["phone"];
            cliente.address = Request.Form["address"];

            if (cliente.name.Length == 0 || cliente.email.Length == 0 ||
                cliente.phone.Length == 0 || cliente.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectioString = "Initial Catalog=Client; Data Source=localhost,1433;"
                + "Persist Security Info=true; User ID=sa; Password=1q2w3e4r@#$";

                using (SqlConnection connection = new SqlConnection(connectioString))
                {
                    connection.Open();
                    string sql = "UPDATE Client SET name=@name, email=@email, phone=@phone, address=@address WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", cliente.id);
                        command.Parameters.AddWithValue("@name", cliente.name);
                        command.Parameters.AddWithValue("@email", cliente.email);
                        command.Parameters.AddWithValue("@phone", cliente.phone);
                        command.Parameters.AddWithValue("@address", cliente.address);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            
            cliente.name = ""; cliente.email = ""; cliente.phone = ""; cliente.address = "";
            successMessage = "Client Updated";
            Response.Redirect("/Clients/Client");
        }
    }
}