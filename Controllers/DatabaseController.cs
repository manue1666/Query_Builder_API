using Microsoft.AspNetCore.Mvc;
using QueryBuilderApi.Models;
using QueryBuilderApi.Services;

namespace QueryBuilderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        // Injecting the DatabaseService into the controller
        public DatabaseController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        [HttpPost("create")]
        public IActionResult CreateDatabase([FromBody] CreateDatabaseDto dto)
        {
            var result = _databaseService.CreateDatabase(dto);
            return Ok(new {message = result});
        }
    }
}