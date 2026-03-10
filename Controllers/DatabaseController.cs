using Microsoft.AspNetCore.Mvc;
using QueryBuilderApi.Models;

namespace QueryBuilderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult GetTest()
        {
            return Ok(new{ message = "Database Controller Working"});
        }

        [HttpPost("create")]
        public IActionResult CreateDatabase([FromBody] CreateDatabaseDto dto)
        {
            return Ok(new
            {
                message = "Database registered",
                data = dto
            });
        }
    }
}