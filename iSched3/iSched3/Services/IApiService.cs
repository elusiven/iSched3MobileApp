using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSched3.Services
{
    public interface IApiService
    {
        Task<bool> LoginAsync(string userName, string password);
        Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password);
        Task<bool> RenewAccessToken();
    }
}
