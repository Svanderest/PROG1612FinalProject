﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using svanderest1_College_Strike.Data;

namespace svanderest1_College_Strike.Data.FPMigrations
{
    [DbContext(typeof(svanderest1_College_StrikeContext))]
    partial class svanderest1_College_StrikeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("FP")
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("svanderest1_College_Strike.Models.Assignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Assignment");
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.Member", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignmentID");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<long>("Phone");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<string>("eMail")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.HasIndex("AssignmentID");

                    b.HasIndex("eMail")
                        .IsUnique();

                    b.ToTable("Member");
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.MemberPosition", b =>
                {
                    b.Property<int>("MemberID");

                    b.Property<int>("PositionID");

                    b.HasKey("MemberID", "PositionID");

                    b.HasIndex("PositionID");

                    b.ToTable("MemberPosition");
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.Position", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("ID");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Position");
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.Shift", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignmentID");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime>("Date");

                    b.Property<int>("MemberID");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("ID");

                    b.HasIndex("AssignmentID");

                    b.HasIndex("MemberID", "Date")
                        .IsUnique();

                    b.ToTable("Shift");
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.Member", b =>
                {
                    b.HasOne("svanderest1_College_Strike.Models.Assignment", "Assignment")
                        .WithMany("Members")
                        .HasForeignKey("AssignmentID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.MemberPosition", b =>
                {
                    b.HasOne("svanderest1_College_Strike.Models.Member", "Member")
                        .WithMany("Positions")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("svanderest1_College_Strike.Models.Position", "Position")
                        .WithMany("Members")
                        .HasForeignKey("PositionID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("svanderest1_College_Strike.Models.Shift", b =>
                {
                    b.HasOne("svanderest1_College_Strike.Models.Assignment", "Assignment")
                        .WithMany("Shifts")
                        .HasForeignKey("AssignmentID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("svanderest1_College_Strike.Models.Member", "Member")
                        .WithMany("Shifts")
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
