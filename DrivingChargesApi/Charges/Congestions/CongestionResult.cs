namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionResult
    {
        public string City { get; set; }

        public string Vehicle { get; set; }

        public DateTime Entered { get; set; }

        public DateTime Left { get; set; } 

        public List<CongestionChargedTime> ChargedTime{ get; set; } = new();

        public double TotalCharge { get; set; }
    }
}
