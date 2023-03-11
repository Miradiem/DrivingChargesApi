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
            var chargedPeriods = new List<CongestionChargedPeriods>();
;
            chargedPeriods.AddRange(await WeekDays());
            chargedPeriods.AddRange(await WeekEnds());
                
            return chargedPeriods;
        }

        private async Task<List<CongestionChargedPeriods>> WeekDays()
        {
            var cityName = _congestionQuery.CityName;
            var vehicleType = _congestionQuery.VehicleType;
            var periodData = await _congestionRepository.PeriodData(cityName);
            var congestionType = "WeekDay";
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargedPeriods>();

            foreach (var period in periods)
            {
                var timeSpentWeekDay = TimeSpentWeekDay(period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicleType);
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
            var entered = _congestionQuery.Entered.Date;
            var left = _congestionQuery.Left.Date;

            TimeSpan minutes = new();

            for (DateTime date = entered; date <= left; date = date.AddDays(1))
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

        private async Task<List<CongestionChargedPeriods>> WeekEnds()
        {
            var cityName = _congestionQuery.CityName;
            var vehicleType = _congestionQuery.VehicleType;
            var periodData = await _congestionRepository.PeriodData(cityName);
            var congestionType = "WeekEnd";
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
            var entered = _congestionQuery.Entered.Date;
            var left = _congestionQuery.Left.Date;

            TimeSpan minutes = new();

            for (DateTime date = entered; date <= left; date = date.AddDays(1))
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
