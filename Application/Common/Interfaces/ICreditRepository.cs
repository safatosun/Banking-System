using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICreditRepository : IRepositoryBase<Credit>
    {
        Task<List<Credit>> GetAllAsync(bool trackChanges);
        Task<List<Credit>> GetAllWithAllDetailsAsync();
        Task<List<Credit>> GetAllByUserIdAsync(string userId);
        Task<Credit> GetOneByIdAsync(int id, bool trackChanges);
        void CreateOne(Credit credit);
        void UpdateOne(Credit credit);
        void DeleteOne(Credit credit);
    }
}
