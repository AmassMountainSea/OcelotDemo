using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Get()
        {
            var project = _configuration.GetValue<string>("project");

            var token = Convert.ToString(HttpContext?.Request.Headers["Authorization"]);
            string info = "Order：" + project;

            return info;


        }
    }
}
