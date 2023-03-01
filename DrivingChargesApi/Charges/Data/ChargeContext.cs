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
            SeedCongestion(modelBuilder);
            
        }

        private static void SeedCongestion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    Name = "London",
                    Coefficient = 1,
                });
            modelBuilder.Entity<Congestion>().HasData(
                new Congestion
                {
                    Id = 1,
                    Type = "WeekDay",
                    Coefficient = 1,
                    CityId = 1
                });
            modelBuilder.Entity<Congestion>().HasData(
                new Congestion
                {
                    Id = 2,
                    Type = "WeekEnd",
                    Coefficient = 1,
                    CityId = 1
                });
            modelBuilder.Entity<Period>().HasData(
                new Period
                {
                    Id = 1,
                    Type = "Am",
                    Start = new(07, 00, 00),
                    End = new(12, 00, 00),
                    Coefficient = 1,
                    CongestionId = 1
                });
            modelBuilder.Entity<Period>().HasData(
                new Period
                {
                    Id = 2,
                    Type = "Pm",
                    Start = new(12, 00, 00),
                    End = new(19, 00, 00),
                    Coefficient = 1,
                    CongestionId = 1
                });
            modelBuilder.Entity<Period>().HasData(
              new Period
              {
                  Id = 3,
                  Type = "Am",
                  Start = new(07, 00, 00),
                  End = new(12, 00, 00),
                  Coefficient = 1,
                  CongestionId = 2
              });
            modelBuilder.Entity<Period>().HasData(
                new Period
                {
                    Id = 4,
                    Type = "Pm",
                    Start = new(12, 00, 00),
                    End = new(19, 00, 00),
                    Coefficient = 1,
                    CongestionId = 2
                });
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    Id = 1,
                    Type = "Car",
                    Rate = 2,
                    PeriodId = 1
                },
                new Vehicle
                {
                    Id = 2,
                    Type = "Van",
                    Rate = 3,
                    PeriodId = 1
                },
                new Vehicle
                {
                    Id = 3,
                    Type = "Motorbike",
                    Rate = 1,
                    PeriodId = 1
                });        
            modelBuilder.Entity<Vehicle>().HasData(
               new Vehicle
               {
                   Id = 4,
                   Type = "Car",
                   Rate = 2.5,
                   PeriodId = 2
               },
               new Vehicle
               {
                   Id = 5,
                   Type = "Van",
                   Rate = 3.5,
                   PeriodId = 2
               },
               new Vehicle
               {
                   Id = 6,
                   Type = "Motorbike",
                   Rate = 1,
                   PeriodId = 2
               });
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    Id = 7,
                    Type = "Car",
                    Rate = 4,
                    PeriodId = 3
                },
                new Vehicle
                {
                    Id = 8,
                    Type = "Van",
                    Rate = 5,
                    PeriodId = 3
                },
                new Vehicle
                {
                    Id = 9,
                    Type = "Motorbike",
                    Rate = 2,
                    PeriodId = 3
                });
            modelBuilder.Entity<Vehicle>().HasData(
               new Vehicle
               {
                   Id = 10,
                   Type = "Car",
                   Rate = 4.5,
                   PeriodId = 2
               },
               new Vehicle
               {
                   Id = 11,
                   Type = "Van",
                   Rate = 5.5,
                   PeriodId = 2
               },
               new Vehicle
               {
                   Id = 12,
                   Type = "Motorbike",
                   Rate = 2,
                   PeriodId = 2
               });
        }
    }
}