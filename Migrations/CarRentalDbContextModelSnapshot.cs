﻿// <auto-generated />
using System;
using System.Collections.Generic;
using CarRentalApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarRentalApp.Migrations
{
    [DbContext(typeof(CarRentalDbContext))]
    partial class CarRentalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CarRentalApp.Models.Domain.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DailyRate")
                        .HasColumnType("numeric");

                    b.Property<List<string>>("Features")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarRentalApp.Models.Domain.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DrivingLicense")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal?>("ReturnFuelLevel")
                        .HasColumnType("numeric");

                    b.Property<int?>("ReturnMileage")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("CarRentalApp.Models.Domain.Rental", b =>
                {
                    b.HasOne("CarRentalApp.Models.Domain.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });
#pragma warning restore 612, 618
        }
    }
}
