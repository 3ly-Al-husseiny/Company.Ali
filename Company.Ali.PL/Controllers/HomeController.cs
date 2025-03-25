using System.Diagnostics;
using System.Text;
using Company.Ali.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Ali.PL.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IScopedService _scopedService01;
    private readonly IScopedService _scopedService02;
    private readonly ITransientService _transientService01;
    private readonly ITransientService _transientService02;
    private readonly ISingltonServices _singltonServices01;
    private readonly ISingltonServices _singltonServices02;


    public HomeController(
        ILogger<HomeController> logger,
        IScopedService scopedService01,
        IScopedService scopedService02,
        ITransientService transientService01,
        ITransientService transientService02,
        ISingltonServices singltonServices01,
        ISingltonServices singltonServices02
        )
    {
        _logger = logger;
        _scopedService01 = scopedService01;
        _scopedService02 = scopedService02;
        _transientService01 = transientService01;
        _transientService02 = transientService02;
        _singltonServices01 = singltonServices01;
        _singltonServices02 = singltonServices02;
    }


    public IActionResult TestLifeTime() 
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"scopedService01 :: {_scopedService01.GetGuid()}\n");
        builder.Append($"scopedService02 :: {_scopedService02.GetGuid()}\n");
        builder.Append($"transientService01 :: {_transientService01.GetGuid()}\n");
        builder.Append($"transientService02 :: {_transientService02.GetGuid()}\n");
        builder.Append($"singltonServices01 :: {_singltonServices01.GetGuid()}\n");
        builder.Append($"singltonServices02 :: {_singltonServices02.GetGuid()}\n");

        return Content(builder.ToString());
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
}
