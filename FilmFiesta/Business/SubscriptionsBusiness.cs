using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business
{
    public class SubscriptionsBusiness : ISubscriptionsBusiness
    {
        private readonly ISubscriptionsRepository _subscriptionsRepository;
        private readonly IUsersRepository _usersRepository;

        public SubscriptionsBusiness(ISubscriptionsRepository subscriptionsRepository, IUsersRepository usersRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _usersRepository = usersRepository;
        }

        public Subscription GetWithUserId(long userId)
        {
            return _subscriptionsRepository.GetByUser(userId);
        }

        public Subscription Get(long id)
        {
            return _subscriptionsRepository.Get(id);
        }

        public IEnumerable<Subscription> GetAll()
        {
            return _subscriptionsRepository.GetAll();
        }

        public IEnumerable<Subscription> GetAllInInterval(DateTime start, DateTime end)
        {
            return _subscriptionsRepository.GetAllInInterval(start, end);
        }

        public async Task<Subscription> Add(Subscription subscription, long IdUser)
        {
            User user = _usersRepository.Get(IdUser);
            if (user == null)
            {
                return null; // no user
            }

            Subscription subscript = _subscriptionsRepository.GetByUser(IdUser);
            if (subscript != null)
            {
                return null; // already has subscription
            }

            // valid
            Subscription newSubscription = await _subscriptionsRepository.Insert(subscription);
            user.IDSubscription = newSubscription.Id;
            _ = await _usersRepository.Update(user);
            return newSubscription;
        }

        public Task<bool> Delete(long id, long idUser)
        {
            Subscription sub = _subscriptionsRepository.GetByUser(idUser);
            return sub == null || sub.Id != id ? null : _subscriptionsRepository.Delete(id);
        }

        public DateTime GetEndDate(DateTime startDate, int subscriptionType)
        {
            return subscriptionType switch
            {
                1 => startDate.Add(new TimeSpan(7, 0, 0, 0)),
                2 => startDate.Add(new TimeSpan(31, 0, 0, 0)),
                _ => startDate.Add(new TimeSpan(365, 0, 0, 0)),
            };
        }

        public SubscriptionType GetSubscriptionType(DateTime start, DateTime end)
        {
            return (start - end) switch
            {
                var t when t.Days >= 365 => SubscriptionType.Year,
                var t when t.Days > 8 => SubscriptionType.Month,
                var t when t.Days > 0 => SubscriptionType.Week,
                _ => SubscriptionType.Free,
            };
        }
    }
}
