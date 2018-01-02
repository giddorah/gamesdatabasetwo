using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Game> Games { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public void RemoveGame(int id)
        {
            var gameToRemove = GameById(id);
            //Games.Remove(gameToRemove);

            SaveChanges();
        }


        public void AddGame(Game game)
        {
            Games.Add(game);
            SaveChanges();
        }
        public ViewGameModel GameById(int id)
        {

            var gameToReturn = Games.Single(i => i.Id == id);
            gameToReturn.Developer = Developers.Single(i => i.Id == gameToReturn.DeveloperId);             
            gameToReturn.Publisher = Publishers.Single(i => i.Id == gameToReturn.PublisherId);

            var viewGameModel = new ViewGameModel { Name = gameToReturn.Name, Developer = gameToReturn.Developer.Name, Genre = gameToReturn.Genre, Platforms = gameToReturn.Platforms, Publisher = gameToReturn.Publisher.Name, ReleasedWhere = gameToReturn.ReleasedWhere, Theme = gameToReturn.Theme, Year = gameToReturn.Year };
            return viewGameModel;
        }

        public List<ViewGameModel> GetAllGamesFromDatabase()
        {
            var listOfGamesWithPublisherAndDeveloper = new List<ViewGameModel>();

            foreach (var game in Games)
            {
                
                var gameToList = game;
                gameToList.Developer = Developers.Single(i => i.Id == gameToList.DeveloperId);
                gameToList.Publisher = Publishers.Single(i => i.Id == gameToList.PublisherId);

                var viewGameModel = new ViewGameModel { Name = gameToList.Name, Developer = gameToList.Developer.Name, Genre = gameToList.Genre, Platforms = gameToList.Platforms, Publisher = gameToList.Publisher.Name, ReleasedWhere = gameToList.ReleasedWhere, Theme = gameToList.Theme, Year = gameToList.Year };

                listOfGamesWithPublisherAndDeveloper.Add(viewGameModel);
            }

            return listOfGamesWithPublisherAndDeveloper;
        }

        public void RemoveUser(ApplicationUser user)
        {
           
            Remove(user);
            SaveChanges();
        }

        public void AddDeveloper(Developer developer)
        {
            Developers.Add(developer);
            SaveChanges();
        }

        public void AddPublisher(Publisher publisher)
        {
            Publishers.Add(publisher);
            SaveChanges();
        }

        public List<Developer> AllDevelopers()
        {
            return Developers.ToList();
        }
        public List<Publisher> AllPublishers()
        {
            return Publishers.ToList();
        }

        public void ClearAllDatabases()
        {
            Games.RemoveRange(Games);
            SaveChanges();
            Publishers.RemoveRange(Publishers);
            SaveChanges();
            Developers.RemoveRange(Developers);
            SaveChanges();
        }
    }
}
