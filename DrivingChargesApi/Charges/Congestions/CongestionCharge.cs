using DrivingChargesApi.Charges.Data.CongestionData;

namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionCharge
    {
        private readonly CongestionRepository _congestionRepository;
        private readonly CongestionTimeData _congestionTimeData;

        public CongestionCharge(CongestionRepository congestionRepository, CongestionTimeData congestionTimeData)
        {
            _congestionRepository = congestionRepository;
            _congestionTimeData = congestionTimeData;
        }

        public async Task<CongestionResult> GetCongestionCharge(
            string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var chargedTime = await ChargedTime(cityName, vehicle, entered, left);
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

        private async Task<List<CongestionChargedPeriods>> ChargedTime(
            string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var chargedTime = new List<CongestionChargedPeriods>();
            var congestionTypes = await _congestionRepository.CongestionTypes(cityName);
            var periodData = await _congestionRepository.PeriodData(cityName);
            
            foreach (var type in congestionTypes)
            {
                if (type == "WeekDay")
                {
                    var weekDayData = await WeekDays(cityName, vehicle, entered, left, periodData[type], type);
                    chargedTime.AddRange(weekDayData);
                }
                if (type == "WeekEnd")
                {
                    var weekEndData = await WeekEnds(cityName, vehicle, entered, left, periodData[type], type);
                    chargedTime.AddRange(weekEndData);
                }
            }
  
            return chargedTime;
        }

        private async Task<List<CongestionChargedPeriods>> WeekDays(
            string cityName, string vehicle, DateTime entered, DateTime left,
            List<Period> periods, string congestionType)
        {
            var chargedTime = new List<CongestionChargedPeriods>();

            foreach (var period in periods)
            {
                var timeSpentWeekDay = _congestionTimeData.TimeSpentWeekDay(entered, left, period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicle);
                var chargeAmount = timeSpentWeekDay.TotalHours * tariff;

                chargedTime.Add(new CongestionChargedPeriods()
                {
                    CongestionType = congestionType,
                    PeriodType = period.Type,
                    TimeSpent = timeSpentWeekDay,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedTime;
        }

        private async Task<List<CongestionChargedPeriods>> WeekEnds(
            string cityName, string vehicle, DateTime entered, DateTime left,
            List<Period> periods, string congestionType)
        {
            var chargedTime = new List<CongestionChargedPeriods>();

            foreach (var period in periods)
            {
                var timeSpentWeekEnd = _congestionTimeData.TimeSpentWeekEnd(entered, left, period.Start, period.End);
                var tariff = await _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicle);
                var chargeAmount = timeSpentWeekEnd.TotalHours * tariff;

                chargedTime.Add(new CongestionChargedPeriods()
                {
                    CongestionType = congestionType,
                    PeriodType = period.Type,
                    TimeSpent = timeSpentWeekEnd,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedTime;
        }
    }
}