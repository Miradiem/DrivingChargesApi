using DrivingChargesApi.Charges.Data.CongestionData;
using DrivingChargesApi.Charges.Data.LowEmissionData;
using DrivingChargesApi.Charges.Data.UltraLowEmissionData;
using Microsoft.EntityFrameworkCore;

namespace DrivingChargesApi.Charges.Data
{
    public class ChargeContext : DbContext
    {
        public ChargeContext(DbContextOptions options) : base(options) { } 

        public DbSet<Congestion> Congestions { get; set; }

        public DbSet<Period> Periods { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<LowEmission> LowEmissions { get; set; }

        public DbSet<UltraLowEmission> UltraLowEmissions { get; set; }

        public DbSet<City> Cities { get; set; }
    }
}
