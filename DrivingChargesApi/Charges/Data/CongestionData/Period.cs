namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Period
    {
        public int PeriodId { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public double Coefficient { get; set; }

        public int CongestionId { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new();
    }
}
