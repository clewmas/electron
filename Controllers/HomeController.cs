using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using elec.Models;
using elec.Services;

namespace elec.Controllers;

public class HomeController : BaseController<HomeController>
{

    public HomeController(
        elec.Logging.ILogger<HomeController> logger,
        IControllerExceptionHandler exceptionHandler) 
        : base(logger, exceptionHandler)
    {
    }

    public IActionResult Index()
    {
        try
        {
            return View();
        }
        catch(Exception ex)
        {
            return HandleException(ex);
        }
    }

    public IActionResult Privacy()
    {
        try
        {
            return View();
        }
        catch(Exception ex)
        {
            return HandleException(ex);
        }
    }
}
