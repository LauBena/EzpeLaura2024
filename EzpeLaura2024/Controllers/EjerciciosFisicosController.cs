using Microsoft.AspNetCore.Mvc;
using EzpeLaura2024.Models;
using EzpeLaura2024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EzpeLaura2024.Controllers;

[Authorize]
public class EjerciciosFisicosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EjerciciosFisicosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        //CREA LISTA DE SelectListItem QUE INCLUYE EL ELEMENTO ADICIONAL
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem {Value = "0", Text = "[SELECCIONE...]"}
        };

        //OBTENEMOS LAS OPCIONES DE ENUM
        var enumValues = Enum.GetValues(typeof(EstadoEmocional)).Cast<EstadoEmocional>();

        //CONVERTIMOS LAS OPCIONES ENUM EN SelectListItems
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        //PASAMOS LA LISTA DE OPCIONES AL MODELO DE LA VISTA
        ViewBag.EstadoEmocionalInicio = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFin = selectListItems.OrderBy(t => t.Text).ToList();

        var tipoEjercicios = _context.TipoEjercicios.ToList();
        var tiposEjerciciosBuscar = tipoEjercicios.ToList();

        tiposEjerciciosBuscar.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        tiposEjerciciosBuscar.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[TODOS LOS TIPOS DE EJERCICIOS]" });
        ViewBag.TipoEjercicioBuscarID = new SelectList(tiposEjerciciosBuscar.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }
}