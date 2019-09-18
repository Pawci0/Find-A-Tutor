﻿// <auto-generated />
using System;
using Find_A_Tutor.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Find_A_Tutor.Infrastructure.Migrations
{
    [DbContext(typeof(FindATurorContext))]
    partial class FindATurorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Find_A_Tutor.Core.Domain.PrivateLesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDone");

                    b.Property<bool>("IsPaid");

                    b.Property<DateTime>("RelevantTo");

                    b.Property<Guid?>("SchoolSubjectId");

                    b.Property<Guid>("StudnetId");

                    b.Property<DateTime?>("TakenAt");

                    b.Property<Guid?>("TutorId");

                    b.Property<DateTime?>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("SchoolSubjectId");

                    b.ToTable("PrivateLessons");
                });

            modelBuilder.Entity("Find_A_Tutor.Core.Domain.SchoolSubject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("SchoolSubjects");
                });

            modelBuilder.Entity("Find_A_Tutor.Core.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Find_A_Tutor.Core.Domain.PrivateLesson", b =>
                {
                    b.HasOne("Find_A_Tutor.Core.Domain.SchoolSubject", "SchoolSubject")
                        .WithMany()
                        .HasForeignKey("SchoolSubjectId");
                });
#pragma warning restore 612, 618
        }
    }
}