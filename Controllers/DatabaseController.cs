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

        [HttpGet("all")]
        public IActionResult GetAllDatabases()
        {
            var result = _databaseService.GetAllDatabases();
            if(result == null || result.Count == 0)
            {
                return NotFound(new {message = "No databases found."});
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDatabaseById(int id)
        {
            var result = _databaseService.GetDatabaseById(id);
            if(result == null)
            {
                return NotFound(new {message = "No database Found"});
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDatabase(int id)
        {
            var result = _databaseService.DeleteDatabase(id);
            if(!result)
            {
                return NotFound(new {message = "No database Found"});
            }
            return Ok(new {message = "Database deleted successfully"});
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDatabase(int id, [FromBody] UpdateDatabaseDto dto)
        {
            var result = _databaseService.UpdateDatabase(id, dto);
            if(!result)
            {
                return NotFound(new {message = "No database Found"});
            }
            return Ok(new {message = "Database updated successfully"});
        }
    }
}