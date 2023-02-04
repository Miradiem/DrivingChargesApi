namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        public string Type { get; set; }

        public double Rate { get; set; }

        public int PeriodId { get; set; }
    }
}
