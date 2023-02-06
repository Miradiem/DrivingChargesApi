using DrivingChargesApi.Charges.Data.CongestionData;
using DrivingChargesApi.Charges.Data.LowEmissionData;
using DrivingChargesApi.Charges.Data.UltraLowEmissionData;
using Microsoft.EntityFrameworkCore;

namespace DrivingChargesApi.Charges.Data
{
    public class ChargeContext : DbContext
    {
        public ChargeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<Congestion> Congestions { get; set; }

        public DbSet<Period> Periods { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<LowEmission> LowEmissions { get; set; }

        public DbSet<UltraLowEmission> UltraLowEmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedWithData(modelBuilder);
        }

        private static void SeedWithData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City
                {
                    CityId = 1,
                    Name = "London",
                    Coefficient = 1,
                });
            modelBuilder.Entity<Congestion>().HasData(
                new Congestion
                {
                    CongestionId = 1,
                    Validity = "Weekly",
                    Coefficient = 1,
                    CityId = 1
                });
            modelBuilder.Entity<Period>().HasData(
                new Period
                {
                    PeriodId = 1,
                    Start = new(07, 00, 00),
                    End = new(12, 00, 00),
                    Coefficient = 1,
                    CongestionId = 1
                });
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    VehicleId = 1,
                    Type = "Car",
                    Rate = 2,
                    PeriodId = 1
                },
                new Vehicle
                {
                    VehicleId = 2,
                    Type = "Van",
                    Rate = 3,
                    PeriodId = 1
                },
                new Vehicle
                {
                    VehicleId = 3,
                    Type = "Motorbike",
                    Rate = 1,
                    PeriodId = 1
                });
            modelBuilder.Entity<Period>().HasData(
               new Period
               {
                    PeriodId = 2,
                    Start = new(12, 00, 00),
                    End = new(19, 00, 00),
                    Coefficient = 1,
                    CongestionId = 1
               });
            modelBuilder.Entity<Vehicle>().HasData(
               new Vehicle
               {
                   VehicleId = 4,
                   Type = "Car",
                   Rate = 2.5,
                   PeriodId = 2
               },
               new Vehicle
               {
                   VehicleId = 5,
                   Type = "Van",
                   Rate = 3.5,
                   PeriodId = 2
               },
               new Vehicle
               {
                   VehicleId = 6,
                   Type = "Motorbike",
                   Rate = 1,
                   PeriodId = 2
               });
        }
    }
}