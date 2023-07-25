using DrivingChargesApi.CongestionCharges;
using DrivingChargesApi.Data;
using DrivingChargesApi.Data.CongestionData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrivingChargesApi.Tests.Data
{
    public class TestChargeContext
    {
        private readonly ChargeContext _context = new ChargeContext(
                new DbContextOptionsBuilder<ChargeContext>()
                    .UseInMemoryDatabase(databaseName: "ChargesDataBase")
                    .Options);

        public TestChargeContext() =>
           _ = BuildInMemoryData("London");
        
        public ChargeContext InMemoryDB() =>
            _context;
            
        private async Task BuildInMemoryData(string cityName)
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
                    Start = new(07, 00, 00),
                    End = new(12, 00, 00),
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
                    Congestions = congestions
                }
            };

            await _context.Cities.AddRangeAsync(cities);
            await _context.SaveChangesAsync();
        }
    }
}