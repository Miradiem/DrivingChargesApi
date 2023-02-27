namespace DrivingChargesApi.Charges.TimeData
{
    public class TimeRange
    {
        public IEnumerable<DateTime> WeekDays(DateTime entered, DateTime left) =>
          Enumerable.Range(0, (int)(left - entered).TotalDays + 1)
          .Select(number => entered.AddDays(number))
          .Where(day => day.DayOfWeek != DayOfWeek.Saturday ||
                        day.DayOfWeek != DayOfWeek.Sunday);

        public IEnumerable<DateTime> WeekEnds(DateTime entered, DateTime left) =>
            Enumerable.Range(0, (int)(left - entered).TotalDays + 1)
            .Select(number => entered.AddDays(number))
            .Where(day => day.DayOfWeek == DayOfWeek.Saturday ||
                          day.DayOfWeek == DayOfWeek.Sunday);
    }
}
