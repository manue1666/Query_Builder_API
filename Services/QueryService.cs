using System.Threading.Tasks;
using QueryBuilderApi.Data;
using QueryBuilderApi.Models;

namespace QueryBuilderApi.Services
{
    public class QueryService
    {
        private readonly AppDbContext _dbcontext;
        private readonly GroqService _groqService;

        public QueryService(AppDbContext context, GroqService groqService)
        {
            _dbcontext = context;
            _groqService = groqService;
        }

        public async Task<Query?> GenerateQuery(GenerateQueryRequest request, int userId)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == request.DatabaseId);
            if(database == null)
            {
                return null;
            }
            var generatedSql = await _groqService.GenerateSqlFromDescription(database.SqlSchema, request.Description);
            var query = new Query
            {
                Description = request.Description,
                GeneratedSql = generatedSql,
                DatabaseId = request.DatabaseId,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            _dbcontext.Queries.Add(query);
            _dbcontext.SaveChanges();
            return query;
        }

        public List<Query> GetAllQueriesByDatabaseId(int databaseId, int userId)
        {
            var queries = _dbcontext.Queries.Where(q => q.DatabaseId == databaseId && q.UserId == userId).ToList();
            return queries;
        }

        public Query? GetQueryById(int id, int userId)
        {
            var query = _dbcontext.Queries.FirstOrDefault(q => q.Id == id && q.UserId == userId);
            return query;
        }

        public bool DeleteQuery(int id, int userId  )
        {
            var query = _dbcontext.Queries.FirstOrDefault(q => q.Id == id && q.UserId == userId);
            if(query == null)
            {
                return false;
            }
            _dbcontext.Queries.Remove(query);
            _dbcontext.SaveChanges();
            return true;
        }
    }
}