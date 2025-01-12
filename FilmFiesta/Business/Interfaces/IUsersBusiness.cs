using FilmFiesta.Dbo;
using FilmFiesta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business.Interfaces
{
    public interface IUsersBusiness
    {
        User Get(long Id);

        User GetByLogin(string Email, string Password);

        IEnumerable<UserDetail> GetAllUserDetails();

        Task<User> Add(string Email, string Password, SubscriptionType subscription);
    }
}
