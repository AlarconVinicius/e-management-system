﻿// <auto-generated />
using EMS.WebApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace EMS.WebApi.Data.Migrations;

[DbContext(typeof(EMSDbContext))]
partial class EMSDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "7.0.17")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("EMS.WebApi.Business.Models.Address", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("City")
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                b.Property<string>("Complement")
                    .HasColumnType("varchar(250)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Neighborhood")
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                b.Property<string>("Number")
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                b.Property<string>("State")
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                b.Property<string>("Street")
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("ZipCode")
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                b.HasKey("Id");

                b.HasIndex("UserId")
                    .IsUnique();

                b.ToTable("Addresses", (string)null);
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Company", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Brand")
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                b.Property<Guid>("PlanId")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("PlanId");

                b.ToTable("Companies", (string)null);
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Plan", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Benefits")
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<decimal>("Price")
                    .HasColumnType("decimal(18,2)");

                b.Property<string>("Subtitle")
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasColumnType("varchar(250)");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.ToTable("Plans", (string)null);
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Product", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("CompanyId")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("varchar(1000)");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                b.Property<decimal>("UnitaryValue")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValue(0m);

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("CompanyId");

                b.ToTable("Products", null, t =>
                    {
                        t.HasCheckConstraint("CK_Product_UnitaryValue", "UnitaryValue >= 0");
                    });
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.User", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("CompanyId")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("varchar(200)");

                b.Property<string>("PhoneNumber")
                    .IsRequired()
                    .HasColumnType("varchar(15)");

                b.Property<short>("Role")
                    .HasColumnType("SMALLINT");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("CompanyId");

                b.ToTable("Users", (string)null);

                b.UseTptMappingStrategy();
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Client", b =>
            {
                b.HasBaseType("EMS.WebApi.Business.Models.User");

                b.ToTable("Clients", (string)null);
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Employee", b =>
            {
                b.HasBaseType("EMS.WebApi.Business.Models.User");

                b.Property<decimal>("Salary")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValue(0m);

                b.ToTable("Employees", null, t =>
                    {
                        t.HasCheckConstraint("CK_Employee_Salary", "Salary >= 0");
                    });
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Address", b =>
            {
                b.HasOne("EMS.WebApi.Business.Models.User", "User")
                    .WithOne("Address")
                    .HasForeignKey("EMS.WebApi.Business.Models.Address", "UserId")
                    .IsRequired();

                b.Navigation("User");
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Company", b =>
            {
                b.HasOne("EMS.WebApi.Business.Models.Plan", "Plan")
                    .WithMany("Companies")
                    .HasForeignKey("PlanId")
                    .IsRequired();

                b.OwnsOne("EMS.Core.DomainObjects.Cpf", "Document", b1 =>
                    {
                        b1.Property<Guid>("CompanyId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("Number")
                            .IsRequired()
                            .HasMaxLength(11)
                            .HasColumnType("varchar(11)")
                            .HasColumnName("Cpf");

                        b1.HasKey("CompanyId");

                        b1.ToTable("Companies");

                        b1.WithOwner()
                            .HasForeignKey("CompanyId");
                    });

                b.Navigation("Document");

                b.Navigation("Plan");
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Product", b =>
            {
                b.HasOne("EMS.WebApi.Business.Models.Company", "Company")
                    .WithMany("Products")
                    .HasForeignKey("CompanyId")
                    .IsRequired();

                b.Navigation("Company");
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.User", b =>
            {
                b.HasOne("EMS.WebApi.Business.Models.Company", "Company")
                    .WithMany("Users")
                    .HasForeignKey("CompanyId")
                    .IsRequired();

                b.OwnsOne("EMS.Core.DomainObjects.Email", "Email", b1 =>
                    {
                        b1.Property<Guid>("UserId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("Address")
                            .IsRequired()
                            .HasColumnType("varchar(254)")
                            .HasColumnName("Email");

                        b1.HasKey("UserId");

                        b1.ToTable("Users");

                        b1.WithOwner()
                            .HasForeignKey("UserId");
                    });

                b.OwnsOne("EMS.Core.DomainObjects.Cpf", "Document", b1 =>
                    {
                        b1.Property<Guid>("UserId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("Number")
                            .IsRequired()
                            .HasMaxLength(11)
                            .HasColumnType("varchar(11)")
                            .HasColumnName("Cpf");

                        b1.HasKey("UserId");

                        b1.ToTable("Users");

                        b1.WithOwner()
                            .HasForeignKey("UserId");
                    });

                b.Navigation("Company");

                b.Navigation("Document");

                b.Navigation("Email");
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Client", b =>
            {
                b.HasOne("EMS.WebApi.Business.Models.User", null)
                    .WithOne()
                    .HasForeignKey("EMS.WebApi.Business.Models.Client", "Id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Employee", b =>
            {
                b.HasOne("EMS.WebApi.Business.Models.User", null)
                    .WithOne()
                    .HasForeignKey("EMS.WebApi.Business.Models.Employee", "Id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Company", b =>
            {
                b.Navigation("Products");

                b.Navigation("Users");
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.Plan", b =>
            {
                b.Navigation("Companies");
            });

        modelBuilder.Entity("EMS.WebApi.Business.Models.User", b =>
            {
                b.Navigation("Address");
            });
#pragma warning restore 612, 618
    }
}
