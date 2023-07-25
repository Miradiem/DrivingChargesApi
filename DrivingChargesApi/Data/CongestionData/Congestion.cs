using System.ComponentModel.DataAnnotations;

namespace DrivingChargesApi.Data.CongestionData
{
    public class Congestion
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Type { get; set; } = "";

        public double Coefficient { get; set; }

        public int CityId { get; set; }

        public List<Period> Periods { get; set; } = new();
    }
}
