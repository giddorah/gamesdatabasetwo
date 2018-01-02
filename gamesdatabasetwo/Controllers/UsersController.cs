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
            return Ok($"User with email {email} is signed in");
        }

        [HttpGet,Route("signout")]
        public IActionResult SignOut()
        {
            signInManager.SignOutAsync();
            return Ok("Signed out");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, Route("Remove")]
        public async Task<IActionResult> Remove(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            applicationDbContext.RemoveUser(user);
            return Ok($"User with email {email} has been removed");
            
        }
    }
}
