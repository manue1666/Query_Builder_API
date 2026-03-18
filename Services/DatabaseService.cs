using QueryBuilderApi.Models;

namespace QueryBuilderApi.Services
{
    public class DatabaseService
    {
        public string CreateDatabase(CreateDatabaseDto dto)
        {
            return $"DB: |{dto.Name}| saved successfuly";
        }
    }
}