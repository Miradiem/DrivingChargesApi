using DrivingChargesApi.Validation;

namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionIntervals
    {
        private readonly CongestionRepository _congestionRepository;
        private readonly CongestionQuery _congestionQuery;

        public CongestionIntervals(CongestionRepository congestionRepository, CongestionQuery congestionQuery)
        {
            _congestionRepository = congestionRepository;
            _congestionQuery = congestionQuery;
        }

        public async Task<List<CongestionChargedPeriods>> ChargedPeriods()
        {
            var cityName = _congestionQuery.CityName;
            var vehicleType = _congestionQuery.VehicleType;
            var congestionTypes = await _congestionRepository.CongestionTypes(cityName);

            var chargedPeriods = new List<CongestionChargedPeriods>();
            foreach (var congestionType in congestionTypes)
            {
                if (congestionType == "WeekDay")
                {
                    var weekDays = await WeekDays(cityName, vehicleType, congestionType);
                    chargedPeriods.AddRange(weekDays);
                }
                if (congestionType == "WeekEnd")
                {
                    var weekEnds = await WeekEnds(cityName, vehicleType, congestionType);
                    chargedPeriods.AddRange(weekEnds);
                }
            };

            return chargedPeriods;
        }

        private async Task<List<CongestionChargedPeriods>> WeekDays(
            string cityName, string vehicleType, string congestionType)
        {
            var periodData = await _congestionRepository.PeriodData(cityName);
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargedPeriods>();
            foreach (var period in periods)
            {
                var timeSpentWeekDay = TimeSpentWeekDay(period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(
                    cityName, congestionType, period.Id, vehicleType);
                var chargeAmount = timeSpentWeekDay.TotalHours * tariff;

                chargedPeriods.Add(new CongestionChargedPeriods()
                {
                    CongestionType = congestionType,
                    PeriodType = period.Type,
                    TimeSpent = timeSpentWeekDay,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedPeriods;
        }

        private TimeSpan TimeSpentWeekDay(TimeSpan periodStart, TimeSpan periodEnd)
        {
            var entered = _congestionQuery.Entered;
            var left = _congestionQuery.Left;

            TimeSpan minutes = new();

            for (DateTime date = entered.Date; date <= left.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek <= DayOfWeek.Friday)
                {
                    DateTime startDateTime = date + periodStart;
                    DateTime finishDateTime = date + periodEnd;

                    if (startDateTime < entered)
                        startDateTime = entered;
                    if (finishDateTime > left)
                        finishDateTime = left;

                    minutes += finishDateTime - startDateTime;
                }
            }

            return minutes;
        }

        private async Task<List<CongestionChargedPeriods>> WeekEnds(
            string cityName, string vehicleType, string congestionType)
        {
            var periodData = await _congestionRepository.PeriodData(cityName);
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargedPeriods>();
            foreach (var period in periods)
            {
                var timeSpentWeekEnd = TimeSpentWeekEnd(period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicleType);
                var chargeAmount = timeSpentWeekEnd.TotalHours * tariff;

                chargedPeriods.Add(new CongestionChargedPeriods()
                {
                    CongestionType = congestionType,
                    PeriodType = period.Type,
                    TimeSpent = timeSpentWeekEnd,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedPeriods;
        }

        private TimeSpan TimeSpentWeekEnd(TimeSpan periodStart, TimeSpan periodEnd)
        {
            var entered = _congestionQuery.Entered;
            var left = _congestionQuery.Left;

            TimeSpan minutes = new();

            for (DateTime date = entered.Date; date <= left.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    DateTime startDateTime = date + periodStart;
                    DateTime finishDateTime = date + periodEnd;

                    if (startDateTime < entered)
                        startDateTime = entered;
                    if (finishDateTime > left)
                        finishDateTime = left;

                    minutes += finishDateTime - startDateTime;
                }
            }

            return minutes;
        }
    }
}
