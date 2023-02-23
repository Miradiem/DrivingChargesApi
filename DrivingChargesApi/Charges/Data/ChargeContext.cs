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

        public DbSet<UserData> Users { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Congestion> Congestions { get; set; }

        public DbSet<Period> Periods { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<LowEmission> LowEmissions { get; set; }

        public DbSet<UltraLowEmission> UltraLowEmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedWithRandomUserData(modelBuilder);
            SeedCongestion(modelBuilder);
            
        }

        private static void SeedCongestion(ModelBuilder modelBuilder)
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
                    Type = "WeekDay",
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

        private static void SeedWithRandomUserData(ModelBuilder modelBuilder)
        {
            var random = new Random();
            var vehicles = new List<string>()
            {
                "Car",
                "Van",
                "Motorbike"
            };
            var start = new DateTime(2023, 01, 01, 00, 00, 00);
            var end = new DateTime(2023, 01, 31, 23, 59, 58);
            
            for (int userId = 1; userId < 100; userId++)
            {
                var enterRange = (int)(end - start).TotalSeconds;
                var entered = start.AddSeconds(random.Next(enterRange));

                var leaveRange = (int)(end - entered.AddSeconds(1)).TotalSeconds;
                var left = entered.AddSeconds(random.Next(leaveRange));

                modelBuilder.Entity<UserData>().HasData(new UserData
                {
                    UserDataId = userId,
                    Entered = entered,
                    Left = left,
                    Vehicle = vehicles[random.Next(vehicles.Count)]
                });
            }
        }
    }
}