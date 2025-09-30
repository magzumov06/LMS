using Domain.Dtos.Account;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var res = await accountService.Login(dto);
        if (res.StatusCode == 200 ) return Ok(res);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var res = await accountService.Register(dto);
        if (res.StatusCode == 200) return Ok(res);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Update([FromBody] ChangePasswordDto dto)
    {
        var res = await accountService.ChangePassword(dto);
        return Ok(res);
    }
}



