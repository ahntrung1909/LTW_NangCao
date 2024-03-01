using BTL.Models;
using BTL.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;
		private SignInManager<AppUserModel> _signInManage;
        public AccountController(SignInManager<AppUserModel> signInManager,UserManager<AppUserModel> userManage) {
            _signInManage = signInManager;
            _userManage = userManage;
		}

		public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManage.PasswordSignInAsync(loginVM.UserName,loginVM.Password,false,false);
                if (result.Succeeded)
                {
                    return Redirect(loginVM.ReturnUrl ?? "/");  
                }
                ModelState.AddModelError("", "Invalid UserName or Password");
            }
            return View(loginVM);
        }
		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
		public async Task<IActionResult> Create(UserModel user)
		{
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email};
                IdentityResult result = await _userManage.CreateAsync(newUser,user.Password);
                if(result.Succeeded)
                {
                    TempData["success"] = "Tạo user thành công";
                    return Redirect("/account/login");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
			return View(user);
		}
	}
}
