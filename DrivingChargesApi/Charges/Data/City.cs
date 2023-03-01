using DrivingChargesApi.Charges.Data.CongestionData;
using DrivingChargesApi.Charges.Data.LowEmissionData;
using DrivingChargesApi.Charges.Data.UltraLowEmissionData;
using System.ComponentModel.DataAnnotations;

namespace DrivingChargesApi.Charges.Data
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
