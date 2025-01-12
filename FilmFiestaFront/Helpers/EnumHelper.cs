using FilmFiestaFront.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FilmFiestaFront.Helpers
{
    public static class EnumHelper
    {
        public static List<SelectListItem> GetGenreSelectListItems()
        {
            return Enum.GetValues(typeof(GenreType))
                .Cast<GenreType>()
                .Select(g => new SelectListItem { Value = g.ToString(), Text = g.ToString() })
                .ToList();
        }
        public static List<SelectListItem> GetStatutSelectListItems()
        {
            return Enum.GetValues(typeof(StatutType))
                .Cast<StatutType>()
                .Select(g => new SelectListItem { Value = g.ToString(), Text = g.ToString() })
            .ToList();
        }

        public static string ToDescString(this SubscriptionType type)
        {
            return type switch
            {
                SubscriptionType.Free => "Free",
                SubscriptionType.Week => "Week",
                SubscriptionType.Month => "Month",
                SubscriptionType.Year => "Year",
            };
        }
    }
}
