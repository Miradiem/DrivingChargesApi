namespace DrivingChargesApi.Charges.Congestions
{
    public class ChargedTime
    {
        public string CongestionType { get; set; }

        public string PeriodType { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public double ChargeAmount { get; set; }
    }
}
