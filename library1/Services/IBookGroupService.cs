using library1.Models;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IBookGroupService
    {
        Task<PagingList<BookGroup>> load(int page);
        BookGroup findById(int id);
        Task<PagingList<BookGroup>> search(string searchbookgroup, int page);
        void create(BookGroup bookgroup);
        void update(BookGroup bookgroup);
        void delete(BookGroup bookgroup);
        void save();

    }
}
