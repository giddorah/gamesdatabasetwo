using System;
using System.Security.Claims;
using gamesdatabasetwo.Data;
using gamesdatabasetwo.Managers;
using gamesdatabasetwo.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gamesdatabasetwo.Controllers
{
    [Route("api/Games")]
    public class GamesController : Controller
    {
        private ApplicationDbContext context;
        private Repository repository = new Repository();
       
        public string UserId { get; set; }

        public GamesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            try
            {
            UserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            }
            catch (Exception)
            {

            }
            //UserId = userId;
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("This is okay");
        }

        [Authorize(Roles = "Admin")]
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
            var result = new ViewGameModel();
            try
            {
                result = context.GameByIdConvertedToViewModel(id);

                if (context.CheckIfUserHasAlreadyVoted(UserId, id))
                {
                    result.Score.Id = -1;
                    return Ok(result);
                }

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
            //return Ok(LoggedInUser);
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("refilldatabase")]
        public IActionResult RefillDatabase()
        {
            repository.DoRefillDatabase(context);
            return Ok("Database refilled");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("cleardatabase")]
        public IActionResult ClearDatabase()
        {
            context.ClearAllDatabases();
            return Ok("Databases cleared");
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [Route("addpublisher")]
        public IActionResult AddPublisher(PublisherCreateModel publisherToAdd)
        {
            var publisherList = context.AllPublishers();

            foreach (var publisher in publisherList)
            {
                if (publisher.Name == publisherToAdd.Name)
                {
                    ModelState.AddModelError("error", "Could not add publisher: Duplicate found. ");
                }
            }

            if (ModelState.IsValid)
            {
                var publisherToAddDbModel = new Publisher();
                publisherToAddDbModel.Name = publisherToAdd.Name;
                context.AddPublisher(publisherToAddDbModel);
                return Ok(publisherToAdd);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [Route("adddeveloper")]
        public IActionResult AddDeveloper(DeveloperCreateModel developerToAdd)
        {
            var developerList = context.AllDevelopers();

            foreach (var developer in developerList)
            {
                if (developer.Name == developerToAdd.Name)
                {
                    ModelState.AddModelError("error", "Could not add developer: Duplicate found. ");
                }
            }

            if (ModelState.IsValid)
            {
                var developerToAddDbModel = new Developer();
                developerToAddDbModel.Name = developerToAdd.Name;
                context.AddDeveloper(developerToAddDbModel);
                return Ok(developerToAdd);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [Authorize(Roles = "Admin, Staff")]
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
                return Ok($"Added {gameToAdd.Name}");
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
            var result = context.GameByNameConvertedToEditModel(name);
            var dbModel = context.GameByName(name);


            if (context.CheckIfUserHasAlreadyVoted(UserId, dbModel.Id))
            {
                result.Score.Id = -1;
                return Ok(result);
            }

            return Ok(result);




            //return Ok(context.GameByNameConvertedToEditModel(name));
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [Route("editgame")]
        public IActionResult EditGame(string nameOfGameToEdit, CreateGameModel gameToEdit)
        {
            if (gameToEdit.Year == 0)
            {
                ModelState.AddModelError("error", "Year can not be empty");
                return BadRequest(ModelState);
            }
            else if (ModelState.IsValid)
            {
                context.EditGame(nameOfGameToEdit, gameToEdit);
                return Ok($"{nameOfGameToEdit} has been edited.");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("addscore")]
        public IActionResult AddScore(string name, int score)
        {
            var gameToChangeScoreOn = context.GameByName(name);
            if (context.CheckIfUserHasAlreadyVoted(UserId, gameToChangeScoreOn.Id))
            {
                return BadRequest("User has already voted.");
            }
            else
            {
                var previousAmountOfVotes = gameToChangeScoreOn.Score.Votes;
                var previousScore = gameToChangeScoreOn.Score.Score;

                var currentTotalScore = previousAmountOfVotes * previousScore;
                var newScore = (currentTotalScore + score) / (previousAmountOfVotes + 1);
                gameToChangeScoreOn.Score.Score = newScore;
                gameToChangeScoreOn.Score.Votes++;
                context.ChangeScoring(gameToChangeScoreOn);
                context.AddRelationUserAndGame(UserId, gameToChangeScoreOn.Id);

                var scoreVM = new ScoreVM { Score = gameToChangeScoreOn.Score.Score, Votes = gameToChangeScoreOn.Score.Votes };

                return Ok(scoreVM);
            }
        }
    }
}
