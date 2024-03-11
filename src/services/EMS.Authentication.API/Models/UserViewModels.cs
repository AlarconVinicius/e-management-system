using System.ComponentModel.DataAnnotations;

namespace EMS.Authentication.API.Models;

public class UserRegistration
{
    [Required(ErrorMessage = "The field {0} is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    public string Cpf { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress(ErrorMessage = "The field {0} is not in a valid format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(100, ErrorMessage = "The field {0} must be between {2} and {1} characters long", MinimumLength = 6)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public string PasswordConfirmation { get; set; }
}

public class UserLogin
{
    [Required(ErrorMessage = "The field {0} is required")]
    [EmailAddress(ErrorMessage = "The field {0} is not in a valid format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(100, ErrorMessage = "The field {0} must be between {2} and {1} characters long", MinimumLength = 6)]
    public string Password { get; set; }
}

public class UserLoginResponse
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken UserToken { get; set; }
}

public class UserToken
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserClaim> Claims { get; set; }
}

public class UserClaim
{
    public string Value { get; set; }
    public string Type { get; set; }
}
