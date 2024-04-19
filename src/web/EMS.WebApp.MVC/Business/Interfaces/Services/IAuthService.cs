﻿using EMS.WebApp.MVC.Business.Models.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace EMS.WebApp.MVC.Business.Interfaces.Services;

public interface IAuthService
{
    Task<IdentityUser> GetUserById(string userId);
    Task<ValidationResult> Login(LoginUser loginUser);
    Task<ValidationResult> RegisterUser(RegisterUser registerUser);
    Task<ValidationResult> DeleteUser(string userId);
    Task<ValidationResult> AddOrUpdateUserClaim(string userId, string type, string value);
}
