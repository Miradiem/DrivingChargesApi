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

        public async Task<List<string>> CongestionTypes(string cityName) =>
           await _context.Cities
                .Include(city => city.Congestions)
            .Where(city => city.Name == cityName)
                .SelectMany(city => city.Congestions)
                .Select(congestion => congestion.Type)
            .ToListAsync();

        public async Task<Dictionary<string, List<Period>>> PeriodData(string cityName) =>
            await _context.Cities
            .Include(city => city.Congestions)
                .ThenInclude(congestion => congestion.Periods)
            .Where(city => city.Name == cityName)
                .SelectMany(city => city.Congestions)
            .ToDictionaryAsync(congestion => congestion.Type,
                congestion => congestion.Periods.ToList());

        public async Task<double> Tariff(
            string cityName, string congestionType,int periodId, string vehicleType)
        {
            var coefficients = await _context.Cities
                .Where(city => city.Name == cityName)
                    .Select(city => city.Coefficient)
                .ToListAsync();

            coefficients.AddRange(
                await _context.Cities
                    .Include(city => city.Congestions)
                .Where(city => city.Name == cityName)
                    .SelectMany(city => city.Congestions)
                .Where(congestion => congestion.Type == congestionType)
                    .Select(congestion => congestion.Coefficient)
                .ToListAsync());

            coefficients.AddRange(
                await _context.Cities
                    .Include(city => city.Congestions)
                        .ThenInclude(congestion => congestion.Periods)
                .Where(city => city.Name == cityName)
                    .SelectMany(city => city.Congestions)
                .Where(congestion => congestion.Type == congestionType)
                    .SelectMany(congestion => congestion.Periods)
                .Where(period => period.Id == periodId)
                    .Select(period => period.Coefficient)
                .ToListAsync());

            coefficients.AddRange(
                await _context.Cities
                    .Include(city => city.Congestions)
                        .ThenInclude(congestion => congestion.Periods)
                .Where(city => city.Name == cityName)
                    .SelectMany(city => city.Congestions)
                .Where(congestion => congestion.Type == congestionType)
                    .SelectMany(congestion => congestion.Periods)
                .Where(period => period.Id == periodId)
                    .SelectMany(period => period.Vehicles)
                .Where(vehicle => vehicle.Type == vehicleType)
                    .Select(vehicle => vehicle.Rate)
                .ToListAsync());

            return coefficients.DefaultIfEmpty(0).Aggregate((x, y) => x * y);
        }
    }
}