using Domain.Dtos.Email;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendEmailController(IEmailService service): Controller
{
    [HttpPost]
    [Authorize(Roles = "Teacher,SuperAdmin")]
    public async Task<IActionResult> SendEmail(SendEmailDto dto)
    {
       await service.SendAsync(dto);
       return Ok();
    }
}