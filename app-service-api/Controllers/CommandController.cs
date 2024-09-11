using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _commandFunctionUrl = "https://<your-command-function-app>.azurewebsites.net/api/command/insert";

        public CommandController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertData([FromBody] InsertRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Data))
            {
                return BadRequest("Invalid request data.");
            }

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_commandFunctionUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok("Data inserted successfully.");
            }

            return StatusCode((int)response.StatusCode, "Failed to insert data.");
        }
    }

    public class InsertRequest
    {
        public string Data { get; set; }
    }
}
