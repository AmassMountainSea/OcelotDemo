using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OcelotDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1 from Ocelot", "value2 from Ocelot" };
        }
    }
}
