﻿// <auto-generated />
using System;
using Cloud_Assignment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cloud_Assignment.Migrations
{
    [DbContext(typeof(Cloud_AssignmentContext))]
    partial class Cloud_AssignmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserAge")
                        .HasColumnType("int");

                    b.Property<DateTime>("UserDOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserFullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserRole")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Cloud_Assignment.Models.DistributionSchedule", b =>
                {
                    b.Property<int>("DistributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DistributionId"));

                    b.Property<DateTime>("DOD")
                        .HasColumnType("datetime2");

                    b.Property<string>("DistributionStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DistributorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageS3Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RecordId")
                        .HasColumnType("int");

                    b.Property<string>("RecordType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DistributionId");

                    b.HasIndex("DistributorId");

                    b.HasIndex("RecordId");

                    b.ToTable("DistributionSchedule");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.FinancialRecord", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecordId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CurrencyType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOR")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecordType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RecordId");

                    b.HasIndex("UserId");

                    b.ToTable("FinancialRecord");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.FoodRecord", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecordId"));

                    b.Property<DateTime>("DOR")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FoodId")
                        .HasColumnType("int");

                    b.Property<int?>("FoodQuantity")
                        .HasColumnType("int");

                    b.Property<string>("RecordType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RecordId");

                    b.HasIndex("FoodId");

                    b.HasIndex("UserId");

                    b.ToTable("FoodRecord");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.InventoryRecord", b =>
                {
                    b.Property<int>("FoodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FoodId"));

                    b.Property<string>("FoodDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FoodName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FoodTotal")
                        .HasColumnType("int");

                    b.HasKey("FoodId");

                    b.ToTable("InventoryRecord");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.NewsletterEmail", b =>
                {
                    b.Property<int>("NewsletterEmailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NewsletterEmailId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NewsletterEmailId");

                    b.ToTable("NewsletterEmail");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.RequestRecord", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecordId"));

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DOR")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FoodId")
                        .HasColumnType("int");

                    b.Property<int?>("FoodQuantity")
                        .HasColumnType("int");

                    b.Property<string>("RequestDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("requestType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RecordId");

                    b.HasIndex("FoodId");

                    b.HasIndex("StaffId");

                    b.HasIndex("UserId");

                    b.ToTable("RequestRecord");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Cloud_Assignment.Models.DistributionSchedule", b =>
                {
                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", "Distributor")
                        .WithMany()
                        .HasForeignKey("DistributorId");

                    b.HasOne("Cloud_Assignment.Models.RequestRecord", "Record")
                        .WithMany()
                        .HasForeignKey("RecordId");

                    b.Navigation("Distributor");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.FinancialRecord", b =>
                {
                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.FoodRecord", b =>
                {
                    b.HasOne("Cloud_Assignment.Models.InventoryRecord", "InventoryRecord")
                        .WithMany()
                        .HasForeignKey("FoodId");

                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("InventoryRecord");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Cloud_Assignment.Models.RequestRecord", b =>
                {
                    b.HasOne("Cloud_Assignment.Models.InventoryRecord", "InventoryRecord")
                        .WithMany()
                        .HasForeignKey("FoodId");

                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", "Staff")
                        .WithMany()
                        .HasForeignKey("StaffId");

                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("InventoryRecord");

                    b.Navigation("Staff");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Cloud_Assignment.Areas.Identity.Data.Cloud_AssignmentUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
