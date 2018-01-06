using System;
using gamesdatabasetwo.Data;
using gamesdatabasetwo.Managers;
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
        public IActionResult RemoveGame(string name)
        {
            var gameToRemove = context.GameByName(name);
            context.RemoveGame(gameToRemove.Id);
            return Ok($"Game with the name {gameToRemove.Name} has been removed.");
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
        public IActionResult GetAllGames(bool toggle)
        {
            return Ok(context.GetAllGamesFromDatabase());
        }

        [HttpGet, Route("sortedByName")]
        public IActionResult SortedByName(bool toggle)
        {
            var list = context.GetAllGamesFromDatabase();
            var sortManager = new SortManager(context);
            var sortedList = sortManager.AlphabeticallySortedGames(list, toggle);
            return Ok(sortedList);
        }

        [HttpGet, Route("sortedByYear")]
        public IActionResult SortedByYear(bool toggle)
        {
            var list = context.GetAllGamesFromDatabase();
            var sortManager = new SortManager(context);

            var sortedList = sortManager.GamesSortedByYear(list, toggle);
            return Ok(sortedList);
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
        public IActionResult AddGame(CreateGameModel gameToAdd)
        {
            var gameList = context.GetAllGamesFromDatabase();

            foreach (var game in gameList)
            {
                if (game.Name == gameToAdd.Name)
                {
                    ModelState.AddModelError("error", "Could not add game: Duplicate found. ");
                }
            }

            if (gameToAdd.Year == 0)
            {
                ModelState.AddModelError("error", "Year can not be empty. ");
                return BadRequest(ModelState);
            }
            else if (gameToAdd.Developer == "Choose Developer..." || gameToAdd.Publisher == "Choose Publisher...")
            {
                ModelState.AddModelError("error", "Publisher/Developer can not be empty. ");
                return BadRequest(ModelState);
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                context.AddGame(gameToAdd);
                return Ok(gameToAdd);
            }
        }

        [HttpGet]
        [Route("getpublishers")]
        public IActionResult GetPublishers()
        {
            return Ok(context.AllPublishers());
        }
        [HttpGet]
        [Route("getdevelopers")]
        public IActionResult GetDevelopers()
        {
            return Ok(context.AllDevelopers());
        }
        [HttpGet]
        [Route("getgamebyname")]
        public IActionResult GetGameByName(string name)
        {
            var gottenGame = context.GameByName(name);
            var gameVM = context.GameByIdConvertedToViewModel(gottenGame.Id);

            // This will return a databasemodel. Need to change.
            return Ok(context.GameByName(name));
        }
        [HttpPost]
        [Route("editgame")]
        public IActionResult EditGame(int id, CreateGameModel gameToEdit)
        {
            if (gameToEdit.Year == 0)
            {
                ModelState.AddModelError("error", "Year can not be empty");
                return BadRequest(ModelState);
            }
            else if (ModelState.IsValid)
            {
                context.EditGame(id, gameToEdit);
                return Ok($"Id {id} and {gameToEdit.Developer}");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
