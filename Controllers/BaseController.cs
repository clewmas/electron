using Microsoft.AspNetCore.Mvc;
using elec.Services;

namespace elec.Controllers;

public abstract class BaseController<T> : Controller
{
    protected readonly elec.Logging.ILogger<T> _logger;
     protected readonly IControllerExceptionHandler _exceptionHandler;

    public BaseController(
        elec.Logging.ILogger<T> logger,
        IControllerExceptionHandler exceptionHandler)
    {
        _logger = logger;
        _exceptionHandler = exceptionHandler;
    }

    protected IActionResult HandleException(Exception ex) {
        _logger.Error($"Exception occured: {ex}", ex);

        return _exceptionHandler.HandleException(ex);
    }
}