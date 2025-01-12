using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.DataAccess.Repositories
{
    public class UsersRepository : Repository<TUsers, User>, IUsersRepository
    {
        public UsersRepository(FilmFiestaContext context, ILogger<UsersRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public User Get(long Id)
        {
            return _mapper.Map<User>(_context.TUsers.Find(Id));
        }

        public IEnumerable<User> GetAll()
        {
            return _mapper.Map<IEnumerable<User>>(_context.TUsers.ToList());
        }

        public IEnumerable<UserDetail> GetAllUserDetails()
        {
            return (from user in _context.TUsers
                    join subscription in _context.TSubscriptions on user.IDSubscription equals subscription.Id
                    select new UserDetail
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Role = user.Role,
                        EndDate = subscription.EndDate,
                    });
        }

        public User GetByLogin(string Email, string Password) 
        {
            TUsers user = _context.TUsers.Where((TUsers user) => user.Name == Email && user.Password == Password).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<User>(user);
        }
    }
}
