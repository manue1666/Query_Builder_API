namespace QueryBuilderApi.Models
{
    public class AuthResponse
    {
        public required string Token { get; set; }
        public required string Message { get; set; }
        public required User User { get; set; }
    }
}