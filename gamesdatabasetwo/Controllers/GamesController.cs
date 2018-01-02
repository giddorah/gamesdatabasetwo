using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gamesdatabasetwo.Data;
using gamesdatabasetwo.Other;
using Microsoft.AspNetCore.Mvc;

namespace gamesdatabasetwo.Controllers
{
    [Route("api/Games")]
    public class GamesController : Controller
    {
        private ApplicationDbContext context;
        private Repository repository = new Repository();

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
            try
            {
                var result = context.GameByIdConvertedToViewModel(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("No game by that Id found.");
            }
            
        }
        [HttpGet]
        [Route("getallgames")]
        public IActionResult GetAllGames()
        {
            return Ok(context.GetAllGamesFromDatabase());
        }


        [HttpGet]
        [Route("refilldatabase")]
        public IActionResult RefillDatabase()
        {
            repository.DoRefillDatabase(context);
            return Ok("Database refilled");
        }

        [HttpPost]
        [Route("addgame")]
        public IActionResult AddGame(Game GameToAdd)
        {
            //context.AddGame(GameToAdd);
            return Ok(GameToAdd);
        }
    }
}
