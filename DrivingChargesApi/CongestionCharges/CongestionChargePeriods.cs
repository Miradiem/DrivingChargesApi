namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionChargePeriods
    {
        public string CongestionType { get; set; }

        public string PeriodType { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public double ChargeAmount { get; set; }
    }
}
