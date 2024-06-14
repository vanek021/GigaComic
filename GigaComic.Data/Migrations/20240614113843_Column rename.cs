using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigaComic.Data.Migrations
{
    /// <inheritdoc />
    public partial class Columnrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Plot",
                table: "ComicAbstracts",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "ComicAbstracts",
                newName: "Plot");
        }
    }
}
