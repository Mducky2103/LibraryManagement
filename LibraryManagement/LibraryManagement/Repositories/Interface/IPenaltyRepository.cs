using LibraryManagement.Models;

namespace LibraryManagement.Repositories.Interface
{
    public interface IPenaltyRepository
    {
        Task AddPenaltyAsync(Penalty penalty);
    }
}
