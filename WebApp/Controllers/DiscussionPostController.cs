using Domain.Dtos.DiscussionPostDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class DiscussionPostController(IDiscussionPostService service ) : Controller
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(CreateDiscussionDto dto)
    {
        var res = await service.Create(dto);
        return Ok(res);
    }

    [HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Update(UpdateDiscussionDto dto)
    {
        var res = await service.Update(dto);
        return Ok(res);
    }

    [HttpDelete]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.Delete(id);
        return Ok(res);
    }

    [HttpGet("{Id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetById(id);
        return Ok(res);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var res = await service.Get();
        return Ok(res);
    }
}