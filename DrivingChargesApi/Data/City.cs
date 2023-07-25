using DrivingChargesApi.Data.CongestionData;
using System.ComponentModel.DataAnnotations;

namespace DrivingChargesApi.Data
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = "";

        public double Coefficient { get; set; }

        public List<Congestion> Congestions { get; set; } = new();
    }
}
