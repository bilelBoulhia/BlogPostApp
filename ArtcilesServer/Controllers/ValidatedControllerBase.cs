using Microsoft.AspNetCore.Mvc;

public abstract class ValidatedControllerBase : ControllerBase
{
    protected string? UserIdFromToken => HttpContext?.Items["userId"]?.ToString();

    protected IActionResult ValidateUser(int UserId)
    {
        int tokenUserId = int.Parse(UserIdFromToken);
        if (UserId != tokenUserId)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "no no you are not allowed to do that");
        }
        return null;
    }
}
