using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmFiesta.Business
{
    public class UsersBusiness : IUsersBusiness
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;
        private readonly IMapper _mapper;

        public UsersBusiness(IUsersRepository usersRepository, ISubscriptionsBusiness subscriptionsBusiness, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _subscriptionsBusiness = subscriptionsBusiness;
            _mapper = mapper;
        }

        public User Get(long Id)
        {
            return _usersRepository.Get(Id);
        }

        public User GetByLogin(string Email, string Password)
        {
            return _usersRepository.GetByLogin(Email, Password);
        }

        public IEnumerable<UserDetail> GetAllUserDetails()
        {
            return _usersRepository.GetAllUserDetails();
        }

        public async Task<User> Add(string Email, string Password, SubscriptionType Subscription)
        {
            // check if there is no user that exists with same credentials
            if (GetByLogin(Email, Password) != null)
            {
                return null;
            }

            int subscriptionType = _mapper.Map<int>(Subscription);
            // create user
            User user = await _usersRepository.Insert(new User
            {
                Name = Email,
                Password = Password,
                Role = "Client",
                IDSubscription = -1
            });

            if (subscriptionType == 0)
            {
                return user;
            }

            Subscription userSubscription = new()
            {
                StartDate = DateTime.Today,
                EndDate = _subscriptionsBusiness.GetEndDate(DateTime.Today, subscriptionType),
            };
            userSubscription = await _subscriptionsBusiness.Add(userSubscription, user.Id);
            user.IDSubscription = userSubscription.Id;
            return await _usersRepository.Update(user);
        }
    }
}
