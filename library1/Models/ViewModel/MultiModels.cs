using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models.ViewModel
{
    public class MultiModels
    {
        public List<Book> _LastMoreVisit { get; set; }

        public List<Book> listShop { get; set; }

        public List<BookListViewModel> _SliderLastBook = new List<BookListViewModel>();

        public List<Book> moreViewBook { get; set; }

        public List<ApplicationUser> _LastRegisterUser { get; set; }

        public List<News> _LastNews { get; set; }

        public List<BookDetailViewModel> BookDetail { get; set; }
    }
}
