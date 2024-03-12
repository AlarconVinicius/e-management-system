using EMS.Core.Data;
using EMS.Users.API.Application.DTO;
using EMS.Users.API.Models;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using EMS.WebAPI.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Users.API.Controllers;

[Authorize]
[Route("users")]
public class UsersController : MainController
{
    private readonly IUserRepository _userRepository;
    private readonly IAspNetUser _user;

    public UsersController(IUserRepository userRepository, IAspNetUser user, INotifier notifier) : base(notifier)
    {
        _userRepository = userRepository;
        _user = user;
    }

    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> AddUser(UserAddDto user)
    {
        if (!user.IsValid()) return CustomResponse(user.GetValidationResult());

        var userDb = await _userRepository.GetById(user.Id);
        if (userDb != null)
        {
            return CustomResponse("There cannot be 2 records with the same Id.");
        }
        var userMap = new User(user.Id, user.Name, user.Email, user.Cpf);
        _userRepository.Add(userMap);
        await PersistData(_userRepository.UnitOfWork);
        return CustomResponse();
    }

    [HttpGet("address")]
    public async Task<IActionResult> GetAddress()
    {
        var address = await _userRepository.GetAddressById(_user.GetUserId());
        var addressMap = new AddressViewDto(address.UserId, address.Street, address.Number, address.Complement, address.Neighborhood, address.ZipCode, address.City, address.State);
        return address == null ? NotFound() : CustomResponse(addressMap);
    }

    [HttpPost("address")]
    public async Task<IActionResult> AddAddress(AddressAddDto address)
    {
        address.SetUserId(_user.GetUserId());
        if (!address.IsValid()) return CustomResponse(address.GetValidationResult());

        var addressMap = new Address(address.Street, address.Number, address.Complement, address.Neighborhood, address.ZipCode, address.City, address.State, address.UserId);
        _userRepository.AddAddress(addressMap);

        await PersistData(_userRepository.UnitOfWork);
        return CustomResponse();
    }

    private async Task<bool> PersistData(IUnitOfWork uow)
    {
        if (!await uow.Commit())
        {
            NotifyError("There was an error persisting the data");
            return false;
        }
        return true;
    }
}
