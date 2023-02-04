using DrivingChargesApi.Charges.Data.CongestionData;
using DrivingChargesApi.Charges.Data.LowEmissionData;
using DrivingChargesApi.Charges.Data.UltraLowEmissionData;

namespace DrivingChargesApi.Charges.Data
{
    public class City
    {
        public int CityId { get; set; }

        public string Name { get; set; }

        public double Coefficient { get; set; }

        public List<Congestion> Congestions { get; set; }

        public List<LowEmission> LowEmissions { get; set; }

        public List<UltraLowEmission> UltraLowEmissions { get; set; }
    }
}
