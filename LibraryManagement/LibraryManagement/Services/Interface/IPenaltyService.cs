using LibraryManagement.ViewModels;

namespace LibraryManagement.Services.Interface
{
    public interface IPenaltyService
    {
        Task<IEnumerable<PenaltyVm>> GetAllPenaltiesAsync();
        Task<IEnumerable<PenaltyVm>> GetPenaltiesByUserIdAsync(string userId);
    }
}
