using DrivingChargesApi.Charges.Data.CongestionData;
using DrivingChargesApi.Charges.TimeData;
using System.Linq;

namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionCharge
    {
        private readonly CongestionRepository _congestionRepository;
        private readonly TimeRange _timeRange;

        public CongestionCharge(CongestionRepository congestionRepository, TimeRange timeRange)
        {
            _congestionRepository = congestionRepository;
            _timeRange = timeRange;
        }

        public CongestionResult GetCongestionCharge(
            string cityName,
            string vehicle,
            DateTime entered,
            DateTime left)
        {
            var chargedTime = ChargedTime(cityName, vehicle, entered, left);
            var totalCharge = chargedTime.Sum(charge => charge.ChargeAmount);

            return new CongestionResult()
            {
                City = cityName,
                Vehicle = vehicle,
                Entered = entered,
                Left = left,
                ChargedTime = chargedTime,
                TotalCharge = totalCharge
            };
        }

        private List<ChargedTime> ChargedTime(
            string cityName,
            string vehicle,
            DateTime entered,
            DateTime left)
        {
            var chargedTime = new List<ChargedTime>();
            var periodData = _congestionRepository.PeriodData(cityName);

            if (_timeRange.WeekDays(entered, left).Any())
            {
                var congestionType = "WeekDay";
                var weekDayPeriods = WeekDayData(cityName, vehicle, entered, left,
                    periodData[congestionType], congestionType);

                chargedTime = Enumerable.Concat(chargedTime, weekDayPeriods).ToList();
            }

            if (_timeRange.WeekEnds(entered, left).Any())
            {
                var congestionType = "WeekEnd";
                var weekDayPeriods = WeekEndData(cityName, vehicle, entered, left,
                    periodData[congestionType], congestionType);

                chargedTime = Enumerable.Concat(chargedTime, weekDayPeriods).ToList();
            }

            return chargedTime;
        }

        private List<ChargedTime> WeekDayData(
            string cityName,
            string vehicle,
            DateTime entered,
            DateTime left,
            List<Period> periods,
            string congestionType) =>
                (from period in periods
                 let timeSpentWeekDay = TimeSpentWeekDay(entered, left, period.Start, period.End)
                 let tariff = _congestionRepository.Tariff(cityName, congestionType, period.PeriodId, vehicle)
                 let chargeAmount = timeSpentWeekDay.TotalHours * tariff
                 select new ChargedTime()
                 {
                     CongestionType = congestionType,
                     PeriodType = period.Type,
                     TimeSpent = timeSpentWeekDay,
                     ChargeAmount = chargeAmount
                 }).ToList();
        

        private List<ChargedTime> WeekEndData(
            string cityName,
            string vehicle,
            DateTime entered,
            DateTime left,
            List<Period> periods,
            string congestionType) => 
                (from period in periods
                 let timeSpentWeekEnd = TimeSpentWeekEnd(entered, left, period.Start, period.End)
                 let tariff = _congestionRepository.Tariff(cityName, congestionType, period.PeriodId, vehicle)
                 let chargeAmount = timeSpentWeekEnd.TotalHours * tariff
                 select new ChargedTime()
                 {
                     CongestionType = congestionType,
                     PeriodType = period.Type,
                     TimeSpent = timeSpentWeekEnd,
                     ChargeAmount = chargeAmount
                 }).ToList();

        private TimeSpan TimeSpentWeekDay(
            DateTime entered,
            DateTime left,
            TimeSpan periodStart,
            TimeSpan periodEnd)
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

        private TimeSpan TimeSpentWeekEnd(
            DateTime entered,
            DateTime left,
            TimeSpan periodStart,
            TimeSpan periodEnd)
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
