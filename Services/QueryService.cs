using QueryBuilderApi.Data;
using QueryBuilderApi.Models;

namespace QueryBuilderApi.Services
{
    public class QueryService
    {
        private readonly AppDbContext _dbcontext;

        public QueryService(AppDbContext context)
        {
            _dbcontext = context;
        }

        public Query? GenerateQuery(GenerateQueryRequest request)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == request.DatabaseId);
            if(database == null)
            {
                return null;
            }
            var query = new Query
            {
                Description = request.Description,
                GeneratedSql = $"-- SQL query generated for database: {database.Name} based on description: {request.Description}",
                DatabaseId = request.DatabaseId,
                CreatedAt = DateTime.UtcNow
            };
            _dbcontext.Queries.Add(query);
            _dbcontext.SaveChanges();
            return query;
        }
    }
}