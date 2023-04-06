﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockMarket.Data;

#nullable disable

namespace StockMarket.Data.Migrations
{
    [DbContext(typeof(StockMarketDbContext))]
    [Migration("20230320152345_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StockMarket.Domain.Order", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("Money");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("StockMarket.Domain.Trade", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<long>("BuyOrderId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("Money");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("Money");

                    b.Property<long>("SellOrderId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BuyOrderId");

                    b.HasIndex("SellOrderId");

                    b.ToTable("Trades");
                });

            modelBuilder.Entity("StockMarket.Domain.Trade", b =>
                {
                    b.HasOne("StockMarket.Domain.Order", null)
                        .WithMany()
                        .HasForeignKey("BuyOrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StockMarket.Domain.Order", null)
                        .WithMany()
                        .HasForeignKey("SellOrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
