namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionCharge
    {
        public string City { get; set; } = "";

        public string Vehicle { get; set; } = "";

        public DateTime Entered { get; set; }

        public DateTime Left { get; set; } 

        public List<CongestionChargePeriods> ChargePeriods { get; set; } = new();

        public double TotalCharge { get; set; }
    }
}
