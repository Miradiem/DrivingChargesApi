using DrivingChargesApi.Data.CongestionData;
using DrivingChargesApi.Data.LowEmissionData;
using DrivingChargesApi.Data.UltraLowEmissionData;
using System.ComponentModel.DataAnnotations;

namespace DrivingChargesApi.Data
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        public double Coefficient { get; set; }

        public List<Congestion> Congestions { get; set; } = new();

        public List<LowEmission> LowEmissions { get; set; } = new();

        public List<UltraLowEmission> UltraLowEmissions { get; set; } = new();
    }
}
