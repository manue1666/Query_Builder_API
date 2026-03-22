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

        public async Task<Query?> GenerateQuery(GenerateQueryRequest request)
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
                CreatedAt = DateTime.UtcNow
            };
            _dbcontext.Queries.Add(query);
            _dbcontext.SaveChanges();
            return query;
        }

        public List<Query> GetAllQueriesByDatabaseId(int databaseId)
        {
            var queries = _dbcontext.Queries.Where(q => q.DatabaseId == databaseId).ToList();
            return queries;
        }

        public Query? GetQueryById(int id)
        {
            var query = _dbcontext.Queries.FirstOrDefault(q => q.Id == id);
            return query;
        }

        public bool DeleteQuery(int id)
        {
            var query = _dbcontext.Queries.FirstOrDefault(q => q.Id == id);
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