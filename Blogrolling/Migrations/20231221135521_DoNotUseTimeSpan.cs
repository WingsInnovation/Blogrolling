using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class DoNotUseTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdateFrequency",
                table: "DataSources",
                type: "longtext",
                nullable: true,
                comment: "更新频率",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "更新频率")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UpdateFrequency",
                table: "DataSources",
                type: "bigint",
                nullable: true,
                comment: "更新频率",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "更新频率")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
