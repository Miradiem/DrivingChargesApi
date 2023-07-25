namespace DrivingChargesApi.Validation
{
    public class CongestionQuery
    {
        public string CityName { get; set; } = "";

        public string VehicleType { get; set; } = "";

        public DateTime Entered { get; set; }

        public DateTime Left { get; set; }
    }
}
