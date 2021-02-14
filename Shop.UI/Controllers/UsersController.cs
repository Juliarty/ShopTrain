using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.UI.ViewModels.Admin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("[controller]")]
    public class UsersController: Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel vm)
        {

            var managerUser = new IdentityUser()
            {
                UserName = vm.Username,
            };

            var identityResult = await _userManager.CreateAsync(managerUser, vm.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            var claim = new Claim("Role", "Manager");
           
            identityResult = await _userManager.AddClaimAsync(managerUser, claim);

            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

            return Ok();
        }
              
    }
}
