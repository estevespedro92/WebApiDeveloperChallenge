﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiDeveloperChallenge.Models;

namespace WebApiDeveloperChallenge.Migrations
{
    [DbContext(typeof(ContactsContext))]
    partial class ContactsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiDeveloperChallenge.Models.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(120)")
                        .HasMaxLength(120);

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("MobilePhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)")
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.HasIndex("Firstname", "Lastname")
                        .IsUnique();

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("WebApiDeveloperChallenge.Models.ContactSkill", b =>
                {
                    b.Property<Guid>("ContactId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SkillId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ContactId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("ContactSkills");
                });

            modelBuilder.Entity("WebApiDeveloperChallenge.Models.Skill", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)")
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.HasIndex("Name", "Level")
                        .IsUnique();

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("WebApiDeveloperChallenge.Models.ContactSkill", b =>
                {
                    b.HasOne("WebApiDeveloperChallenge.Models.Contact", "Contact")
                        .WithMany("ContactSkills")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApiDeveloperChallenge.Models.Skill", "Skill")
                        .WithMany("ContactSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
