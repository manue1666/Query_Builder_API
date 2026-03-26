namespace QueryBuilderApi.Models
{
    public class Query
    {
        public int Id { get; set;}
        public int UserId { get; set;}
        public required string Description { get; set; }
        public required string GeneratedSql { get; set; }
        public required int DatabaseId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public User? User { get; set; }
    }
}