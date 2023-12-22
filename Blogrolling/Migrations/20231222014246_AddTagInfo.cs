using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class AddTagInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                table: "Tags");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "longtext",
                nullable: false,
                comment: "标签名",
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "博客Id");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "Tags",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                comment: "标签Guid")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Tags",
                type: "longtext",
                nullable: true,
                comment: "标签链接")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PublishTime",
                table: "Posts",
                type: "longtext",
                nullable: false,
                comment: "发布时间")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UpdateTime",
                table: "Posts",
                type: "longtext",
                nullable: true,
                comment: "更新时间")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_BlogId",
                table: "Tags",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Guid_BlogId",
                table: "Tags",
                columns: new[] { "Guid", "BlogId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Blogs_BlogId",
                table: "Tags",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Blogs_BlogId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_BlogId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Guid_BlogId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "PublishTime",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "标签名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }
    }
}
