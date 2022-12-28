using Microsoft.AspNetCore.Mvc;

namespace elec.Services;

public class ControllerExceptionHandler : IControllerExceptionHandler
{
    public IActionResult HandleException(Exception ex)
    {
        if(ex is UnauthorizedAccessException)
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
            
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}