﻿using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Dtos.Account;

public class RegisterDto
{
    [Required]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "FullName must be between 4 and 50 characters")]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required]
    [StringLength(50,MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public DateTime Birthday { get; set; }
    public string PhoneNumber { get; set; } = string.Empty; 
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
}