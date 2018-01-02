using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gamesdatabasetwo.Data;
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

        [HttpGet, Route("getall")]
        public IActionResult GetAll()
        {
            return Ok(userManager.Users);
        }

        [HttpPost, Route("signin")]
        public async Task<IActionResult> SignIn(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            await signInManager.SignInAsync(user, true);
            return Ok($"User with email {email} is signed in");
        }

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
            var user = await userManager.FindByEmailAsync(email);
            applicationDbContext.RemoveUser(user);
            return Ok($"User with email {email} has been removed");

        }

        [HttpGet, Route("add")]
        public async Task<IActionResult> Add(string email)
        {
            //if (String.IsNullOrEmpty(email))
            //{
            //    return BadRequest("Emailadress field can not be empty");
            //}
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                var user = new ApplicationUser
                {
                    Email = email
                };
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("Email is not valid");
                }
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
            string userId =  userManager.GetUserId(HttpContext.User);
            var user = await userManager.FindByIdAsync(userId);
            var token = await userManager.GenerateChangeEmailTokenAsync(user, email);
            var result = await userManager.ChangeEmailAsync(user, email, token);
            if (!result.Succeeded)
            {
                return BadRequest("That is not a valid email");
            }
            return Ok($"Updated email to {email}");
        }
    }
}
