using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class AllowTagWithSameGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Guid_BlogId",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Tags",
                type: "varchar(255)",
                nullable: true,
                comment: "标签链接",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "标签链接")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Guid_BlogId_Link",
                table: "Tags",
                columns: new[] { "Guid", "BlogId", "Link" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Guid_BlogId_Link",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "Tags",
                type: "longtext",
                nullable: true,
                comment: "标签链接",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true,
                oldComment: "标签链接")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Guid_BlogId",
                table: "Tags",
                columns: new[] { "Guid", "BlogId" },
                unique: true);
        }
    }
}
