using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class NullableBlogSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs");
            
            migrationBuilder.DropIndex(
                name: "IX_Blogs_SourceId",
                table: "Blogs");
            
            migrationBuilder.CreateIndex(
                name: "IX_Blogs_SourceId",
                table: "Blogs",
                column: "SourceId",
                unique: false);
            
            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs",
                column: "SourceId",
                principalTable: "DataSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs");
            
            migrationBuilder.DropIndex(
                name: "IX_Blogs_SourceId",
                table: "Blogs");
            
            migrationBuilder.CreateIndex(
                name: "IX_Blogs_SourceId",
                table: "Blogs",
                column: "SourceId",
                unique: true);
            
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
