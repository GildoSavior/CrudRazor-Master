using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace RazorCrud.Pages.Clients
{
    public class Client : PageModel
    {
        private readonly ILogger<Client> _logger;

        public Client(ILogger<Client> logger)
        {
            _logger = logger;
        }

        public List<ClientInfo> clientList = new List<ClientInfo>();

        public void OnGet()
        {
            try
            {
                string connectioString = "Initial Catalog=Client; Data Source=localhost,1433;"
                +"Persist Security Info=true; User ID=sa; Password=1q2w3e4r@#$";

                using (SqlConnection connection = new SqlConnection(connectioString))
                {
                    connection.Open();
                    string sql = "select * from Client";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                ClientInfo cliente  = new ClientInfo() 
                                {
                                    id = "" + reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    phone = reader.GetString(3),
                                    address = reader.GetString(4),
                                    created_at = reader.GetDateTime(5).ToString(),
                                };

                                clientList.Add(cliente);
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
    }

      public class ClientInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string created_at { get; set; }
    }
}