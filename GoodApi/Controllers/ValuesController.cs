using GoodApi.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<MyOptions> _options;
        private readonly IFreeSql _freeSql;

        public ValuesController(IConfiguration configuration,IOptionsMonitor<MyOptions> options,IFreeSql freeSql)
        {
            _configuration = configuration;
            this._options = options;
            this._freeSql = freeSql;
        }

        [HttpGet]
        public string Get()
        {
           var project = _configuration["project"];
            
             var token = Convert.ToString(HttpContext?.Request.Headers["Authorization"]);

            var dbconfig = _freeSql.Ado.ConnectionString;

            string info = "Good：" + project +"；新数据："+ dbconfig;

            return info;


        }
    }
}
