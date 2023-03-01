namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionTimeData
    {
        public TimeSpan TimeSpentWeekDay(
            DateTime entered, DateTime left, TimeSpan periodStart, TimeSpan periodEnd)
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

        public TimeSpan TimeSpentWeekEnd(
            DateTime entered, DateTime left, TimeSpan periodStart, TimeSpan periodEnd)
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
