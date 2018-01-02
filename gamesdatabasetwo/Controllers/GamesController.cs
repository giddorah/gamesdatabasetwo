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
            var addPublishers = new List<Publisher>();
            var addDevelopers = new List<Developer>();

            addPublishers.Add(new Publisher { Id = 0, Name = "Electronic Arts" });
            addPublishers.Add(new Publisher { Id = 1, Name = "Sausage Party" });
            addPublishers.Add(new Publisher { Id = 2, Name = "Inferior Software" });
            addPublishers.Add(new Publisher { Id = 3, Name = "Melissa McGregor Inc." });
            addDevelopers.Add(new Developer { Id = 0, Name = "Microsoft Studios" });
            addDevelopers.Add(new Developer { Id = 1, Name = "Playhouse Productions" });
            addDevelopers.Add(new Developer { Id = 2, Name = "Studio Sentinel" });
            addDevelopers.Add(new Developer { Id = 3, Name = "Assignation Assimilation" });

            int loopNumber = 0;
            int year = 1999;
            for (int i = 0; i < 10; i++)
            {
                if (loopNumber == 4)
                {
                    loopNumber = 0;
                }

                var developer = addDevelopers.Single(o => o.Id == loopNumber);
                var publisher = addPublishers.Single(o => o.Id == loopNumber);

                context.AddGame(new Game { Name = $"Test{i}", Genre = $"TestGenre{i}", Platforms = $"TestPlatform{i}", ReleasedWhere = $"TestWhere{i}", Theme = $"TestTheme{i}", Year = year, Developer = developer, Publisher = publisher });
                year++;
                loopNumber++;
            }
        }
    }
}
