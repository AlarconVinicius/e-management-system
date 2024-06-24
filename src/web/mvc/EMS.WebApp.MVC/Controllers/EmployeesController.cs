using EMS.Core.Notifications;
using EMS.Core.Requests.Employees;
using EMS.Core.Requests.Identities;
using EMS.Core.Responses.Employees;
using EMS.Core.User;
using EMS.WebApp.MVC.Handlers;
using EMS.WebApp.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.WebApp.MVC.Controllers;

[Authorize]
public class EmployeesController : MainController
{
    private readonly IEmployeeHandler _employeeHandler;
    private readonly IIdentityHandler _identityHandler;
    private readonly IAspNetUser _appUser;

    public EmployeesController(INotifier notifier, IAspNetUser appUser, IEmployeeHandler employeeHandler) : base(notifier)
    {
        _appUser = appUser;
        _employeeHandler = employeeHandler;
    }

    public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string q = null)
    {
        var ps = 10;
        var request = new GetAllEmployeesRequest { PageNumber = page, PageSize = ps, Query = q };
        var response = await _employeeHandler.GetAllAsync(request);

        var mappedEmployees = new PagedViewModel<EmployeeResponse>
        {
            List = response.Data.List,
            PageIndex = response.Data.PageIndex,
            PageSize = response.Data.PageSize,
            Query = request.Query,
            TotalResults = response.Data.TotalResults
        };
        ViewBag.Search = q;
        mappedEmployees.ReferenceAction = "Index";

        return View(mappedEmployees);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        return View(response);
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateEmployeeAndUserRequest request, string returnUrl = null)
    {
        var password = GeneratePassword(request.Employee);
        var newUser = new CreateUserRequest(request.Employee.Id, request.Employee.Email, password, password);
        request.User = newUser;
        ModelState.Remove("User");
        ModelState.Remove("User.Email");
        ModelState.Remove("User.Password");
        ModelState.Remove("User.ConfirmPassword");
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _employeeHandler.CreateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Colaborador adicionado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        var employeeRequest = new UpdateEmployeeRequest(id, response.CompanyId, response.Name, response.LastName, response.Email, response.PhoneNumber, response.Salary, response.Role, response.IsActive);
        var userRequest = new UpdateUserEmailRequest(id, response.Email);
        var request = new UpdateEmployeeAndUserRequest(userRequest, employeeRequest);
        ViewBag.Document = response.Document;
        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateEmployeeAndUserRequest request)
    {
        //if (id != request.Employee.Id)
        //{
        //    return NotFound();
        //}
        request.Employee.Id = id;
        var newUser = new UpdateUserEmailRequest(id, request.Employee.Email);
        request.User = newUser;
        ModelState.Remove("User");
        ModelState.Remove("User.Id");
        ModelState.Remove("User.NewEmail");
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _employeeHandler.UpdateAsync(request);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Colaborador atualizado com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProfile()
    {
        var id = _appUser.GetUserIdByJwt();
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        var employeeRequest = new UpdateEmployeeRequest(id, response.CompanyId, response.Name, response.LastName, response.Email, response.PhoneNumber, response.Salary, response.Role, response.IsActive);
        var userRequest = new UpdateUserEmailRequest(id, response.Email);
        var employeeUserrequest = new UpdateEmployeeAndUserRequest(userRequest, employeeRequest);
        var passwordRequest = new UpdateUserPasswordRequest();
        var request = new EmployeeAndUserViewModel(employeeUserrequest, passwordRequest);
        ViewBag.Document = response.Document;
        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(EmployeeAndUserViewModel request, string returnUrl = null)
    {
        var id = request.UpdateEmployeeAndUserRequest.Employee.Id;
        //request.UpdateEmployeeAndUserRequest.Employee.Id = id;
        var newUser = new UpdateUserEmailRequest(id, request.UpdateEmployeeAndUserRequest.Employee.Email);
        request.UpdateEmployeeAndUserRequest.User = newUser;
        ModelState.Remove("UpdateEmployeeAndUserRequest.User");
        ModelState.Remove("UpdateEmployeeAndUserRequest.User.Id");
        ModelState.Remove("UpdateEmployeeAndUserRequest.User.NewEmail");
        ModelState.Remove("UpdateUserPasswordRequest.OldPassword");
        ModelState.Remove("UpdateUserPasswordRequest.NewPassword");
        ModelState.Remove("UpdateUserPasswordRequest.ConfirmNewPassword");
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _employeeHandler.UpdateAsync(request.UpdateEmployeeAndUserRequest);

        if (HasErrorsInResponse(result)) return View(request);

        TempData["Success"] = "Perfil atualizado com sucesso!";
        return RedirectToAction(nameof(UpdateProfile));
        //ModelState.Remove("UpdateUserPasswordViewModel");
        //if (!ModelState.IsValid)
        //{
        //    return View(updateUserVM);
        //}
        //var id = _appUser.GetUserId();
        //var userDb = await _employeeHandler.GetByIdAsync(id);

        //if (userDb is null)
        //{
        //    return NotFound();
        //}
        ////var updateUserResult = await _employeeHandler.Update(new Employee());
        ////if (!updateUserResult.IsValid)
        ////{
        ////    AddError(updateUserResult);
        ////    TempData["Failure"] = "Falha ao atualizar usuário: " + string.Join("; ", GetModelStateErrors());
        ////    return View(updateUserVM);
        ////}
        //TempData["Success"] = "Usuário atualizado com sucesso!";
        //return RedirectToAction(nameof(UpdateProfile));
    }

    ////[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await GetById(id);
        if (response is null)
        {
            return NotFound();
        }
        return View(response);
    }

    ////[Authorize(Roles = "Admin")]
    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> DeleteConfirmed(Guid id)
    //{
    //    var response = await GetById(id);
    //    var identityUser = await _authService.GetUserById(id.ToString());
    //    if (response is null)
    //    {
    //        return NotFound();
    //    }
    //    if (identityUser is null)
    //    {
    //        return NotFound();
    //    }
    //    await _authService.DeleteUser(id.ToString());
    //    await _employeeHandler.DeleteAsync(new DeleteEmployeeRequest { Id = id });

    //    if (!IsValidOperation())
    //    {
    //        TempData["Failure"] = "Falha ao deletar colaborador: " + string.Join("; ", await GetNotificationErrors());
    //        return View(response);
    //    }

    //    TempData["Success"] = "Colaborador deletado com sucesso!";
    //    return RedirectToAction(nameof(Index));
    //}

    //private async Task<bool> AddIdentityUser(CreateEmployeeRequest request)
    //{
    //    var identityUser = new RegisterUser
    //    {
    //        Id = request.Id,
    //        Role = request.Role.ToString(),
    //        Email = request.Email,
    //        Password = GeneratePassword(request)
    //    };
    //    await _authService.RegisterUser(identityUser);
    //    if (!IsValidOperation())
    //    {
    //        return false;
    //    }
    //    return true;
    //}
    //private Guid GetTenant()
    //{
    //    return _appUser.GetTenantId();
    //}
    private async Task<EmployeeResponse> GetById(Guid id)
    {
        return (await _employeeHandler.GetByIdAsync(new GetEmployeeByIdRequest { Id = id })).Data;
    }

    private static string GeneratePassword(CreateEmployeeRequest request)
    {
        return $"{request.Document[..5]}@{request.Name[..1].ToUpper()}{request.LastName[..1].ToLower()}";
    }

}
