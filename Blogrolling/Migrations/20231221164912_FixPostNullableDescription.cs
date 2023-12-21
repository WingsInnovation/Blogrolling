using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class FixPostNullableDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "longtext",
                nullable: true,
                comment: "文章介绍",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "文章介绍")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "Blogs",
                type: "int",
                nullable: true,
                comment: "数据源ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "数据源ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs",
                column: "SourceId",
                principalTable: "DataSources",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Description",
                keyValue: null,
                column: "Description",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "longtext",
                nullable: false,
                comment: "文章介绍",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "文章介绍")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "数据源ID",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "数据源ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs",
                column: "SourceId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
