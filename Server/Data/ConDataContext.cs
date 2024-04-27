using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoringSystem.Server.Models.ConData;

namespace VehicleMonitoringSystem.Server.Data
{
    public partial class ConDataContext : DbContext
    {
        public ConDataContext()
        {
        }

        public ConDataContext(DbContextOptions<ConDataContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>()
              .HasOne(i => i.Vehicle)
              .WithMany(i => i.GpsData)
              .HasForeignKey(i => i.VehicleID)
              .HasPrincipalKey(i => i.VehicleID);

            builder.Entity<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>()
              .HasOne(i => i.Vehicle)
              .WithMany(i => i.SpeedMeasurements)
              .HasForeignKey(i => i.VehicleID)
              .HasPrincipalKey(i => i.VehicleID);

            builder.Entity<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>()
              .Property(p => p.DateAndTimeInserted)
              .HasColumnType("datetime");

            builder.Entity<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>()
              .Property(p => p.DateAndTimeInserted)
              .HasColumnType("datetime");
            this.OnModelBuilding(builder);
        }

        public DbSet<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> GpsData { get; set; }

        public DbSet<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> SpeedMeasurements { get; set; }

        public DbSet<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> Vehicles { get; set; }

        public DbSet<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> SpeedClassifications { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}