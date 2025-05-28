using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly UserManager<IdentityUser> _userManager;
  private readonly ITokenRepository _tokenRepository;

  public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
  {
      _userManager = userManager;
      _tokenRepository = tokenRepository;
  }
  
  [HttpPost]
  [Route("Register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
  {
      var identityUser = new IdentityUser
      {
          UserName = registerRequestDto.Username,
          Email = registerRequestDto.Username
      };
      
      var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

      if (identityResult.Succeeded)
      {
          if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
          {
              identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

              if (identityResult.Succeeded)
              {
                  return Ok("User created Succesfully! you can Login now..");
              }
          }

      }
      
      return BadRequest("User creation failed");
  }

  [HttpPost]
  [Route("Login")]
  public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
  {
      var user= await _userManager.FindByNameAsync(loginRequestDto.Username);

      if (user != null)
      {
          var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

          if (checkPasswordResult)
          {
              //generate token and return
                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                    var response = new LoginResponseDto()
                    {
                        JwtToken = jwtToken,
                    };
                    
                    return Ok(response);
                }
          }
      }
      
      return  BadRequest("Incorrect Username or password");
  }
}