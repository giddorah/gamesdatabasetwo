﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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


        public void AddGame(Game game)
        {
            Games.Add(game);
            SaveChanges();
        }
        public Game GameById(int id)
        {

            var gameToReturn = Games.Single(i => i.Id == id);
            gameToReturn.Developer = Developers.Single(i => i.Id == gameToReturn.Developer.Id);             
            gameToReturn.Publisher = Publishers.Single(i => i.Id == gameToReturn.Publisher.Id);
            return gameToReturn;
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
