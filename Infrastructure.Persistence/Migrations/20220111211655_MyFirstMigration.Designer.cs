﻿// <auto-generated />
using System;
using Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20220111211655_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.HasSequence("orderitemseq")
                .IncrementsBy(10);

            modelBuilder.HasSequence("orderseq", "ordering")
                .IncrementsBy(10);

            modelBuilder.Entity("Domain.Core.Entities.BuyerAggregate.Buyer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("IdentityGuid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Buyers");
                });

            modelBuilder.Entity("Domain.Core.Entities.OrderAggrigate.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("orderseq", "ordering");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("_buyerId")
                        .HasColumnType("int")
                        .HasColumnName("BuyerId");

                    b.Property<DateTime>("_orderDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("OrderDate");

                    b.HasKey("Id");

                    b.HasIndex("_buyerId");

                    b.ToTable("orders", "ordering");
                });

            modelBuilder.Entity("Domain.Core.Entities.OrderAggrigate.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseHiLo("orderitemseq");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal>("_discount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Discount");

                    b.Property<string>("_pictureUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("PictureUrl");

                    b.Property<string>("_productName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ProductName");

                    b.Property<decimal>("_unitPrice")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("UnitPrice");

                    b.Property<int>("_units")
                        .HasColumnType("int")
                        .HasColumnName("Units");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("orderItems", "ordering");
                });

            modelBuilder.Entity("Domain.Core.Entities.OrderAggrigate.Order", b =>
                {
                    b.HasOne("Domain.Core.Entities.BuyerAggregate.Buyer", null)
                        .WithMany()
                        .HasForeignKey("_buyerId");

                    b.OwnsOne("Domain.Core.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<int>("OrderId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .UseHiLo("orderseq", "ordering");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ZipCode")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OrderId");

                            b1.ToTable("orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Domain.Core.Entities.OrderAggrigate.OrderItem", b =>
                {
                    b.HasOne("Domain.Core.Entities.OrderAggrigate.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Core.Entities.OrderAggrigate.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
