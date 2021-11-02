using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AccountRestApi.Controllers
{
    public interface IAccountsStore
    {
        void RegisterAcc(IAccount acc);

        IEnumerable<IAccount> GetAllAccounts();

        IAccount GetAccById(string idOfAccount);

        IEnumerable<IAccount> GetAccByName(string nameOfAccount);

        IAccount Deposit(string accountId, double sumOfDeposit);

        IAccount Withdrawal(string accountId, double sumOfWithdrawal);

        void Transfer(string senderAccId, string recipientAccId, double sumOfTransfer);
    }
    
    public class AccountsStore : IAccountsStore
    {
        private readonly Dictionary<string, IAccount> _accounts = new ();

        public void RegisterAcc(IAccount account)
        {
            _accounts.Add(account.Id, account);
        }
        
        public IEnumerable<IAccount> GetAllAccounts()
        {
            return _accounts.Values;
        }
        
        public IAccount GetAccById(string idOfAccount)
        {
            return _accounts.ContainsKey(idOfAccount) ? _accounts[idOfAccount] : null;
        }
        
        public IEnumerable<IAccount> GetAccByName(string nameOfAccount)
        {
            return _accounts.Values.Where(itm => itm.Name == nameOfAccount);
        }
        
        public IAccount Deposit(string accountId, double sumOfDeposit)
        {
            var account = _accounts.FirstOrDefault(itm => itm.Key == accountId).Value;
            
            if (account != null && sumOfDeposit > 0)
                account.Balance += sumOfDeposit;
            return account;
        }
        
        public IAccount Withdrawal(string accountId, double sumOfWithdrawal)
        {
            var account = _accounts.FirstOrDefault(itm => itm.Key == accountId).Value;
            
            if (account != null && sumOfWithdrawal > 0)
                account.Balance -= sumOfWithdrawal;
            
            return account;
        }
        

        public void Transfer(string senderAccId, string recipientAccId, double sumOfTransfer)
        {
            var senderAcc = _accounts.FirstOrDefault(itm => itm.Key == senderAccId).Value;
            var recipientAcc = _accounts.FirstOrDefault(itm => itm.Key == recipientAccId).Value;

            if (senderAcc == null || recipientAcc == null)
                return;

            if (senderAcc.Balance <= 0)
                return;

            if (sumOfTransfer < 0)
                return;
            
            senderAcc.Balance -= sumOfTransfer;
            recipientAcc.Balance += sumOfTransfer;
        }
    }
}