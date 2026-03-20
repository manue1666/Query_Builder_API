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
        public string CreateDatabase(CreateDatabaseDto dto)
        {
            var database = new Database
            {
                Name = dto.Name,
                Description = dto.Description,
                SqlSchema = dto.SqlSchema,
                CreatedAt = DateTime.UtcNow
            };
            _dbcontext.Databases.Add(database);
            _dbcontext.SaveChanges();
            return $"DB: |{database.Name}| saved successfully with id: {database.Id}";
        }

        public List<Database> GetAllDatabases()
        {
            var databases = _dbcontext.Databases.ToList();
            return databases;
        }
        
        public Database? GetDatabaseById(int id)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == id);
            return database;
        }

        public bool DeleteDatabase(int id)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == id);
            if (database == null)
            {
                return false;
            }
            _dbcontext.Databases.Remove(database);
            _dbcontext.SaveChanges();
            return true;
        }

        public bool UpdateDatabase(int id, UpdateDatabaseDto dto)
        {
            var database = _dbcontext.Databases.FirstOrDefault(db => db.Id == id);
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