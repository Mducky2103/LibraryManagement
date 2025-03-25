using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<AuthorVm>> GetAllAuthorsAsync()
        {
            var auhtor = await _authorRepository.GetAllAsync();

            return auhtor.Select(a => new AuthorVm
            {
                Id = a.Id,
                Name = a.Name,
                Address = a.Address
            }).ToList();
        }

        public async Task<AuthorVm> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null) return null;

            return new AuthorVm
            {
                Id = author.Id,
                Name = author.Name,
                Address = author.Address
            };
        }

        public async Task AddAuthorAsync(AuthorVm authorVm)
        {
            var author = new Author
            {
                Name = authorVm.Name,
                Address = authorVm.Address
            };

            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAuthorAsync(int id, AuthorVm authorVm)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null) throw new Exception("Not found author.");

            author.Name = authorVm.Name;
            author.Address = authorVm.Address;

            await _authorRepository.UpdateAsync(id, author);
        }

        public async Task DeleteAuthorAsync(int id)
        {
            await _authorRepository.DeleteAsync(id);
        }
    }
}
