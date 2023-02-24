using DrivingChargesApi.Charges.Data;
using DrivingChargesApi.Charges.Data.CongestionData;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionRepository
    {
        private readonly ChargeContext _context;

        public CongestionRepository(ChargeContext context)
        {
            _context = context;
        }

       

        public TimeSpan TimeSpentWeekDay(
             DateTime entered, DateTime left,
             TimeSpan periodStart, TimeSpan periodEnd)
        {
            var minutes = 0;
            for (var current = entered; current < left; current = current.AddMinutes(1))
            {
                if (current.TimeOfDay >= periodStart &&
                    current.TimeOfDay < periodEnd)
                {
                    minutes++;
                }
            };

            return TimeSpan.FromMinutes(minutes);
        }

        public TimeSpan TimeSpentWeekEnd(
            DateTime entered, DateTime left,
            TimeSpan periodStart, TimeSpan periodEnd)
        {
            var minutes = 0;
            for (var current = entered; current < left; current = current.AddMinutes(1))
            {
                if (current.TimeOfDay >= periodStart &&
                    current.TimeOfDay < periodEnd)
                {
                    minutes++;
                }
            };

            return TimeSpan.FromMinutes(minutes);
        }

        public double ChargeAmount(string cityName, string congestionType, int periodId, string vehicle) =>
                CityCoefficient(cityName) *
                CongestionCoefficient(congestionType) *
                PeriodCoefficient(periodId) *
                VehicleRate(vehicle);

        //###################
        public IEnumerable<DateTime> WeekEnds(DateTime entered, DateTime left) =>
            Enumerable.Range(0, (int)(left - entered).TotalDays + 1)
            .Select(number => entered.AddDays(number))
            .Where(day => day.DayOfWeek == DayOfWeek.Saturday ||
                          day.DayOfWeek == DayOfWeek.Sunday);

        public IEnumerable<DateTime> WeekDays(DateTime entered, DateTime left) =>
            Enumerable.Range(0, (int)(left - entered).TotalDays + 1)
            .Select(number => entered.AddDays(number))
            .Where(day => day.DayOfWeek != DayOfWeek.Saturday ||
                          day.DayOfWeek != DayOfWeek.Sunday);

        public Dictionary<string, List<Period>> PeriodData(string cityName)
        {
            var periodData = _context.Cities
            .Include(city => city.Congestions)
                .ThenInclude(congestion => congestion.Periods)
            .Where(city => city.Name == cityName)
            .SelectMany(city => city.Congestions)
            .ToDictionary(
                congestion => congestion.Type,
                congestion => congestion.Periods.ToList());

            return periodData;
        }

        public List<ChargedTimeData> WeekDayPeriods(string cityName, string vehicle, DateTime entered, DateTime left, List<Period> periods, string congestionType)
        {
            var chargedTime = new List<ChargedTimeData>();
            
            foreach (var period in periods)
            {
                var timeSpent = TimeSpentWeekDay(entered, left, period.Start, period.End);
                var chargeAmount = ChargeAmount(cityName, congestionType, period.PeriodId, vehicle);

                chargedTime.Add(new ChargedTimeData
                {
                    CongestionType = congestionType,
                    PeriodStart = period.Start,
                    TimeSpent = timeSpent,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedTime;
        }

        public List<ChargedTimeData> WeekDayPeriods(string cityName, string vehicle, DateTime entered, DateTime left, List<Period> periods, string congestionType)
        {
            var chargedTime = new List<ChargedTimeData>();

            foreach (var period in periods)
            {
                var timeSpent = TimeSpentWeekDay(entered, left, period.Start, period.End);
                var chargeAmount = ChargeAmount(cityName, congestionType, period.PeriodId, vehicle);

                chargedTime.Add(new ChargedTimeData
                {
                    CongestionType = congestionType,
                    PeriodStart = period.Start,
                    TimeSpent = timeSpent,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedTime;
        }

        public List<ChargedTimeData> ChargedTime(string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var chargedTime = new List<ChargedTimeData>();
            var periodData = PeriodData(cityName);
           
            if (WeekDays(entered, left).Any())
            {
                var congestionType = "WeekDay";
                var weekDayPeriods = WeekDayPeriods(cityName, vehicle, entered, left, periodData["WeekDay"], congestionType);
                chargedTime = Enumerable.Concat(chargedTime, weekDayPeriods).ToList();
            }

            if (WeekDays(entered, left).Any())
            {
                var congestionType = "WeekDay";
                var weekDayPeriods = WeekDayPeriods(cityName, vehicle, entered, left, periodData["WeekDay"], congestionType);
                chargedTime = Enumerable.Concat(chargedTime, weekDayPeriods).ToList();
            }

            return chargedTime;
        }
            
            
      



        public CongestionResult GetCongestionCharge(string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var chargedTime = ChargedTime(cityName, vehicle, entered, left);

            double totalCharge = 0;
            foreach (var charge in chargedTime)
            {
                totalCharge += charge.ChargeAmount;
            }

            return new CongestionResult()
            {
                City = cityName,
                Vehicle = vehicle,
                Entered = entered,
                Left = left,
                ChargedTimeData = chargedTime,
                TotalCharge = totalCharge
            };
        }


        //###################

        
        //###################

        
        public List<string> Cities() =>
           _context.Cities
           .Select(city => city.Name)
           .ToList();

        public int CityId(string cityName) =>
            _context.Cities
            .Where(city => city.Name == cityName)
            .Select(city => city.CityId)
            .FirstOrDefault();

        public double CityCoefficient(string cityName) =>
            _context.Cities
            .Where(city => city.Name == cityName)
            .Select(city => city.Coefficient)
            .FirstOrDefault();

        
        //##################
        public List<Congestion> Congestions(int cityId) =>
            _context.Cities
            .Where(city => city.CityId == cityId)
            .SelectMany(city => city.Congestions)
            .ToList();


        public List<string> CongestionTypes(int cityId) =>
            _context.Congestions
            .Where(congestion => congestion.CityId == cityId)
            .Select(congestion => congestion.Type)
            .ToList();

        public int CongestionId(string congestionType, int cityId) =>
            _context.Congestions
            .Where(congestion => congestion.CityId == cityId &&
                                 congestion.Type == congestionType)
            .Select(congestion => congestion.CongestionId)
            .FirstOrDefault();

        public double CongestionCoefficient(string congestionType) =>
            _context.Congestions
            .Where(congestion => congestion.Type == congestionType)
            .Select(congestion => congestion.Coefficient)
            .FirstOrDefault();
        //###################

        public List<TimeSpan> PeriodStartTimes(int congestionId) =>
            _context.Periods
            .Where(period => period.CongestionId == congestionId)
            .Select(period => period.Start)
            .ToList();

        public List<Period> Periods(int congestionId) =>
            _context.Congestions
            .Where(congestion => congestion.CongestionId == congestionId)
            .SelectMany(congestion => congestion.Periods)
            .ToList();

        public double PeriodCoefficient(int periodId) =>
            _context.Periods
            .Where(period => period.PeriodId == periodId)
            .Select(period => period.Coefficient)
            .FirstOrDefault();
        //#####################

        public List<string> VehicleTypes(int periodId) =>
            _context.Vehicles
            .Where(vehicle => vehicle.PeriodId == periodId)
            .Select(vehicle => vehicle.Type)
            .ToList();

        public int VehicleId(int periodId, string vehicleType) =>
            _context.Vehicles
            .Where(vehicle => vehicle.PeriodId == periodId &&
                              vehicle.Type == vehicleType)
            .Select(vehicle => vehicle.VehicleId)
            .FirstOrDefault();

        public double VehicleRate(string vehicleType) =>
            _context.Vehicles
            .Where(vehicle => vehicle.Type == vehicleType)
            .Select(vehicle => vehicle.Rate)
            .FirstOrDefault();

        //#####################
        public List<ChargedTimeData> WeekDayData(string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var weekDayData = new List<ChargedTimeData>();
            var weekDayPeriods = Periods(cityName, "WeekDay");

            foreach (var period in weekDayPeriods)
            {
                weekDayData.Add(new ChargedTimeData()
                {
                    CongestionType = "WeekDay",
                    PeriodStart = period.Start,
                    TimeSpent = TimeSpentWeekDay(entered, left, period.Start, period.End),
                    ChargeAmount = ChargeAmount(cityName, "WeekDay", period.PeriodId, vehicle)
                });
            }

            return weekDayData;
        }

        public List<ChargedTimeData> WeekEndData(string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var weekEndData = new List<ChargedTimeData>();
            var weekEndPeriods = Periods(cityName, "WeekEnd");

            foreach (var period in weekEndPeriods)
            {
                weekEndData.Add(new ChargedTimeData()
                {
                    CongestionType = "WeekEnd",
                    PeriodStart = period.Start,
                    TimeSpent = TimeSpentWeekEnd(entered, left, period.Start, period.End),
                    ChargeAmount = ChargeAmount(cityName, "WeekEnd", period.PeriodId, vehicle)
                });
            }

            return weekEndData;
        }

    }
}