using DrivingChargesApi.Validation;

namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionTaxation
    {
        private readonly CongestionRepository _congestionRepository;

        public CongestionTaxation(CongestionRepository congestionRepository)
        {
            _congestionRepository = congestionRepository;
        }

        public async Task<CongestionCharge> GetCongestionCharge(string cityName, string vehicleType, DateTime entered, DateTime left)
        {
            var chargePeriods = await ChargePeriods(cityName, vehicleType, entered, left);
            var totalCharge = chargePeriods.Sum(charge => charge.ChargeAmount);

            return new CongestionCharge()
            {
                City = cityName,
                Vehicle = vehicleType,
                Entered = entered,
                Left = left,
                ChargePeriods = chargePeriods,
                TotalCharge = totalCharge
            };
        }

        private async Task<List<CongestionChargePeriods>> ChargePeriods(string cityName, string vehicleType, DateTime entered, DateTime left)
        {
            var congestionTypes = await _congestionRepository.CongestionTypes(cityName);

            var chargedPeriods = new List<CongestionChargePeriods>();
            foreach (var congestionType in congestionTypes)
            {
                if (congestionType == "WeekDay")
                {
                    var weekDays = await WeekDays(cityName, vehicleType, entered, left, congestionType);
                    chargedPeriods.AddRange(weekDays);
                }
                if (congestionType == "WeekEnd")
                {
                    var weekEnds = await WeekEnds(cityName, vehicleType, entered, left, congestionType);
                    chargedPeriods.AddRange(weekEnds);
                }
            };

            return chargedPeriods;
        }

        private async Task<List<CongestionChargePeriods>> WeekDays(
            string cityName, string vehicleType, DateTime entered, DateTime left, string congestionType)
        {
            var periodData = await _congestionRepository.PeriodData(cityName);
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargePeriods>();
            foreach (var period in periods)
            {
                var timeSpentWeekDay = TimeSpentWeekDay(entered, left, period.Start, period.End);
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

        private TimeSpan TimeSpentWeekDay(DateTime entered, DateTime left, TimeSpan periodStart, TimeSpan periodEnd)
        {
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

        private async Task<List<CongestionChargePeriods>> WeekEnds(
            string cityName, string vehicleType, DateTime entered, DateTime left, string congestionType)
        {
            var periodData = await _congestionRepository.PeriodData(cityName);
            var periods = periodData[congestionType];

            var chargedPeriods = new List<CongestionChargePeriods>();
            foreach (var period in periods)
            {
                var timeSpentWeekEnd = TimeSpentWeekEnd(entered, left, period.Start, period.End);
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

        private TimeSpan TimeSpentWeekEnd(DateTime entered, DateTime left, TimeSpan periodStart, TimeSpan periodEnd)
        {
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
