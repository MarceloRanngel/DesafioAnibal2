using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Prova.Pages
{
    public class IndexModel : PageModel
    {
        public FraseInfo fraseInfo = new FraseInfo();
        public String statusMessage = "";
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)

                
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            fraseInfo.frase = Request.Form["frase"];

            try
            {
                String connectionString = "Data Source=DESKTOP-LIMIU14;Initial Catalog=ProvaTecnica;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {   
                    connection.Open();
                    String sql = "INSERT INTO frase " +
                        "(frase) VALUES" +
                        "(@frase)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@frase", fraseInfo.frase);
                        command.ExecuteNonQuery();
                    }

                    statusMessage = "Enviado com sucesso!";
                }
            }
            catch (Exception ex)
            {   
                statusMessage = ex.Message;
                Console.Write(ex.ToString());
            }

            
        }

        public class FraseInfo
        {
            public string id;
            public string frase;
        }
    }
}