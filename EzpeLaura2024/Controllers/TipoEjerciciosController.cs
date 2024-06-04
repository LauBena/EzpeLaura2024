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

    public JsonResult GuardarTipoEjercicio(int tipoEjercicioID, string descripcion)
    {
        //1- VERIFICAMOS SI INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        // if(descripcion != null && descripcion != "")
        // {
        //      INGRESA SI ESCRIBE SI O SI    
        //}

        //if(String.IsNullOrEmpty(descripcion)== false)
        //{
        // INGRESA SI ESCRIBE SI O SI
        //}

        string resultado = "";

        if (!String.IsNullOrEmpty(descripcion))
        {
            descripcion = descripcion.ToUpper();
            //INGRESA SI ESCRIBIO SI O SI 

            //2-VERIFICA SI EDITA O CREA NUEVO REGISTRO
            if (tipoEjercicioID == 0)
            {
                //3-VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //BUSCAMOS EN EL CONTEXTO (BASE DE DATOS)
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION
                var existeTipoEjercicio = _context.TipoEjercicios.Where(t => t.Descripcion == descripcion).Count();
                if (existeTipoEjercicio == 0)
                {
                    //4-GUARDA EL TIPO DE EJERCICIO
                    var tipoEjercicio = new TipoEjercicio
                    {
                        Descripcion = descripcion,
                    };
                    _context.Add(tipoEjercicio);
                    _context.SaveChanges();
                }
                else
                {
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCION";
                }
            }
            else
            {
                //VAMOS A EDITAR EL REGISTRO
                var tipoEjercicioEditar = _context.TipoEjercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).SingleOrDefault();
                if (tipoEjercicioEditar != null)
                {
                    //BUSCAMOS EN TABLA SI EXISTE UN REGISTRO CON MISMO NOMBRE, PERO DISTINTO ID AL QUE ESTAMOS EDITANDO
                    var existeTipoEjercicio = _context.TipoEjercicios.Where(t => t.Descripcion == descripcion && t.TipoEjercicioID != tipoEjercicioID).Count();
                    if (existeTipoEjercicio == 0)
                    {
                        //QUIERE DECIR QUE EXISTE Y ES CORRECTO, ENTONCES CONTINUAMOS EDITANDO
                        tipoEjercicioEditar.Descripcion = descripcion;
                        _context.SaveChanges();
                    }
                    else
                    {
                        resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCION";
                    }
                }
            }
        }
        else
        {
            resultado = "DEBE INGRESAR UNA DESCRIPCION";
        }
        return Json(resultado);
    }

    public JsonResult EliminarTipoEjercicio(int tipoEjercicioID)
    {
        bool eliminado = false;

        //BUSCO SI EXISTEN EJERCICIOS CARGADOS
        var existeEjercicio = _context.EjerciciosFisicos.Where(t => t.TipoEjercicioID == tipoEjercicioID).Count();
        if (existeEjercicio == 0)
        {
            var tipoEjercicio = _context.TipoEjercicios.Find(tipoEjercicioID);
            _context.Remove(tipoEjercicio);
            _context.SaveChanges();
            eliminado = true;
        }

        return Json(eliminado);
    }
}