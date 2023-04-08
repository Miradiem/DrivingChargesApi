using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data;
using DrivingChargesApi.Tests.Data;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DrivingChargesApi.Tests.CongestionCharges
{
    public class CongestionTaxationTests
    {
        private readonly ChargeContext _context;
        private readonly CongestionRepository _congestionRepository;

        public CongestionTaxationTests()
        {
            _context = new TestChargeContext().InMemoryDB();
            _congestionRepository = new CongestionRepository(_context);
        }

        [Fact]
        public async Task ShouldGetCongestionCharge()
        {
            string cityName = "London";
            string vehicleType = "Car";
            DateTime entered = new DateTime(2023, 01, 02, 10, 00, 00);
            DateTime left = new DateTime(2023, 01, 02, 11, 00, 00);

            var sut = CreateSut();
            var result = await sut.GetCongestionCharge(cityName, vehicleType, entered, left);

            result.TotalCharge.Should().Be(10);
        }

        private CongestionTaxation CreateSut() =>
            new CongestionTaxation(_congestionRepository);
    }
}
