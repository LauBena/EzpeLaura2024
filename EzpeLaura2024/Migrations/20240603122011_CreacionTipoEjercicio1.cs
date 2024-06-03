using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzpeLaura2024.Migrations
{
    /// <inheritdoc />
    public partial class CreacionTipoEjercicio1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoEjercicios",
                columns: table => new
                {
                    TipoEjercicioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eliminado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEjercicios", x => x.TipoEjercicioID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoEjercicios");
        }
    }
}
