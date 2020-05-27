using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Services.User
{
    public interface IPaymentService
    {
        bool PaymentVerify(string UserId, string email, string mobile, string description, int amount, string TransactionNo);
    }
}
