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

        internal IOrderedEnumerable<UserVM> AlphabeticallySortedUsers(List<UserVM> users)
        {
            var sortedList = users.OrderBy(o => o.Email);
            return sortedList;
        }

        internal IOrderedEnumerable<ViewGameModel> AlphabeticallySortedGames(List<ViewGameModel> games, bool toggle)
        {
            if (toggle)
            {
                var sortedList = games.OrderBy(o => o.Name);
                return sortedList;
            }
            var sortedListDescending = games.OrderByDescending(o => o.Name);
            return sortedListDescending;
           
        }

      
        

        internal IOrderedEnumerable<ViewGameModel> GamesSortedByYear(List<ViewGameModel> games, bool toggle)
        {
            if (toggle)
            {
            var sortedList = games.OrderBy(o => o.Year);
            return sortedList;
            }
            var sortedListDescending = games.OrderByDescending(o => o.Year);
            return sortedListDescending;
        }

       
    }
}
