using DrivingChargesApi.Validation;

namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionTaxation
    {
        private readonly CongestionRepository _congestionRepository;
        private readonly string _cityName;
        private readonly string _vehicleType;
        private readonly DateTime _entered;
        private readonly DateTime _left;

        public CongestionTaxation(CongestionRepository congestionRepository, string cityName, string vehicleType, DateTime entered, DateTime left)
        {
            _congestionRepository = congestionRepository;
            _cityName = cityName;
            _vehicleType = vehicleType;
            _entered = entered;
            _left = left;
        }

        public async Task<CongestionCharge> GetCongestionCharge()
        {
            var chargePeriods = await ChargePeriods();
            var totalCharge = chargePeriods.Sum(charge => charge.ChargeAmount);

            return new CongestionCharge()
            {
                City = _cityName,
                Vehicle = _vehicleType,
                Entered = _entered,
                Left = _left,
                ChargePeriods = chargePeriods,
                TotalCharge = totalCharge
            };
        }

        private async Task<List<CongestionChargePeriods>> ChargePeriods()
        {
            var congestionTypes = await _congestionRepository.CongestionTypes(_cityName);

            var chargedPeriods = new List<CongestionChargePeriods>();
            foreach (var congestionType in congestionTypes)
            {
                if (congestionType == "WeekDay")
                {
                    var weekDays = await WeekDays(_cityName, _vehicleType, congestionType);
                    chargedPeriods.AddRange(weekDays);
                }
                if (congestionType == "WeekEnd")
                {
                    var weekEnds = await WeekEnds(_cityName, _vehicleType, congestionType);
                    chargedPeriods.AddRange(weekEnds);
                }
            };

            return chargedPeriods;
        }

        private async Task<List<CongestionChargePeriods>> WeekDays(
            string cityName, string vehicleType, string congestionType)
        {
            var periodData = await _congestionRepository.PeriodData(cityName);
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargePeriods>();
            foreach (var period in periods)
            {
                var timeSpentWeekDay = TimeSpentWeekDay(period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(
                    cityName, congestionType, period.Id, vehicleType);
                var chargeAmount = timeSpentWeekDay.TotalHours * tariff;

                chargedPeriods.Add(new CongestionChargePeriods()
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
            TimeSpan minutes = new();

            for (DateTime date = _entered.Date; date <= _left.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek <= DayOfWeek.Friday)
                {
                    DateTime startDateTime = date + periodStart;
                    DateTime finishDateTime = date + periodEnd;

                    if (startDateTime < _entered)
                        startDateTime = _entered;
                    if (finishDateTime > _left)
                        finishDateTime = _left;

                    minutes += finishDateTime - startDateTime;
                }
            }

            return minutes;
        }

        private async Task<List<CongestionChargePeriods>> WeekEnds(
            string cityName, string vehicleType, string congestionType)
        {
            var periodData = await _congestionRepository.PeriodData(cityName);
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargePeriods>();
            foreach (var period in periods)
            {
                var timeSpentWeekEnd = TimeSpentWeekEnd(period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicleType);
                var chargeAmount = timeSpentWeekEnd.TotalHours * tariff;

                chargedPeriods.Add(new CongestionChargePeriods()
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
            TimeSpan minutes = new();

            for (DateTime date = _entered.Date; date <= _left.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    DateTime startDateTime = date + periodStart;
                    DateTime finishDateTime = date + periodEnd;

                    if (startDateTime < _entered)
                        startDateTime = _entered;
                    if (finishDateTime > _left)
                        finishDateTime = _left;

                    minutes += finishDateTime - startDateTime;
                }
            }

            return minutes;
        }
    }
}
