using Domain.Dtos.Email;
using Infrastructure.Helpers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;
public class EmailService(IOptions<EmailSettings> options) : Interfaces.IEmailService
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(SendEmailDto dto)
    {
        await SendEmailHelper.SendEmail(dto, _settings);
    }
}



