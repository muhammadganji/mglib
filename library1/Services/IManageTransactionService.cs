using library1.Models.ViewModel;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IManageTransactionService
    {
        Task<PagingList<PaymentTransactionViewModel>> load(int page);
        Task<PagingList<PaymentTransactionViewModel>> search(string fromDate1, string todate1, string searchuser, int page);
    }
}
