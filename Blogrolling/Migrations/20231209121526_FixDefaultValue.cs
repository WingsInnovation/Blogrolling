using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class FixDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Blog",
                type: "longtext",
                nullable: false,
                comment: "博客状态",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "博客状态")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "FeedPrevUpdate",
                table: "Blog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "上次Feed更新时间",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "上次Feed更新时间");

            migrationBuilder.AlterColumn<long>(
                name: "FeedNextUpdate",
                table: "Blog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "下次Feed更新时间",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "下次Feed更新时间");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Blog",
                type: "longtext",
                nullable: false,
                comment: "博客状态",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValue: "Ok",
                oldComment: "博客状态")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "FeedPrevUpdate",
                table: "Blog",
                type: "bigint",
                nullable: false,
                comment: "上次Feed更新时间",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 0L,
                oldComment: "上次Feed更新时间");

            migrationBuilder.AlterColumn<long>(
                name: "FeedNextUpdate",
                table: "Blog",
                type: "bigint",
                nullable: false,
                comment: "下次Feed更新时间",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 0L,
                oldComment: "下次Feed更新时间");
        }
    }
}
