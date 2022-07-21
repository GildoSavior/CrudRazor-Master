using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RazorCrud.Pages.Clients
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;

        public Create(ILogger<Create> logger)
        {
            _logger = logger;
        }

        public ClientInfo cliente = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
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

            try {
                   string connectioString = "Initial Catalog=Client; Data Source=localhost,1433;" 
                   +"Persist Security Info=true; User ID=sa; Password=1q2w3e4r@#$";

                   using(SqlConnection connection = new SqlConnection(connectioString))
                   {
                    connection.Open();

                    string sql = "INSERT INTO client" + 
                    "(name, email, phone, address)" +
                    "values (@name, @email, @phone, @address);";

                    using(SqlCommand command = new SqlCommand(sql, connection)){
                        
                        command.Parameters.AddWithValue("@name", cliente.name);
                        command.Parameters.AddWithValue("@email", cliente.email);
                        command.Parameters.AddWithValue("@phone", cliente.phone);
                        command.Parameters.AddWithValue("@address", cliente.address);
                        command.ExecuteNonQuery();
                    }
                   }
            } catch(Exception ex )
            {
                errorMessage = ex.Message;
                return;
            }

            cliente.name = ""; cliente.email = ""; cliente.phone = ""; cliente.address = "";
            successMessage = "Client Created";
            Response.Redirect("/Clients/Client");
        }
    }
}