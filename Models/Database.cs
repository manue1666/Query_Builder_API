namespace QueryBuilderApi.Models
{
    public class Database
    {
        public int Id {get; set;}
        public int UserId {get; set;}
        public required string Name {get; set;}
        public required string Description {get; set;}
        public required string SqlSchema {get; set;}
        public required DateTime CreatedAt {get; set;}
        public List<Query> Queries { get; set;} = new List<Query>();
        public User? User { get; set; }

    }
}