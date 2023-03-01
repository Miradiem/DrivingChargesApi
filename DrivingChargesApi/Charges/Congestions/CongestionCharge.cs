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

        public CongestionResult GetCongestionCharge(
            string cityName, string vehicle, DateTime entered, DateTime left)
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

        private List<CongestionChargedTime> ChargedTime(
            string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var chargedTime = new List<CongestionChargedTime>();
            var congestionTypes = _congestionRepository.CongestionTypes(cityName);
            var periodData = _congestionRepository.PeriodData(cityName);
            
            foreach (var type in congestionTypes)
            {
                if (type == "WeekDay")
                {
                    var weekDayData = WeekDay(cityName, vehicle, entered, left, periodData[type], type);
                    chargedTime.Concat(weekDayData);
                }
                if (type == "WeekEnd")
                {
                    var weekEndData = WeekEnd(cityName, vehicle, entered, left, periodData[type], type);
                    chargedTime.Concat(weekEndData);
                }
            }
  
            return chargedTime;
        }

        private List<CongestionChargedTime> WeekDay(
            string cityName, string vehicle, DateTime entered, DateTime left,
            List<Period> periods, string congestionType)
        {
            var chargedTime = new List<CongestionChargedTime>();

            foreach (var period in periods)
            {
                var timeSpentWeekDay = _congestionTimeData.TimeSpentWeekDay(entered, left, period.Start, period.End);
                var tariff = _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicle);
                var chargeAmount = timeSpentWeekDay.TotalHours * tariff;

                chargedTime.Add(new CongestionChargedTime()
                {
                    CongestionType = congestionType,
                    PeriodType = period.Type,
                    TimeSpent = timeSpentWeekDay,
                    ChargeAmount = chargeAmount
                });
            }

            return chargedTime;
        }

        private List<CongestionChargedTime> WeekEnd(
            string cityName, string vehicle, DateTime entered, DateTime left,
            List<Period> periods, string congestionType)
        {
            var chargedTime = new List<CongestionChargedTime>();

            foreach (var period in periods)
            {
                var timeSpentWeekEnd = _congestionTimeData.TimeSpentWeekEnd(entered, left, period.Start, period.End);
                var tariff = _congestionRepository.Tariff(cityName, congestionType, period.Id, vehicle);
                var chargeAmount = timeSpentWeekEnd.TotalHours * tariff;

                chargedTime.Add(new CongestionChargedTime()
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
