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
    public class SupportRequestRepository : RepositoryBase<SupportRequest>, ISupportRequestRepository
    {
        public SupportRequestRepository(BankingSystemDbContext context) : base(context)
        {
        }


        public async Task<List<SupportRequest>> GetAllAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(x => x.CreatedAt).ToListAsync();

        }

        public async Task<SupportRequest> GetOneByIdAsync(int id, bool trackChanges)
        {
            var supportRequest = await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
            return supportRequest;
        }

        public async Task<SupportRequest> GetOneByIdWithUserDetailsAsync(int id, bool trackChanges)
        {
            return await _dbContext.SupportRequests.Include(s => s.User).SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SupportRequest>> GetAllByUserIdAsync(string userId)
        {
            return await _dbContext.SupportRequests.Where(s => s.UserId == userId).ToListAsync();
        }

        public void CreateOne(SupportRequest supportRequest)
        {
            Create(supportRequest);
        }

        public void DeleteOne(SupportRequest supportRequest)
        {
            Delete(supportRequest);
        }


        public void UpdateOne(SupportRequest supportRequest)
        {
            Update(supportRequest);
        }

       
    }
}
