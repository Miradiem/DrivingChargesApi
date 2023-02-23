namespace DrivingChargesApi.Charges.Congestions
{
    public class ChargedTimeData
    {
        public string CongestionType { get; set; }

        public TimeSpan PeriodStart { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public double ChargeAmount { get; set; }
        
    }
}
