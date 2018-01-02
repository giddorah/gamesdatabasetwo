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
    [Route("api/Users")]
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
            return Ok(user.Email);
        }

        [HttpGet, Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();

            return Ok("Logged out");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, Route("remove")]
        public async Task<IActionResult> Remove(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            return Ok($"User with email: {email} removed");
        }
    }
}
