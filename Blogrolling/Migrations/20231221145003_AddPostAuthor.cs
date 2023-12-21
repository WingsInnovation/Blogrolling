using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class AddPostAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Posts",
                type: "longtext",
                nullable: true,
                comment: "文章作者")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Blogs",
                type: "varchar(255)",
                nullable: false,
                comment: "博客标志",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "博客标志")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Guid",
                table: "Blogs",
                column: "Guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_Guid",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Guid",
                table: "Blogs",
                type: "longtext",
                nullable: false,
                comment: "博客标志",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "博客标志")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
