using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppServiceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _queryFunctionUrl = "https://<your-query-function-app>.azurewebsites.net/api/query/get";

        public QueryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetData([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid ID.");
            }

            var response = await _httpClient.GetAsync($"{_queryFunctionUrl}?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode, "Failed to retrieve data.");
        }
    }
}
