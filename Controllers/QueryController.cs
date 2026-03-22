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
        public IActionResult GenerateQuery([FromBody] GenerateQueryRequest request)
        {
            var result = _queryService.GenerateQuery(request);
            if(result == null)
            {
                return NotFound(new { message = "database for query not found" });
            }

            return Ok(new {message = "Query successfully generated", data = result});
        }

        [HttpGet("all/{databaseId}")]
        public IActionResult GetAllQueriesByDatabaseId(int databaseId)
        {
            var queries = _queryService.GetAllQueriesByDatabaseId(databaseId);
            return Ok(new { message = "Queries retrieved successfully", data = queries });
        }

        [HttpGet("{id}")]
        public IActionResult GetQueryById(int id)
        {
            var query = _queryService.GetQueryById(id);
            if(query == null)
            {
                return NotFound(new { message = "Query not found" });
            }
            return Ok(new { message = "Query retrieved successfully", data = query });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuery(int id)
        {
            var result = _queryService.DeleteQuery(id);
            if(!result)
            {
                return NotFound(new { message = "Query not found" });
            }
            return Ok(new { message = "Query deleted successfully" });
        }
    }
}