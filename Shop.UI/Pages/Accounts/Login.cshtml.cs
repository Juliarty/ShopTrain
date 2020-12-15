using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shop.UI.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private SignInManager<IdentityUser> _signInManager;

        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }


        [BindProperty]
        public LoginViewModel Input { get; set; }
        
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Admin/Index");
            }

            return RedirectToPage("/Accounts/Login");
        }

        public class LoginViewModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
