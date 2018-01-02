using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //public void Remove(int id)
        //{
        //    var gameToRemove = ById(id);
        //    Game.Remove(userToRemove);

        //    SaveChanges();
        //}


        //public void AddCustomer(Game game)
        //{ 
        //    Game.Add(game);
        //    SaveChanges();
        //}
        //public Customer ById(int id)
        //{
        //    var customerToReturn = Customer.Single(i => i.Id == id);

        //    return customerToReturn;
        //}
    }
}
