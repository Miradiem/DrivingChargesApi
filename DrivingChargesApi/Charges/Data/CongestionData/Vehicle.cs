namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        public Dictionary<string, double> VehicleRates { get; set; }
    }
}
