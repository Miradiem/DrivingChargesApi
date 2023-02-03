namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Period
    {
        public int PeriodId { get; set; }

        public int CongestionId { get; set; }

        public int VehicleId { get; set; }

        public string Validity { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public double Rate { get; set; }
    }
}
