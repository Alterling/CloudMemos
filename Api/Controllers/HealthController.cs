using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CloudMemos.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get() => await Task.FromResult(Ok());
    }
}