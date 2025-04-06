using LibraryManagement.Repositories.Interface;
using LibraryManagement.Services.Interface;
using LibraryManagement.ViewModels;

public class PenaltyService : IPenaltyService
{
    private readonly IPenaltyRepository _penaltyRepository;

    public PenaltyService(IPenaltyRepository penaltyRepository)
    {
        _penaltyRepository = penaltyRepository;
    }

    public async Task<IEnumerable<PenaltyVm>> GetAllPenaltiesAsync()
    {
        return await _penaltyRepository.GetAllPenaltiesAsync();   
    }

    public async Task<IEnumerable<PenaltyVm>> GetPenaltiesByUserIdAsync(string userId)
    {
        return await _penaltyRepository.GetPenaltiesByUserIdAsync(userId);
    }
}
