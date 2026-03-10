
//User Database DTO
namespace QueryBuilderApi.Models
{
    public class CreateDatabaseDto
    {
        public required string Name  { get; set; }
        public required string Description { get; set; }
        public required string SqlSchema { get; set; }

    }
}