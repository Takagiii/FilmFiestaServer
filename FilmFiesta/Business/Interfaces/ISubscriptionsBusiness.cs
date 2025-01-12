using FilmFiesta.Dbo;
using FilmFiesta.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business.Interfaces
{
    public interface ISubscriptionsBusiness
    {
        Subscription GetWithUserId(long userId);

        Subscription Get(long id);

        IEnumerable<Subscription> GetAll();

        IEnumerable<Subscription> GetAllInInterval(DateTime start, DateTime end);

        Task<Subscription> Add(Subscription subscription, long IdUser);

        Task<bool> Delete(long id, long idUser);

        DateTime GetEndDate(DateTime startDate, int subscriptionType);

        SubscriptionType GetSubscriptionType(DateTime start, DateTime end);
    }
}
