using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data;
using DrivingChargesApi.Tests.Data;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace DrivingChargesApi.Tests.CongestionCharges
{
    public class CongestionRepositoryTests
    {
        private readonly ChargeContext _context;
        private readonly CongestionRepository _congestionRepository;

        public CongestionRepositoryTests()
        {
            _context = new TestChargeContext().InMemoryDB();
            _congestionRepository = new CongestionRepository(_context);
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

            var result = await _congestionRepository.Tariff(cityName, congestionType, periodId, vehicleType);

            result.Should().Be(10);
        }
    }
}
