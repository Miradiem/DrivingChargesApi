namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionCharge
    {
        public string City { get; set; }

        public string Vehicle { get; set; }

        public DateTime Entered { get; set; }

        public DateTime Left { get; set; } 

        public List<CongestionChargedPeriods> ChargedPeriods { get; set; } = new();

        public double TotalCharge { get; set; }
    }
}
