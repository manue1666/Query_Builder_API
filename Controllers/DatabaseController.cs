using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateDatabase([FromBody] CreateDatabaseDto dto)
        {
            var userId = GetUserId();
            if(userId == 0)
            {
                return Unauthorized(new ApiResponse<string> 
                { Success = false, Message = "Invalid user ID in token." });
            }
            var result = _databaseService.CreateDatabase(userId, dto);
            return Ok(new ApiResponse<string> { Success = true, Message = result });
        }

        [Authorize]
        [HttpGet("all")]
        public IActionResult GetAllDatabases()
        {
            var userId = GetUserId();
            if(userId == 0)
            {
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "Invalid user ID in token." });
            }
            var result = _databaseService.GetAllDatabases(userId);
            if(result == null || result.Count == 0)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "No databases found." });
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetDatabaseById(int id)
        {
            var userId = GetUserId();
            if(userId == 0)
            {
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "Invalid user ID in token." });
            }
            var result = _databaseService.GetDatabaseById(id, userId);
            if(result == null)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "No database Found"});
            }
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteDatabase(int id)
        {            
            var userId = GetUserId();
            if(userId == 0)
            {
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "Invalid user ID in token." });
            }
            var result = _databaseService.DeleteDatabase(id, userId);
            if(!result)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "No database Found" });
            }
            return Ok(new ApiResponse<string> { Success = true, Message = "Database deleted successfully" });
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateDatabase(int id, [FromBody] UpdateDatabaseDto dto)
        {
            var userId = GetUserId();
            if(userId == 0)
            {
                return Unauthorized(new ApiResponse<string> { Success = false, Message = "Invalid user ID in token." });
            }
            var result = _databaseService.UpdateDatabase(id, userId, dto);
            if(!result)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "No database Found" });
            }
            return Ok(new ApiResponse<string> { Success = true, Message = "Database updated successfully" });
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("sub")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? "0";
            
            return int.Parse(userIdClaim);
        }
    }
}