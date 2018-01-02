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

        [HttpPost]
        [Route("addgame")]
        public IActionResult AddGame(Game GameToAdd)
        {
            context.AddGame(GameToAdd);
            return Ok($"Game {GameToAdd.Name} has been added.");
        }
    }
}
