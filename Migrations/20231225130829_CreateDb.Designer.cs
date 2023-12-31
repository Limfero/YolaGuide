﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YolaGuide.DAL;

#nullable disable

namespace YolaGuide.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231225130829_CreateDb")]
    partial class CreateDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CategoryPlace", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<int>("PlacesId")
                        .HasColumnType("int");

                    b.HasKey("CategoriesId", "PlacesId");

                    b.HasIndex("PlacesId");

                    b.ToTable("place_has_category", (string)null);
                });

            modelBuilder.Entity("CategoryUser", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("int");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("CategoriesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("user_has_category", (string)null);
                });

            modelBuilder.Entity("PlaceRoute", b =>
                {
                    b.Property<int>("PlacesId")
                        .HasColumnType("int");

                    b.Property<int>("RoutesId")
                        .HasColumnType("int");

                    b.HasKey("PlacesId", "RoutesId");

                    b.HasIndex("RoutesId");

                    b.ToTable("route_has_place", (string)null);
                });

            modelBuilder.Entity("PlaceUser", b =>
                {
                    b.Property<int>("PlacesId")
                        .HasColumnType("int");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("PlacesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("user_has_place", (string)null);
                });

            modelBuilder.Entity("RouteUser", b =>
                {
                    b.Property<int>("RoutesId")
                        .HasColumnType("int");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("RoutesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("user_has_route", (string)null);
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_category");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<int?>("SubcategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("category", (string)null);
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.Fact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_fact");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("fact", (string)null);
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.Place", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_place");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("adress");

                    b.Property<string>("ContactInformation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("contact_information");

                    b.Property<string>("Coordinates")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("coordinates");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("route_to_image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<long>("YIdOrganization")
                        .HasColumnType("bigint")
                        .HasColumnName("yid_org");

                    b.HasKey("Id");

                    b.ToTable("place", (string)null);
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id_route");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Cost")
                        .HasColumnType("money")
                        .HasColumnName("cost");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)")
                        .HasColumnName("telephone");

                    b.HasKey("Id");

                    b.ToTable("route", (string)null);
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id_user");

                    b.Property<int>("Language")
                        .HasColumnType("int")
                        .HasColumnName("language");

                    b.Property<int>("State")
                        .HasColumnType("int")
                        .HasColumnName("state");

                    b.Property<int>("Substate")
                        .HasColumnType("int")
                        .HasColumnName("substate");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("CategoryPlace", b =>
                {
                    b.HasOne("YolaGuide.Domain.Entity.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YolaGuide.Domain.Entity.Place", null)
                        .WithMany()
                        .HasForeignKey("PlacesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CategoryUser", b =>
                {
                    b.HasOne("YolaGuide.Domain.Entity.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YolaGuide.Domain.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlaceRoute", b =>
                {
                    b.HasOne("YolaGuide.Domain.Entity.Place", null)
                        .WithMany()
                        .HasForeignKey("PlacesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YolaGuide.Domain.Entity.Route", null)
                        .WithMany()
                        .HasForeignKey("RoutesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlaceUser", b =>
                {
                    b.HasOne("YolaGuide.Domain.Entity.Place", null)
                        .WithMany()
                        .HasForeignKey("PlacesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YolaGuide.Domain.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RouteUser", b =>
                {
                    b.HasOne("YolaGuide.Domain.Entity.Route", null)
                        .WithMany()
                        .HasForeignKey("RoutesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("YolaGuide.Domain.Entity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.Category", b =>
                {
                    b.HasOne("YolaGuide.Domain.Entity.Category", "Subcategory")
                        .WithMany("Subcategories")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("subcategory");

                    b.Navigation("Subcategory");
                });

            modelBuilder.Entity("YolaGuide.Domain.Entity.Category", b =>
                {
                    b.Navigation("Subcategories");
                });
#pragma warning restore 612, 618
        }
    }
}
