using library1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> PagingAuthorListAsync(int page);

        Task<List<Author>> SearchAuthorService(string searchauthor, int page);

        Task<Author> GetAuthorByIdAsync(int Id);

        void InsertAuthor(Author author);

        void EditAuthor(Author author);

        void DeleteAuthor(Author author);

        void Save();

    }
}
