using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories.Interface;

namespace LibraryManagement.Repositories
{
    public class PenaltyRepository : IPenaltyRepository
    {
        private readonly LibraryDbContext _context;

        public PenaltyRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddPenaltyAsync(Penalty penalty)
        {
            if (penalty == null)
            {
                throw new ArgumentNullException(nameof(penalty), "Penalty object is null before adding to database.");
            }

            _context.Penalties.Add(penalty);
            await _context.SaveChangesAsync();
        }

    }
}
