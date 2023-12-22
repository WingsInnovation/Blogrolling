using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class FixDataSourceLinkUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "DataSources",
                type: "varchar(255)",
                nullable: false,
                comment: "链接",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "链接")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_Link",
                table: "DataSources",
                column: "Link",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DataSources_Link",
                table: "DataSources");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "DataSources",
                type: "longtext",
                nullable: false,
                comment: "链接",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "链接")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
