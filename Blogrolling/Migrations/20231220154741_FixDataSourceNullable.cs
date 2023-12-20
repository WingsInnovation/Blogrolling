using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class FixDataSourceNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextUpdateTime",
                table: "DataSources");

            migrationBuilder.RenameColumn(
                name: "PrevUpdateTime",
                table: "DataSources",
                newName: "LastUpdateTime");

            migrationBuilder.AddColumn<string>(
                name: "NextFetchTime",
                table: "DataSources",
                type: "longtext",
                nullable: true,
                comment: "下次获取时间")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PrevFetchTime",
                table: "DataSources",
                type: "longtext",
                nullable: true,
                comment: "上次获取时间")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextFetchTime",
                table: "DataSources");

            migrationBuilder.DropColumn(
                name: "PrevFetchTime",
                table: "DataSources");

            migrationBuilder.RenameColumn(
                name: "LastUpdateTime",
                table: "DataSources",
                newName: "PrevUpdateTime");

            migrationBuilder.AddColumn<string>(
                name: "NextUpdateTime",
                table: "DataSources",
                type: "longtext",
                nullable: true,
                comment: "下次Feed更新时间")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
