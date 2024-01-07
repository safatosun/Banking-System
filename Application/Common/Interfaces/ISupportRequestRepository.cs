using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISupportRequestRepository : IRepositoryBase<SupportRequest>
    {
        Task<List<SupportRequest>> GetAllAsync(bool trackChanges);
        Task<SupportRequest> GetOneByIdAsync(int id, bool trackChanges);
        Task<SupportRequest> GetOneByIdWithUserDetailsAsync(int id, bool trackChanges);
        Task<List<SupportRequest>> GetAllByUserIdAsync(string userId);
        void CreateOne(SupportRequest supportRequest);
        void UpdateOne(SupportRequest supportRequest);
        void DeleteOne(SupportRequest supportRequest);
    }
}
