using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using System;
using System.Collections.Generic;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface ISubscriptionsRepository : IRepository<TSubscriptions, Subscription>
    {
        Subscription Get(long id);

        Subscription GetByUser(long userId);

        IEnumerable<Subscription> GetAll();

        IEnumerable<Subscription> GetAllInInterval(DateTime start, DateTime end);
    }
}
