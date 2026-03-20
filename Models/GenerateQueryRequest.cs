namespace QueryBuilderApi.Models
{
    public class GenerateQueryRequest
    {
        //id of the database to generate query for
        public required int DatabaseId { get; set; }

        //user input for query generation
        public required string Description { get; set; }
    }
}