using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gamesdatabasetwo.Data;
using gamesdatabasetwo.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gamesdatabasetwo.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext applicationDbContext;


        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;

        }

        [HttpGet, Route("admintest")]
        [Authorize(Roles = "Admin")]
        public IActionResult IsAdmin()
        {
            return Ok("You are admin");
        }

        [HttpGet, Route("returnrole")]
        public async Task<IActionResult> ReturnRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = userManager.GetUserId(HttpContext.User);
                var loggedInUser = await userManager.FindByIdAsync(userId);
                var result = await userManager.GetRolesAsync(loggedInUser);

                return Ok(result);
            }
            else
            {
                return Ok("Anonymous");
            }

        }



        [HttpGet, Route("TempAdminAdd")]
        public async Task<IActionResult> AddAdmin()
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            var user = new ApplicationUser()
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com"
            };
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest("Did not add user");

            var roleResult = await userManager.AddToRoleAsync(user, "Admin");
            if (!roleResult.Succeeded) return BadRequest("Did not add to role");
            return Ok($"Admin with email {user.Email} created");
        }

        [HttpGet, Route("sortbyemail")]
        public IActionResult SortByEmail()
        {
            var sortManager = new SortManager(applicationDbContext);
            var users = applicationDbContext.AllUsers();
            var sortedList = sortManager.AlphabeticallySortedUsers(users);


            return Ok(sortedList);
        }

        [HttpGet, Route("getall")]
        public IActionResult GetAll()
        {
            return Ok(applicationDbContext.AllUsers());
        }

        [AllowAnonymous]
        [HttpPost, Route("signin")]
        public async Task<IActionResult> SignIn(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);
                await signInManager.SignInAsync(user, true);
            }
            catch (Exception)
            {

                return BadRequest($"User with email {email} does not exist");
            }
            
            
            
            return Ok($"User with email {email} is signed in");
        }

        [Authorize]
        [HttpPost, Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return Ok("Signed out");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, Route("remove")]
        public async Task<IActionResult> Remove(string email)
        {
            try
            {
                var userToRemove = await userManager.FindByEmailAsync(email);
                var result = await userManager.DeleteAsync(userToRemove);
                if (!result.Succeeded) return BadRequest($"User with email {email} does not exist");
                return Ok($"User with email {email} has been removed");
            }
            catch (Exception)
            {

                return BadRequest($"User with email {email} does not exist");
            }
            

        }

        [AllowAnonymous]
        [HttpPost, Route("add")]
        public async Task<IActionResult> Add(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return BadRequest("Emailadress field can not be empty");
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Staff"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                var user = new ApplicationUser
                {
                    Email = email,
                    UserName = email

                };
                if (User.Identity.IsAuthenticated)
                {
                    string userId = userManager.GetUserId(HttpContext.User);
                    var loggedInUser = await userManager.FindByIdAsync(userId);
                    if (await userManager.IsInRoleAsync(loggedInUser, "Admin"))
                    {
                        var userResult = await userManager.CreateAsync(user);
                        if (!userResult.Succeeded) return BadRequest("Email is not valid");

                        var staffResult = await userManager.AddToRoleAsync(user, "Staff");
                        if (!staffResult.Succeeded) return BadRequest("Role does not exist");

                        return Ok($"User {email} added");
                    }
                }


                var normalUserResult = await userManager.CreateAsync(user);
                if (!normalUserResult.Succeeded) return BadRequest("Email is not valid");

                var normalRoleResult = await userManager.AddToRoleAsync(user, "User");
                if (!normalRoleResult.Succeeded) return BadRequest("Role does not exist");

                return Ok($"User {email} added");

            }
            catch
            {
                return BadRequest($"{email} is not a valid emailadress");
            }
        }

        [HttpPost, Route("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return BadRequest("Emailadress field can not be empty");
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                string userId = userManager.GetUserId(HttpContext.User);
                var user = await userManager.FindByIdAsync(userId);
                var token = await userManager.GenerateChangeEmailTokenAsync(user, email);
                var result = await userManager.ChangeEmailAsync(user, email, token);
                if (!result.Succeeded)
                {
                    return BadRequest("That is not a valid email");
                }

                var change = await userManager.SetUserNameAsync(user, email);
                if (!change.Succeeded)
                {
                    return BadRequest("Not a valid username");
                }
                return Ok($"Updated email to {email}");
            }
            catch (Exception)
            {

                return BadRequest($"{email} is not a valid email");
            }
            
        }
    }
}
