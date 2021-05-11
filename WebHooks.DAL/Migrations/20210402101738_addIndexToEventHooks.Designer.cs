﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebHooks.DAL.EF;

namespace WebHooks.DAL.Migrations
{
    [DbContext(typeof(WebHooksContext))]
    [Migration("20210402101738_addIndexToEventHooks")]
    partial class addIndexToEventHooks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebHooks.DAL.Models.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeactualizeTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActual")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.EventHook", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeactualizeTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Event")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActual")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("SendTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("SubscriptionId")
                        .HasColumnType("bigint");

                    b.Property<int>("TryCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.HasIndex("IsActual", "SendTime", "SubscriptionId");

                    b.HasIndex("IsActual", "SubscriptionId", "SendTime");

                    b.HasIndex("IsActual", "Status", "TryCount", "SubscriptionId");

                    b.ToTable("EventHooks");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.HookTry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeactualizeTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("EventHookId")
                        .HasColumnType("bigint");

                    b.Property<string>("HttpStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActual")
                        .HasColumnType("bit");

                    b.Property<string>("Request")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventHookId");

                    b.ToTable("HookTryes");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeactualizeTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ExternalAccountId")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HttpMethod")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActual")
                        .HasColumnType("bit");

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("IsActual", "AccountId", "ExternalAccountId", "EventId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.EventHook", b =>
                {
                    b.HasOne("WebHooks.DAL.Models.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.HookTry", b =>
                {
                    b.HasOne("WebHooks.DAL.Models.EventHook", "EventHook")
                        .WithMany("HookTryes")
                        .HasForeignKey("EventHookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventHook");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.Subscription", b =>
                {
                    b.HasOne("WebHooks.DAL.Models.Account", "Account")
                        .WithMany("Subscriptions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.Account", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("WebHooks.DAL.Models.EventHook", b =>
                {
                    b.Navigation("HookTryes");
                });
#pragma warning restore 612, 618
        }
    }
}
