

namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Congestion
    {
        public int CongestionId { get; set; }

        public double Coefficient { get; set; }

        public int CityId { get; set; }

        public List<Period> Periods { get; set; } = new();
    }
}
