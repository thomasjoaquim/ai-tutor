using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace MyProject.Pages
{
    public class TestConnectionModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public TestConnectionModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsConnected { get; set; }
        public string Message { get; set; } = "";
        public string ServerVersion { get; set; } = "";

        public void OnGet()
        {
            TestConnection();
        }

        public void OnPost()
        {
            TestConnection();
        }

        private void TestConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();
                
                IsConnected = true;
                ServerVersion = connection.ServerVersion;
                Message = "Conexão estabelecida com sucesso!";
            }
            catch (Exception ex)
            {
                IsConnected = false;
                Message = $"Erro: {ex.Message}";
            }
        }
    }
}