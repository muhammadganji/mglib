using library1.Models;
using library1.Models.ViewModel;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services.User
{
    public interface IUserProfileService
    {
        Task<PagingList<ManageRequestBookViewModel>> load(int page);
        Task<PagingList<ManageRequestBookViewModel>> searchInRequest(string fromDate1, string todate1, string bookname, int page);
        void ConfirmMobile(ApplicationUser user);
        void updateProfile(UserListViewModel user);
        void save();

    }
}
