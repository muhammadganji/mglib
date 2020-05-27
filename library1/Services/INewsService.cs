using library1.Models;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface INewsService
    {
        Task<PagingList<News>> load(int page);
        Task<PagingList<News>> search(string fromDate1, string todate1, string searchnews, int page = 1);
        News FindById(int Id);
        void update(News news);
        void create(News news);
        void save();
        void delete(News news);
    }
}
