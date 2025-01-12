﻿// <auto-generated />
using System;
using LaundryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LaundryManagement.Infrastructure.Migrations
{
    [DbContext(typeof(LaundryDbContext))]
    partial class LaundryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Cycle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Cost")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("IdMachine")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdMachine");

                    b.ToTable("Cycles");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Laundry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdProprietaire")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdProprietaire");

                    b.ToTable("Laundries");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CurrentCycleEndTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CurrentCycleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CurrentCycleStartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdLaundry")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentCycleId");

                    b.HasIndex("IdLaundry");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdCycle")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdClient");

                    b.HasIndex("IdCycle");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Cycle", b =>
                {
                    b.HasOne("LaundryManagement.Infrastructure.Models.Machine", "Machine")
                        .WithMany("Cycles")
                        .HasForeignKey("IdMachine")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Machine");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Laundry", b =>
                {
                    b.HasOne("LaundryManagement.Infrastructure.Models.Owner", "Proprietaire")
                        .WithMany()
                        .HasForeignKey("IdProprietaire")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proprietaire");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Machine", b =>
                {
                    b.HasOne("LaundryManagement.Infrastructure.Models.Cycle", "CurrentCycle")
                        .WithMany()
                        .HasForeignKey("CurrentCycleId");

                    b.HasOne("LaundryManagement.Infrastructure.Models.Laundry", "Laundry")
                        .WithMany("Machines")
                        .HasForeignKey("IdLaundry")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentCycle");

                    b.Navigation("Laundry");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Transaction", b =>
                {
                    b.HasOne("LaundryManagement.Infrastructure.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("LaundryManagement.Infrastructure.Models.Cycle", "Cycle")
                        .WithMany()
                        .HasForeignKey("IdCycle")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Cycle");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Laundry", b =>
                {
                    b.Navigation("Machines");
                });

            modelBuilder.Entity("LaundryManagement.Infrastructure.Models.Machine", b =>
                {
                    b.Navigation("Cycles");
                });
#pragma warning restore 612, 618
        }
    }
}
