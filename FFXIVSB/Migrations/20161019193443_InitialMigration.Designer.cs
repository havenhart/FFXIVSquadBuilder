using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FFXIVSB.Models;

namespace FFXIVSB.Migrations
{
    [DbContext(typeof(SBContext))]
    [Migration("20161019193443_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("FFXIVSB.Models.SquadMember", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Mental");

                    b.Property<string>("Name");

                    b.Property<int>("Physical");

                    b.Property<int>("Tactical");

                    b.HasKey("ID");

                    b.ToTable("SquadMembers");
                });
        }
    }
}
