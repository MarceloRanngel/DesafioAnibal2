using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Prova.Pages
{
    public class IndexModel : PageModel
    {
        public FraseInfo fraseInfo = new FraseInfo();
        public FraseInfo fraseInfo2 = new FraseInfo();
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
            fraseInfo2.frase = Request.Form["frase2"];

            try
            {
                String connectionString = "Data Source=DESKTOP-LIMIU14;Initial Catalog=ProvaTecnica2;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {   
                    connection.Open();
                    String sql = "INSERT INTO frases " +
                        "(frase1, frase2) VALUES" +
                        "(@frase, @frase2)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@frase", ComparaString(fraseInfo.frase, fraseInfo2.frase,1));
                        command.Parameters.AddWithValue("@frase2", ComparaString(fraseInfo.frase, fraseInfo2.frase, 2));
                        command.ExecuteNonQuery();
                    }

                    statusMessage = ComparaString(fraseInfo.frase, fraseInfo2.frase, 1) + " / " + ComparaString(fraseInfo.frase, fraseInfo2.frase, 2);
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

        public string ComparaString(string frase1, string frase2, int posicao)
{
    IEnumerable<string> primeiraFrase = frase1.Split(' ').Distinct();
    IEnumerable<string> segundaFrase = frase2.Split(' ').Distinct();
    IEnumerable<string> diff;
    int indexador;

    //diff primeiraFrase na 1
    //diff segundaFrase na 0

    diff = segundaFrase.Except(primeiraFrase).ToList().Concat(primeiraFrase.Except(segundaFrase));

    indexador = primeiraFrase.ToList().IndexOf(diff.Last());
    primeiraFrase = primeiraFrase.ToArray().Select(s => s.Replace(diff.Last(), "[" + diff.Last() + "]"));
    segundaFrase = segundaFrase.ToArray().Select(s => s.Replace(diff.First(), "[" + diff.First() + "]"));



    return posicao == 1 ? String.Join(" ", primeiraFrase.ToArray()) : String.Join(" ", segundaFrase.ToArray());

}
    }
}