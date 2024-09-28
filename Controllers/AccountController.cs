using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByNameAsync(loginDto.Username);
                if (user == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                // var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false); // false means we don't want to lock the user out
                if (!passwordValid.Succeeded)
                {
                    return Unauthorized("Invalid username or password");
                }

                return Ok(new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                // Do we need this? ModelState.IsValid should take care of this.
                // if (string.IsNullOrEmpty(registerDto.Password))
                // {
                //     return BadRequest("Password cannot be null or empty.");
                // }

                var userCreated = await _userManager.CreateAsync(user, registerDto.Password);
                if (userCreated.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        // return Ok("User created successfully");
                        return Ok(new NewUserDto
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user)
                        });
                    }
                    else
                    {
                        return BadRequest(roleResult.Errors);
                    }
                }
                else
                {
                    return BadRequest(userCreated.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}