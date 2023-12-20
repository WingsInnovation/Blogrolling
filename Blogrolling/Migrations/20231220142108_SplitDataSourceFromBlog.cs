using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class SplitDataSourceFromBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Blog_BlogId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Post_PostId",
                table: "PostTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tag_TagId",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blog",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Feed",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "FeedNextUpdate",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "FeedPrevUpdate",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Blog");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Blog",
                newName: "Blogs");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_Name",
                table: "Tags",
                newName: "IX_Tags_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Post_Guid",
                table: "Posts",
                newName: "IX_Posts_Guid");

            migrationBuilder.RenameIndex(
                name: "IX_Post_BlogId",
                table: "Posts",
                newName: "IX_Posts_BlogId");

            migrationBuilder.AlterTable(
                name: "Blogs",
                comment: "博客",
                oldComment: "博客数据源")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Posts",
                type: "longtext",
                nullable: false,
                comment: "链接")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "数据源ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "源ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(type: "longtext", nullable: false, comment: "数据源状态")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false, comment: "数据源类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "longtext", nullable: false, comment: "链接")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrevUpdateTime = table.Column<string>(type: "longtext", nullable: true, comment: "上次Feed更新时间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateFrequency = table.Column<long>(type: "bigint", nullable: true, comment: "更新频率"),
                    NextUpdateTime = table.Column<string>(type: "longtext", nullable: true, comment: "下次Feed更新时间")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.Id);
                },
                comment: "数据源")
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Blogs_BlogId",
                table: "Posts",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Posts_PostId",
                table: "PostTag",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tags_TagId",
                table: "PostTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_DataSources_SourceId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Blogs_BlogId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Posts_PostId",
                table: "PostTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tags_TagId",
                table: "PostTag");

            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_SourceId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "Blogs",
                newName: "Blog");

            migrationBuilder.RenameIndex(
                name: "IX_Tags_Name",
                table: "Tag",
                newName: "IX_Tag_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_Guid",
                table: "Post",
                newName: "IX_Post_Guid");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_BlogId",
                table: "Post",
                newName: "IX_Post_BlogId");

            migrationBuilder.AlterTable(
                name: "Blog",
                comment: "博客数据源",
                oldComment: "博客")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Feed",
                table: "Blog",
                type: "longtext",
                nullable: false,
                comment: "Feed")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "FeedNextUpdate",
                table: "Blog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "下次Feed更新时间");

            migrationBuilder.AddColumn<long>(
                name: "FeedPrevUpdate",
                table: "Blog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "上次Feed更新时间");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Blog",
                type: "longtext",
                nullable: false,
                defaultValue: "Ok",
                comment: "博客状态")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blog",
                table: "Blog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Blog_BlogId",
                table: "Post",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Post_PostId",
                table: "PostTag",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tag_TagId",
                table: "PostTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
