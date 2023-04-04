using Microsoft.AspNetCore.Identity;
using NoteTakingApp.Models;
using NoteTakingApp.Models.DTO;
using NoteTakingApp.Repository.interfaces;
using System.Security.Claims;

namespace NoteTakingApp.Repository
{
    public class AuthRepository : IAuthRepository
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public AuthRepository(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, 
                            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public AjaxResponse Login(LoginData loginData)
        {
            var response = new AjaxResponse();

            var user = _userManager.FindByEmailAsync(loginData.Email).GetAwaiter().GetResult();
            if(user == null)
            {
                response.IsSuccess = false;
                response.StatusCode = 400;
                response.ResponseMessage = "Invalid User";
                return response;
            }

            if(!_userManager.CheckPasswordAsync(user, loginData.Password).GetAwaiter().GetResult())
            {
                response.IsSuccess = false;
                response.StatusCode = 400;
                response.ResponseMessage = "Invalid User";
                return response;
            }

            var signInResult = _signInManager.PasswordSignInAsync(user, loginData.Password, false, true).GetAwaiter().GetResult();
            if (signInResult.Succeeded)
            {
                var userRoles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                response.IsSuccess = true;
                response.StatusCode = 200;
                response.ResponseMessage = "Login success";
                return response;
            }else
            {
                response.IsSuccess = false;
                response.StatusCode = 400;
                response.ResponseMessage = "Login failed";
                return response;
            }


        }

        public void Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
        }

        public AjaxResponse Registration(RegistartionData registerData)
        {
            var response = new AjaxResponse();
            var userExist = _userManager.FindByEmailAsync(registerData.Email).GetAwaiter().GetResult();
            
            if(userExist != null)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "User already exist";
                response.StatusCode = 400;

                return response;
            }


            ApplicationUser user = new ApplicationUser
            {
                Email = registerData.Email,
                Name = registerData.Name,
                UserName = registerData.UserName,
                EmailConfirmed = true
            };


            var result  = _userManager.CreateAsync(user, registerData.Password)
                        .GetAwaiter().GetResult();


            if (!result.Succeeded)
            {
                response.IsSuccess = false;
                response.ResponseMessage = "Registeration failed";
                response.StatusCode = 400;
                return response;
            }

            if(!_roleManager.RoleExistsAsync("User").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
            }

            _userManager.AddToRoleAsync(user, "User");

            response.IsSuccess = true;
            response.StatusCode = 201;
            response.ResponseMessage = "Registration success";
            return response;
        }
    }
}
