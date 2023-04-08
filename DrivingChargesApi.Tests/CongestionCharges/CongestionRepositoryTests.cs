using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data;
using DrivingChargesApi.Data.CongestionData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DrivingChargesApi.Tests.CongestionCharges
{
    public class CongestionRepositoryTests
    {
        private readonly DbContextOptions<ChargeContext> _options;
        private readonly ChargeContext _context;
        private readonly CongestionRepository _congestionRepository;

        public CongestionRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ChargeContext>()
                .UseInMemoryDatabase(databaseName: "ChargesDataBase")
                .Options;
            _context = new ChargeContext(_options);
            _congestionRepository = new CongestionRepository(_context);
            _ = CreateTestData("London");
        }

        [Fact]
        public async Task ShouldGetCongestionTypes()
        {
            var cityName = "London";
            var congestionType = "WeekDay";

            var result = await _congestionRepository.CongestionTypes(cityName);

            result.Should().Contain(congestionType);
        }

        [Fact]
        public async Task ShouldGetPeriodData()
        {
            var cityName = "London";
            var congestionType = "WeekDay";
            var periodType = "Am";

            var result = await _congestionRepository.PeriodData(cityName);

            result[congestionType].Should().Contain(property => property.Type == periodType);
        }

        [Fact]
        public async Task ShouldGetTariff()
        {
            var cityName = "London";
            var congestionType = "WeekDay";
            var periodId = 1;
            var vehicleType = "Car";

            var result = await _congestionRepository.Tariff(
                cityName, congestionType, periodId, vehicleType);

            result.Should().Be(10);
        }

        private async Task CreateTestData(string cityName)
        {
            var vehicles = new List<Vehicle>()
            {
                new Vehicle
                {
                    Id = 1,
                    Type = "Car",
                    Rate = 10,
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
