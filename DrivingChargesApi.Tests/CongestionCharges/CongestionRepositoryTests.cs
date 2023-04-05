using DrivingChargesApi.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DrivingChargesApi.Data;
using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data.CongestionData;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace DrivingChargesApi.Tests.CongestionCharges
{
    public class CongestionRepositoryTests
    {
        private readonly Mock<ChargeContext> _chargeContextMock;
        private readonly CongestionRepository _congestionRepository;

        public CongestionRepositoryTests()
        {
            _chargeContextMock = new Mock<ChargeContext>();
            _congestionRepository = new CongestionRepository(_chargeContextMock.Object);
        }

        [Fact]
        public async void ShouldGetCongestionTypes()
        {
            var congestions = new List<Congestion>()
            {
                new Congestion
                {
                    Id = 1,
                    Type = "WeekDay",
                    Coefficient = 1,
                    CityId = 1,
                    Periods = new()
                },
                new Congestion
                {
                    Id = 2,
                    Type = "WeekEnd",
                    Coefficient = 1,
                    CityId = 1,
                    Periods = new()
                }
            };

            _chargeContextMock.Setup(m => m.Congestions.ToList()).Returns(congestions);

            var cityName = "London";
            var result = await _congestionRepository.CongestionTypes(cityName);

            result.Should().Contain("WeekDay");
            result.Should().Contain("WeekEnd");
        }
    }
}
