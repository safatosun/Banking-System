using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(BankingSystemDbContext context) : base(context)
        {

        }


        public async Task<List<Account>> GetAllAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(x => x.Id).ToListAsync();
        }

        public void CreateOne(Account account)
        {
            Create(account);
        }

        public void DeleteOne(Account account)
        {
            Delete(account);    
        }

        public async Task<Account> GetByUserIdAndAccountNameWithUserDetailsAsync(string userId, string accountName, bool trackChanges)
        {
            var account = await _dbContext.Accounts.Include(a => a.User).SingleOrDefaultAsync( a=>a.UserId == userId && a.Name == accountName);
            return account;
        }

        public async Task<Account> GetOneByIdAsync(int id, bool trackChanges)
        {
           var account = await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

           return account;
        }

        public void UpdateOne(Account account)
        {
            Update(account);
        }
    }
}
