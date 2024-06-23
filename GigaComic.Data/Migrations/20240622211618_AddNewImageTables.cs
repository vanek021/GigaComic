using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GigaComic.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewImageTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComicCompositeImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageGrid = table.Column<int>(type: "integer", nullable: false),
                    ComicId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PublicUrl = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicCompositeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicCompositeImages_Comics_ComicId",
                        column: x => x.ComicId,
                        principalTable: "Comics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ComicRawImages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeneratingRequest = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsCensored = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    ComicId = table.Column<long>(type: "bigint", nullable: false),
                    ComicCompositeImageId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PublicUrl = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicRawImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicRawImages_ComicCompositeImages_ComicCompositeImageId",
                        column: x => x.ComicCompositeImageId,
                        principalTable: "ComicCompositeImages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ComicRawImages_Comics_ComicId",
                        column: x => x.ComicId,
                        principalTable: "Comics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComicCompositeImages_ComicId",
                table: "ComicCompositeImages",
                column: "ComicId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicCompositeImages_Id",
                table: "ComicCompositeImages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComicRawImages_ComicCompositeImageId",
                table: "ComicRawImages",
                column: "ComicCompositeImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicRawImages_ComicId",
                table: "ComicRawImages",
                column: "ComicId");

            migrationBuilder.CreateIndex(
                name: "IX_ComicRawImages_Id",
                table: "ComicRawImages",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComicRawImages");

            migrationBuilder.DropTable(
                name: "ComicCompositeImages");
        }
    }
}
