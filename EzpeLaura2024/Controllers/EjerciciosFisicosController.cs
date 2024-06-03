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

    public JsonResult GetEjerciciosFisicos(int? id, DateTime? FechaDesdeBuscar, DateTime? FechaHastaBuscar, int? TipoEjercicioBuscarID)
    {
        var ejerciciosFisicos = _context.EjerciciosFisicos.Include(t => t.TipoEjercicio).ToList();
        if (id != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.EjercicioFisicoID == id).ToList();
        }

        if (FechaDesdeBuscar != null && FechaHastaBuscar != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.Inicio >= FechaDesdeBuscar && t.Inicio <= FechaHastaBuscar).ToList();
        }

        if (TipoEjercicioBuscarID != null && TipoEjercicioBuscarID != 0)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.TipoEjercicioID == TipoEjercicioBuscarID).ToList();
        }

        var ejercicioFisicosMostrar = ejerciciosFisicos.Select(e => new VistaEjercicioFisico
        {
            EjercicioFisicoID = e.EjercicioFisicoID,
            TipoEjercicioID = e.TipoEjercicioID,
            TipoEjercicioNombre = e.TipoEjercicio.Descripcion,
            Inicio = e.Inicio,
            InicioString = e.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Fin = e.Fin,
            FinString = e.Fin.ToString("dd/MM/yyyy HH:mm"),
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinString = e.EstadoEmocionalFin.ToString(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioString = e.EstadoEmocionalInicio.ToString(),
            Observaciones = e.Observaciones
        })
        .ToList();

        return Json(ejercicioFisicosMostrar);
    }

    public JsonResult GuardarEjercicio(int ejercicioFisicoID, int tipoEjercicioID, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, DateTime fechaInicio, DateTime fechaFin, string? observaciones)
    {
        int error = 0;

        //VALIDAMOS QUE SELECCIONE TIPO DE EJERCICIO
        if (error == 0)
        {
            error = 1;
        }

        //VALIDAMOS QUE FECHA DE INICIO NO SEA MAYOR A LA DE FIN
        if (error == 0)
        {
            if (ejercicioFisicoID == 0)
            {
                //GUARDAMOS EL EJERCICIO
                var ejercicio = new EjercicioFisico
                {
                    TipoEjercicioID = tipoEjercicioID,
                    EstadoEmocionalInicio = estadoEmocionalInicio,
                    EstadoEmocionalFin = estadoEmocionalFin,
                    Inicio = fechaInicio,
                    Fin = fechaFin,
                    Observaciones = observaciones
                };
                _context.Add(ejercicio);
                _context.SaveChanges();
            }
            else
            {
                //VAMOS A EDITAR EL REGISTRO
                var ejercicioEditar = _context.EjerciciosFisicos.Where(t => t.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();
                if (ejercicioEditar != null)
                {
                    ejercicioEditar.TipoEjercicioID = tipoEjercicioID;
                    ejercicioEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                    ejercicioEditar.EstadoEmocionalFin = estadoEmocionalFin;
                    ejercicioEditar.Inicio = fechaInicio;
                    ejercicioEditar.Fin = fechaFin;
                    ejercicioEditar.Observaciones = observaciones;
                    _context.SaveChanges();
                }
            }
        }

        return Json(error);
    }

    public JsonResult EliminarEjercicio(int ejercicioFisicoID)
    {
        var ejercicio = _context.EjerciciosFisicos.Find(ejercicioFisicoID);
        _context.Remove(ejercicio);
        _context.SaveChanges();

        return Json(true);
    }
}