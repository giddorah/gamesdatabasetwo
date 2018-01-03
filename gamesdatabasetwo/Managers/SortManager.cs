using gamesdatabasetwo.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Managers
{
    public class SortManager
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SortManager(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        internal IOrderedEnumerable<ApplicationUser> AlphabeticallySortedUsers(List<ApplicationUser> users)
        {
            var sortedList = users.OrderBy(o => o.Email);
            return sortedList;
        }

        internal IOrderedEnumerable<Game> AlphabeticallySortedGames(List<Game> games)
        {
            var sortedList = games.OrderBy(o => o.Name);
            return sortedList;
        }

        internal IOrderedEnumerable<Game> AlphabeticallySortedGenres(List<Game> games)
        {
            var sortedList = games.OrderBy(o => o.Genre);
            return sortedList;
        }



    }
}
