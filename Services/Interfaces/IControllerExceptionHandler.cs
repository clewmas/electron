using Microsoft.AspNetCore.Mvc;

namespace elec.Services;

public interface IControllerExceptionHandler
{
    IActionResult HandleException(Exception ex);
}