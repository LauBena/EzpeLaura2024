using Microsoft.AspNetCore.Mvc;
using EzpeLaura2024.Models;
using EzpeLaura2024.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzpeLaura2024.Controllers;

[Authorize]

public class TipoEjerciciosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public TipoEjerciciosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}