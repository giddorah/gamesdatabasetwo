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
            Games.Remove(gameToRemove);

            SaveChanges();
        }


        public void AddGame(CreateGameModel game)
        {
            Games.Add(NewGameConvertedFromCreateGameModelToDbGame(game));
            SaveChanges();
        }
        public Game GameById(int id)
        {
            var gameToReturn = Games.Single(i => i.Id == id);
            gameToReturn.Developer = Developers.Single(i => i.Id == gameToReturn.DeveloperId);             
            gameToReturn.Publisher = Publishers.Single(i => i.Id == gameToReturn.PublisherId);

            return gameToReturn;
        }

        public Game GameByName(string name)
        {
            var gameToReturn = Games.Single(i => i.Name.Contains(name));
            gameToReturn.Developer = Developers.Single(i => i.Id == gameToReturn.DeveloperId);
            gameToReturn.Publisher = Publishers.Single(i => i.Id == gameToReturn.PublisherId);

            return gameToReturn;
        }

        public Game NewGameConvertedFromCreateGameModelToDbGame(CreateGameModel game)
        {
            var developer = Developers.Single(o => o.Name == game.Developer);
            var publisher = Publishers.Single(o => o.Name == game.Publisher);

            var gameToAdd = new Game { Name = game.Name, Genre = game.Genre, Platforms = game.Platforms, ReleasedWhere = game.ReleasedWhere, Theme = game.Theme, Year = game.Year, Developer = developer, DeveloperId = developer.Id, Publisher = publisher, PublisherId = publisher.Id };

            return gameToAdd;
        }

        public ViewGameModel GameByIdConvertedToViewModel(int id)
        {
            return GameConvertFromDbModelToViewModel(GameById(id));
        }

        public ViewGameModel GameConvertFromDbModelToViewModel(Game game)
        {
            game.Developer = Developers.Single(i => i.Id == game.DeveloperId);
            game.Publisher = Publishers.Single(i => i.Id == game.PublisherId);

            var viewGameModel = new ViewGameModel { Name = game.Name, Developer = game.Developer, Genre = game.Genre, Platforms = game.Platforms, Publisher = game.Publisher, ReleasedWhere = game.ReleasedWhere, Theme = game.Theme, Year = game.Year };
            return viewGameModel;
        }

        public List<ViewGameModel> GetAllGamesFromDatabase()
        {
            var listOfGamesWithPublisherAndDeveloper = new List<ViewGameModel>();

            foreach (var game in Games)
            {
                listOfGamesWithPublisherAndDeveloper.Add(GameConvertFromDbModelToViewModel(game));
            }

            return listOfGamesWithPublisherAndDeveloper;
        }

        public void RemoveUser(ApplicationUser user)
        {
           
            Users.Remove(user);
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

        public void EditGame(int id, CreateGameModel gameToEdit)
        {
            var gameAfterEdit = GameById(id);

            gameAfterEdit.Name = gameToEdit.Name;
            gameAfterEdit.Platforms = gameToEdit.Platforms;
            gameAfterEdit.ReleasedWhere = gameToEdit.ReleasedWhere;
            gameAfterEdit.Theme = gameToEdit.Theme;
            gameAfterEdit.Year = gameToEdit.Year;
            gameAfterEdit.Genre = gameToEdit.Genre;
            gameAfterEdit.Developer = Developers.Single(i => i.Name.Equals(gameToEdit.Developer));
            gameAfterEdit.Publisher = Publishers.Single(i => i.Name.Equals(gameToEdit.Publisher));

            Games.Update(gameAfterEdit);
            SaveChanges();
        }
    }
}
