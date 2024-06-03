using EzpeLaura2024.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EzpeLaura2024.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TipoEjercicio> TipoEjercicios { get; set; }
    public DbSet<EjercicioFisico> EjerciciosFisicos { get; set; }
}
