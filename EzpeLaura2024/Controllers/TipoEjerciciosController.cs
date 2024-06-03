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

    public JsonResult ListadoTipoEjercicios(int? id)
    {
        //DEFINIMOS LA VARIABLE DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPO DE EJERCICIOS
        var tipoDeEjercicios = _context.TipoEjercicios.ToList();

        //PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //OSEA QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO POR EL EJERCICIO QUE COINCIDA CON ESE ID
            tipoDeEjercicios = tipoDeEjercicios.Where(t => t.TipoEjercicioID == id).ToList();
        }

        return Json(tipoDeEjercicios);
    }


}