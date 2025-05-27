using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthContrller : ControllerBase
{
  private readonly UserManager<IdentityUser> _userManager;

  public AuthContrller(UserManager<IdentityUser> userManager)
  {
    _userManager = userManager;
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
          }
          
          return Ok("Login Successfully!");
      }
      
      return  BadRequest("Incorrect Username or password");
  }
}