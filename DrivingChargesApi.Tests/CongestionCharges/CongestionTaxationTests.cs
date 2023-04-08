using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data;
using DrivingChargesApi.Data.CongestionData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace DrivingChargesApi.Tests.CongestionCharges
{
    public class CongestionTaxationTests
    {
        private readonly DbContextOptions<ChargeContext> _options;
        private readonly ChargeContext _context;
        private readonly CongestionRepository _congestionRepository;

        public CongestionTaxationTests()
        {
            _options = new DbContextOptionsBuilder<ChargeContext>()
                .UseInMemoryDatabase(databaseName: "ChargesDataBase")
                .Options;
            _context = new ChargeContext(_options);
            _congestionRepository = new CongestionRepository(_context);
            _ = CreateTestData("London");
        }

        [Fact]
        public async Task ShouldGetCongestionCharge()
        {
            string cityName = "London";
            string vehicleType = "Car";
            DateTime entered = new DateTime(2023, 01, 02, 11, 00, 00);
            DateTime left = new DateTime(2023, 01, 02, 13, 00, 00);
            var taxation = new CongestionTaxation(_congestionRepository);
            var result = await taxation.GetCongestionCharge(cityName, vehicleType, entered, left);
            result.TotalCharge.Should().Be(10);
        }

        private async Task CreateTestData(string cityName)
        {
            var vehicles = new List<Vehicle>()
            {
                new Vehicle
                {
                    Id = 1,
                    Type = "Car",
                    Rate = 1,
                    PeriodId = 1
                }
            };

            var periods = new List<Period>()
            {
                new Period
                {
                    Id = 1,
                    Type = "Am",
                    Start = new(),
                    End = new(),
                    Coefficient = 1,
                    CongestionId = 1,
                    Vehicles = vehicles
                }
            };

            var congestions = new List<Congestion>()
            {
                new Congestion
                {
                    Id = 1,
                    Type = "WeekDay",
                    Coefficient = 1,
                    CityId = 1,
                    Periods = periods
                }
            };

            var cities = new List<City>()
            {
                new City
                {
                    Id = 1,
                    Name = cityName,
                    Coefficient = 1,
                    Congestions = congestions,
                    LowEmissions = new(),
                    UltraLowEmissions= new()
                }
            };

            await _context.Cities.AddRangeAsync(cities);
            await _context.SaveChangesAsync();
        }
    }
}
