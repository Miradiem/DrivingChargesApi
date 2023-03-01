using DrivingChargesApi.Charges.Data;
using DrivingChargesApi.Charges.Data.CongestionData;
using Microsoft.EntityFrameworkCore;

namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionRepository
    {
        private readonly ChargeContext _context;

        public CongestionRepository(ChargeContext context)
        {
            _context = context;
        }
        public Dictionary<string, List<Period>> PeriodData(string cityName) =>
            _context.Cities
            .Include(city => city.Congestions)
                .ThenInclude(congestion => congestion.Periods)
            .Where(city => city.Name == cityName)
            .SelectMany(city => city.Congestions)
            .ToDictionary(congestion => congestion.Type,
                congestion => congestion.Periods.ToList());

        public double Tariff(
            string cityName, string congestionType,int periodId, string vehicleType) =>
            _context.Cities
            .Where(city => city.Name == cityName)
            .Select(city => city.Coefficient)
            .Concat(_context.Congestions
                .Where(congestion => congestion.Type == congestionType)
                .Select(congestion => congestion.Coefficient))
            .Concat(_context.Periods
                .Where(period => period.Id == periodId)
                .Select(period => period.Coefficient))
            .Concat(_context.Vehicles
                .Where(vehicle => vehicle.Type == vehicleType)
                .Select(vehicle => vehicle.Rate))
            .DefaultIfEmpty(0)
            .Aggregate((x, y) => x * y);

        public List<string> CongestionTypes(string cityName) =>
            _context.Cities
            .Include(city => city.Congestions)
            .Where(city => city.Name == cityName)
            .SelectMany(city => city.Congestions)
            .Select(congestion => congestion.Type)
            .ToList();    
    }
}