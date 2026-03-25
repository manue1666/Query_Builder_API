
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


        [HttpPost("generate")]
        public async Task<IActionResult> GenerateQuery([FromBody] GenerateQueryRequest request)
        {
            var result = await _queryService.GenerateQuery(request);
            if(result == null)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Database not found for the provided ID" });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Query successfully generated", Data = result });
        }

        [HttpGet("all/{databaseId}")]
        public IActionResult GetAllQueriesByDatabaseId(int databaseId)
        {
            var queries = _queryService.GetAllQueriesByDatabaseId(databaseId);
            return Ok(new ApiResponse<object> { Success = true, Message = "Queries retrieved successfully", Data = queries });
        }


        [HttpGet("{id}")]
        public IActionResult GetQueryById(int id)
        {
            var query = _queryService.GetQueryById(id);
            if(query == null)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Query not found" });
            }
            return Ok(new ApiResponse<object> { Success = true, Message = "Query retrieved successfully", Data = query });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteQuery(int id)
        {
            var result = _queryService.DeleteQuery(id);
            if(!result)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Query not found" });
            }
            return Ok(new ApiResponse<string> { Success = true, Message = "Query deleted successfully" });
        }
    }
}