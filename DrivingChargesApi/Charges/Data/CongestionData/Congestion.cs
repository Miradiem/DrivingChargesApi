

namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Congestion
    {
        public int CongestionId { get; set; }

        public double Rate { get; set; }

        public List<Period> Periods { get; set; } = new();
    }
}
