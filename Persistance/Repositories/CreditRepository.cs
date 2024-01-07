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
    public class CreditRepository : RepositoryBase<Credit>, ICreditRepository
    {
        public CreditRepository(BankingSystemDbContext context) : base(context)
        {
        }


        public async Task<List<Credit>> GetAllAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }

        public async Task<Credit> GetOneByIdAsync(int id, bool trackChanges)
        {
            return await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }


        public async Task<List<Credit>> GetAllByUserIdAsync(string userId)
        {
           return await _dbContext.Credits.Where(c=>c.UserId == userId).ToListAsync();
        }

        public void CreateOne(Credit credit)
        {
            Create(credit);
        }

        public void DeleteOne(Credit credit)
        {
            Delete(credit);
        }

        public void UpdateOne(Credit credit)
        {
            Update(credit);
        }

        public async Task<List<Credit>> GetAllWithAllDetailsAsync()
        {
           return await _dbContext.Credits.Include(c=>c.User).ThenInclude(u=>u.Accounts).ToListAsync();
        }
    }
}
