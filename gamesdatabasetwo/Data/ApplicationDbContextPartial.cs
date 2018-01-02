using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public partial class ApplicationDbContextPartial : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Game> Games { get; set; }


        public void RemoveGame(int id)
        {
            var gameToRemove = GameById(id);
            Games.Remove(gameToRemove);

            SaveChanges();
        }


        public void AddGame(Game game)
        {
            Games.Add(game);
            SaveChanges();
        }
        public Game GameById(int id)
        {
            var gameToReturn = Games.Single(i => i.Id == id);

            return gameToReturn;
        }

        public void RemoveUser(ApplicationUser user)
        {
            Remove(user);
            SaveChanges();
        }
    }
}
