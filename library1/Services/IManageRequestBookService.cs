using library1.Models.ViewModel;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services
{
    public interface IManageRequestBookService
    {
        Task<PagingList<ManageRequestBookViewModel>> load(int page);
        Task<PagingList<ManageRequestBookViewModel>> search(string fromDate1, string todate1, string searchuser, int page);
        List<ManageRequestBookViewModel> RejectRequest(int id);
        void RejectRequestConfirm(int id);
        List<ManageRequestBookViewModel> AcceptRequest(int id);
        void AcceptRequsetConfirm(int id);
        List<ManageRequestBookViewModel> givebackRequest(int id);
        void givebackRequestConfirm(int id);


    }
}
