using library1.Models.ViewModel;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services.User
{
    public interface ITransactionService
    {
        Task<PagingList<PaymentTransactionViewModel>> load(int page);
        Task<PagingList<PaymentTransactionViewModel>> SearchInPaymentTransaction(string fromDate1, string todate1, int page);

    }
}
