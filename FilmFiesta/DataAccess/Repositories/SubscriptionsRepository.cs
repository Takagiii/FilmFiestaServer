using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.DataAccess.Repositories
{
    public class SubscriptionsRepository : Repository<TSubscriptions, Subscription>, ISubscriptionsRepository
    {
        public SubscriptionsRepository(FilmFiestaContext context, ILogger<SubscriptionsRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public Subscription Get(long id)
        {
            return _mapper.Map<Subscription>(_context.TSubscriptions.Find(id));
        }

        public Subscription GetByUser(long userId)
        {
            var subscriptionUser = (from subscription in _context.TSubscriptions
                                    join user in _context.TUsers on subscription.Id equals user.IDSubscription
                                    where user.Id == userId
                                    select new
                                    {
                                        subscription.Id,
                                        subscription.StartDate,
                                        subscription.EndDate,
                                    }).FirstOrDefault();

            return subscriptionUser == null ? null : new Subscription()
            {
                Id = subscriptionUser.Id,
                StartDate = subscriptionUser.StartDate,
                EndDate = subscriptionUser.EndDate,
            };
        }

        public IEnumerable<Subscription> GetAll()
        {
            return _mapper.Map<IEnumerable<Subscription>>(_context.TSubscriptions.ToList());
        }

        public IEnumerable<Subscription> GetAllInInterval(DateTime start, DateTime end)
        {
            // get subscriptions if startDate < start or endDate > end,
            // in which case the subscription is active for some of the time selected
            IEnumerable<TSubscriptions> subscriptions = (from subscription in _context.TSubscriptions
                                                         where DateTime.Compare(subscription.StartDate, start) < 0 ||
                                                         DateTime.Compare(subscription.EndDate, end) > 0
                                                         select subscription).ToList();
            return _mapper.Map<IEnumerable<Subscription>>(subscriptions);
        }
    }
}
