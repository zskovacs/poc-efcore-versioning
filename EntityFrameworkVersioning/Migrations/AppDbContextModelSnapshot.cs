﻿// <auto-generated />
using System;
using EntityFrameworkVersioning.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EntityFrameworkVersioning.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.ArticleBaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlogId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("ArticleBases");
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.ArticleEntity", b =>
                {
                    b.Property<Guid>("BaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Revision")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("BaseId", "Revision");

                    b.ToTable("Articles", (string)null);
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.BlogBaseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("BlogBases");
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.BlogEntity", b =>
                {
                    b.Property<Guid>("BaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Revision")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("BaseId", "Revision");

                    b.ToTable("Blogs", (string)null);
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.ArticleBaseEntity", b =>
                {
                    b.HasOne("EntityFrameworkVersioning.Entities.BlogBaseEntity", "Blog")
                        .WithMany()
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.ArticleEntity", b =>
                {
                    b.HasOne("EntityFrameworkVersioning.Entities.ArticleBaseEntity", "Base")
                        .WithMany()
                        .HasForeignKey("BaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Base");
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.BlogEntity", b =>
                {
                    b.HasOne("EntityFrameworkVersioning.Entities.BlogBaseEntity", "Base")
                        .WithMany("Details")
                        .HasForeignKey("BaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Base");
                });

            modelBuilder.Entity("EntityFrameworkVersioning.Entities.BlogBaseEntity", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
