﻿// <auto-generated />
using Blogrolling.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blogrolling.Migrations
{
    [DbContext(typeof(BlogrollingContext))]
    [Migration("20240621100237_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Blogrolling.Database.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("博客ID");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("博客链接");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("博客名称");

                    b.Property<string>("Owner")
                        .HasColumnType("longtext")
                        .HasComment("博主");

                    b.HasKey("Id");

                    b.ToTable("Blogs", t =>
                        {
                            t.HasComment("博客");
                        });
                });

            modelBuilder.Entity("Blogrolling.Database.DataSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("源ID");

                    b.Property<int>("BlogId")
                        .HasColumnType("int")
                        .HasComment("博客ID");

                    b.Property<string>("LastUpdateTime")
                        .HasColumnType("longtext")
                        .HasComment("上次Feed更新时间");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("链接");

                    b.Property<string>("NextFetchTime")
                        .HasColumnType("longtext")
                        .HasComment("下次获取时间");

                    b.Property<string>("PrevFetchTime")
                        .HasColumnType("longtext")
                        .HasComment("上次获取时间");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("数据源状态");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("数据源类型");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("DataSources", t =>
                        {
                            t.HasComment("数据源");
                        });
                });

            modelBuilder.Entity("Blogrolling.Database.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("文章ID");

                    b.Property<string>("Author")
                        .HasColumnType("longtext")
                        .HasComment("文章作者");

                    b.Property<int>("BlogId")
                        .HasColumnType("int")
                        .HasComment("博客ID");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasComment("文章简介");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("链接");

                    b.Property<string>("PublishTime")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("发布时间");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasComment("文章标题");

                    b.Property<string>("UpdateTime")
                        .HasColumnType("longtext")
                        .HasComment("更新时间");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts", t =>
                        {
                            t.HasComment("博客文章");
                        });
                });

            modelBuilder.Entity("Blogrolling.Database.PostTag", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int")
                        .HasComment("文章ID");

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasComment("标签ID");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTag", t =>
                        {
                            t.HasComment("文章标签");
                        });
                });

            modelBuilder.Entity("Blogrolling.Database.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("标签Id");

                    b.Property<int>("BlogId")
                        .HasColumnType("int")
                        .HasComment("博客Id");

                    b.Property<string>("Link")
                        .HasColumnType("longtext")
                        .HasComment("标签链接");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasComment("标签名");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("Name", "BlogId")
                        .IsUnique();

                    b.ToTable("Tags", t =>
                        {
                            t.HasComment("标签");
                        });
                });

            modelBuilder.Entity("Blogrolling.Database.DataSource", b =>
                {
                    b.HasOne("Blogrolling.Database.Blog", "Blog")
                        .WithMany("Sources")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("Blogrolling.Database.Post", b =>
                {
                    b.HasOne("Blogrolling.Database.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("Blogrolling.Database.PostTag", b =>
                {
                    b.HasOne("Blogrolling.Database.Post", null)
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blogrolling.Database.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Blogrolling.Database.Tag", b =>
                {
                    b.HasOne("Blogrolling.Database.Blog", "Blog")
                        .WithMany("Tags")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("Blogrolling.Database.Blog", b =>
                {
                    b.Navigation("Posts");

                    b.Navigation("Sources");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
