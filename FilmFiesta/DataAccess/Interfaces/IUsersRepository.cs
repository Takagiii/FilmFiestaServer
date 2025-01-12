using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using System.Collections.Generic;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface IUsersRepository : IRepository<TUsers, User>
    {
        User Get(long Id);

        IEnumerable<User> GetAll();

        IEnumerable<UserDetail> GetAllUserDetails();

        User GetByLogin(string Email, string Password);
    }
}
