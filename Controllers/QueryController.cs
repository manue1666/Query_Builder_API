
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryBuilderApi.Models;
using QueryBuilderApi.Services;

namespace QueryBuilderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryController: ControllerBase
    {
        private readonly QueryService _queryService;

        public QueryController(QueryService queryService)
        {
            _queryService = queryService;
        }

        [Authorize]
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuery([FromBody] GenerateQueryRequest request)
        {
            var userId = GetUserId();
            var result = await _queryService.GenerateQuery(request, userId);
            if(result == null)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Database not found for the provided ID" });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Query successfully generated", Data = result });
        }

        [Authorize]
        [HttpGet("all/{databaseId}")]
        public IActionResult GetAllQueriesByDatabaseId(int databaseId)
        {
            var userId = GetUserId();
            var queries = _queryService.GetAllQueriesByDatabaseId(databaseId, userId);
            return Ok(new ApiResponse<object> { Success = true, Message = "Queries retrieved successfully", Data = queries });
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetQueryById(int id)
        {
            var userId = GetUserId();
            var query = _queryService.GetQueryById(id, userId);
            if(query == null)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Query not found" });
            }
            return Ok(new ApiResponse<object> { Success = true, Message = "Query retrieved successfully", Data = query });
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteQuery(int id)
        {
            var userId = GetUserId();
            var result = _queryService.DeleteQuery(id, userId);
            if(!result)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Query not found" });
            }
            return Ok(new ApiResponse<string> { Success = true, Message = "Query deleted successfully" });
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