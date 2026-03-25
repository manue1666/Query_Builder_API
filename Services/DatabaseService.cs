using Microsoft.AspNetCore.Http.HttpResults;
using QueryBuilderApi.Data;
using QueryBuilderApi.Models;

namespace QueryBuilderApi.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _dbcontext;

        public DatabaseService(AppDbContext context)
        {
            _dbcontext = context;
        }
        public string CreateDatabase(int userId, CreateDatabaseDto dto)
        {
            var database = new Database
            {
                Name = dto.Name,
                Description = dto.Description,
                SqlSchema = dto.SqlSchema,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            _dbcontext.Databases.Add(database);
            _dbcontext.SaveChanges();
            return $"DB: |{database.Name}| saved successfully with id: {database.Id}";
        }

        public List<Database> GetAllDatabases(int userId)
        {
            var databases = _dbcontext.Databases.Where(db => db.UserId == userId).ToList();
            return databases;
        }
        
        public Database? GetDatabaseById(int id, int userId)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == id && db.UserId == userId);
            return database;
        }

        public bool DeleteDatabase(int id, int userId)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == id && db.UserId == userId);
            if (database == null)
            {
                return false;
            }
            _dbcontext.Databases.Remove(database);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateDatabase(int id, int userId, UpdateDatabaseDto dto)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == id && db.UserId == userId);
            if(database == null)
            {
                return false;
            }
            database.Name = dto.Name;
            database.Description = dto.Description;
            database.SqlSchema = dto.SqlSchema;
            _dbcontext.SaveChanges();
            return true;
        }
    }
}