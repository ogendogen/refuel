﻿using System;
using System.Linq;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class RefuelContext : DbContext
    {
        internal DbSet<User> Users { get; set; }
        internal DbSet<Vehicle> Vehicles { get; set; }
        internal DbSet<Refuel> Refuels { get; set; }

        public RefuelContext()
        {

        }

        public RefuelContext(DbContextOptions<RefuelContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=refuel;user=root;password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Login).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Login).IsUnique();

                entity.Property(e => e.Password).HasMaxLength(64);
                entity.Property(e => e.Salt).HasMaxLength(20);
                
                entity.Property(e => e.Email).IsRequired().HasMaxLength(64);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.RegisterDate).IsRequired();
                entity.Property(e => e.VerificationCode).IsRequired().HasMaxLength(32).HasDefaultValue("0");

                entity.Property(e => e.ExternalProvider).HasMaxLength(32);

                entity.HasMany(e => e.Vehicles)
                    .WithOne(e => e.Owner);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Manufacturer).IsRequired().HasMaxLength(32);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(32);

                entity.Property(e => e.Engine).IsRequired();

                entity.Property(e => e.Horsepower).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasOne(e => e.Owner)
                    .WithMany(e => e.Vehicles)
                    .IsRequired();

                entity.HasMany(e => e.Refuels)
                    .WithOne(e => e.Vehicle)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Refuel>(entity =>
            {
                entity.Property(e => e.ID).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Kilometers).IsRequired();
                entity.Property(e => e.PricePerLiter).IsRequired();
                entity.Property(e => e.Liters).IsRequired();
                entity.Property(e => e.Combustion).IsRequired();
                entity.Property(e => e.Fuel).IsRequired();

                entity.HasOne(e => e.Vehicle)
                    .WithMany(e => e.Refuels)
                    .IsRequired();
            });

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(6, 2)");
            }
        }
    }
}
