using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogrolling.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "博客ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "博客名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Owner = table.Column<string>(type: "longtext", nullable: true, comment: "博主")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "longtext", nullable: false, comment: "博客链接")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                },
                comment: "博客")
                .Annotation("MySql:CharSet", "utf8mb4");

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
                    LastUpdateTime = table.Column<string>(type: "longtext", nullable: true, comment: "上次Feed更新时间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrevFetchTime = table.Column<string>(type: "longtext", nullable: true, comment: "上次获取时间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NextFetchTime = table.Column<string>(type: "longtext", nullable: true, comment: "下次获取时间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlogId = table.Column<int>(type: "int", nullable: false, comment: "博客ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataSources_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "数据源")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "文章ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false, comment: "文章标题")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "文章简介")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Author = table.Column<string>(type: "longtext", nullable: true, comment: "文章作者")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "longtext", nullable: false, comment: "链接")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublishTime = table.Column<string>(type: "longtext", nullable: false, comment: "发布时间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<string>(type: "longtext", nullable: true, comment: "更新时间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlogId = table.Column<int>(type: "int", nullable: false, comment: "博客ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "博客文章")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "标签Id")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false, comment: "标签名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Link = table.Column<string>(type: "longtext", nullable: true, comment: "标签链接")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BlogId = table.Column<int>(type: "int", nullable: false, comment: "博客Id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "标签")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false, comment: "文章ID"),
                    TagId = table.Column<int>(type: "int", nullable: false, comment: "标签ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "文章标签")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_BlogId",
                table: "DataSources",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_BlogId",
                table: "Tags",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name_BlogId",
                table: "Tags",
                columns: new[] { "Name", "BlogId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
