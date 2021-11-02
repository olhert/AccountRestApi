using System.Data;
using AccountRestApi.Controllers;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql;

namespace AccountRestApi.DB
{
    public class DbUserStore : IUserStore
    {
        private readonly string _connectionString = null;

        public DbUserStore(string conn)
        {
            _connectionString = conn;
        }
        
        public void Registration(IUser user)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            var sqlQuery = "INSERT INTO users (id, email, password) VALUES(@Id, @Email, @Password)";
            db.Execute(sqlQuery, user);
        }

        public DbUser GetUserByEmail(string email)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.QueryFirstOrDefault<DbUser>("SELECT * FROM users WHERE email = @Email", new {email});
        }
        
        

        
    }
}