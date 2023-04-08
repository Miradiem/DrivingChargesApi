using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data;
using DrivingChargesApi.Tests.Data;
using DrivingChargesApi.Validation;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DrivingChargesApi.Tests.CongestionCharges
{
    public class CongestionControllerTests
    {
        private readonly ChargeContext _context;
        private readonly CongestionRepository _congestionRepository;
        private readonly CongestionController _congestionController;
        private readonly IValidator<CongestionQuery> _validator;

        public CongestionControllerTests()
        {
            _context = new TestChargeContext().InMemoryDB();
            _congestionRepository = new CongestionRepository(_context);
            _validator = new CongestionValidator();
            _congestionController = new CongestionController(_congestionRepository, _validator);
        }

        [Fact]
        public async Task ShouldGetCongestionCharge()
        {
            var query = new CongestionQuery()
            {
                CityName = "London",
                VehicleType = "Car",
                Entered = new DateTime(2023, 01, 02, 10, 00, 00),
                Left = new DateTime(2023, 01, 02, 11, 00, 00)
            };

            var result = await _congestionController.GetCongestionCharge(query) as ObjectResult;

            result.StatusCode.Should().Be(200);
        }
    }
}
