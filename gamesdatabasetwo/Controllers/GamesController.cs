using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gamesdatabasetwo.Data;
using Microsoft.AspNetCore.Mvc;

namespace gamesdatabasetwo.Controllers
{
    [Route("api/Games")]
    public class GamesController : Controller
    {

        private ApplicationDbContext context;

        public GamesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("This is okay");
        }

        [HttpPost]
        [Route("removegame")]
        public IActionResult RemoveGame(int id)
        {
            context.RemoveGame(id);
            return Ok($"Game with the id {id} has been removed.");
        }

        [HttpGet]
        [Route("getspecificgame")]
        public IActionResult GetSpecificGame(int id)
        {
            return Ok(context.GameById(id));
        }

        [HttpGet]
        [Route("refilldatabase")]
        public IActionResult RefillDatabase()
        {
            DoRefillDatabase();
            return Ok("Database refilled");
        }

        [HttpPost]
        [Route("addgame")]
        public IActionResult AddGame(Game GameToAdd)
        {
            context.AddGame(GameToAdd);
            return Ok($"Game {GameToAdd.Name} has been added.");
        }

        public void DoRefillDatabase()
        {
            context.ClearAllDatabases();
            context.AddPublisher(new Publisher { Name = "Electronic Arts" });
            context.AddPublisher(new Publisher { Name = "Sausage Party" });
            context.AddPublisher(new Publisher { Name = "Inferior Software" });
            context.AddPublisher(new Publisher { Name = "Melissa McGregor Inc." });
            context.AddDeveloper(new Developer { Name = "Microsoft Studios" });
            context.AddDeveloper(new Developer { Name = "Playhouse Productions" });
            context.AddDeveloper(new Developer { Name = "Studio Sentinel" });
            context.AddDeveloper(new Developer { Name = "Assignation Assimilation" });

            var addPublishers = context.AllPublishers();
            var addDevelopers = context.AllDevelopers();



            int loopNumber = 1;
            int year = 1999;
            for (int i = 0; i < 10; i++)
            {
                if (loopNumber == 4)
                {
                    loopNumber = 0;
                }

                int randomDeveloper = RandomGenerator(addDevelopers.First().Id, addDevelopers.Last().Id);
                int randomPublisher = RandomGenerator(addPublishers.First().Id, addPublishers.Last().Id);

                var developer = addDevelopers.Single(o => o.Id == randomDeveloper );
                var publisher = addPublishers.Single(o => o.Id == randomPublisher );

                context.AddGame(new Game { Name = $"Test{i}", Genre = $"TestGenre{i}", Platforms = $"TestPlatform{i}", ReleasedWhere = $"TestWhere{i}", Theme = $"TestTheme{i}", Year = year, Developer = developer, Publisher = publisher });
                year++;
                loopNumber++;
            }
        }

        public int RandomGenerator(int firstNumber, int secondNumber)
        {
            var random = new Random();
            return(random.Next(firstNumber, secondNumber + 1));

        }
    }
}
