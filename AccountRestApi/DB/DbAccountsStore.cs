using System.Collections.Generic;
using System.Data;
using System.Linq;
using AccountRestApi.Controllers;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace AccountRestApi.DB
{
    public class DbAccountsStore : IAccountsStore
    {
        readonly string connectionString = null;

        public DbAccountsStore(string conn)
        {
            connectionString = conn;
        }

        public void RegisterAcc(IAccount account)
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            var sqlQuery = "INSERT INTO accounts_table (id, name, balance, currency) VALUES(@Id, @Name, @Balance, @Currency)";
            db.Execute(sqlQuery, account);
        }

        public IEnumerable<IAccount> GetAllAccounts()
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            return db.Query<DbAccount>("SELECT * FROM accounts_table").ToList();
        }

        public IAccount GetAccById(string idOfAccount)
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            return db.QueryFirst<DbAccount>("SELECT * FROM accounts_table WHERE id = @idOfAccount",new {idOfAccount});
        }
        
        public IEnumerable<IAccount> GetAccByName(string nameOfAccount)
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            return db.Query<DbAccount>("SELECT * FROM accounts_table WHERE name =@nameOfAccount",new {nameOfAccount});
        }

        public IAccount Deposit(string accountId, double sumOfDeposit)
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            return db.QueryFirst<DbAccount>("select id, name, deposit(@accountId, @sumOfDeposit) as balance, currency from accounts_table where id = @accountId", 
                new
            {
                accountId = accountId,
                sumOfDeposit = sumOfDeposit
            });
        }

        public IAccount Withdrawal(string accountId, double sumOfWithdrawal)
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            var sql =
                "select id, name, withdrawal(@accountId, @sumOfWithdrawal) as balance, currency from accounts_table where id = @accountId";

            var par =new 
            {
                accountId = accountId,
                sumOfWithdrawal = sumOfWithdrawal
            };
            
            return db.QueryFirst<DbAccount>(sql, par);
        }

        public void Transfer(string senderAccId, string recipientAccId, double sumOfTransfer)
        {
            using IDbConnection db = new NpgsqlConnection(connectionString);
            var sqlQuery = "select transfer(@senderAccId, @recipientAccId, @sumOfTransfer)";
            var par = new
            {
                senderAccId = senderAccId,
                recipientAccId = recipientAccId,
                sumOfTransfer = sumOfTransfer
            };
            db.Execute(sqlQuery, par);
        }
        
    }
}