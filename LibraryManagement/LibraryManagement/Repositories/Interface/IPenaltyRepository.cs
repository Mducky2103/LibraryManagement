using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Repositories.Interface
{
    public interface IPenaltyRepository
    {
        Task AddPenaltyAsync(Penalty penalty);
        Task<IEnumerable<PenaltyVm>> GetAllPenaltiesAsync();
        Task<IEnumerable<PenaltyVm>> GetPenaltiesByUserIdAsync(string userId);
    }
}
