using System.ComponentModel.DataAnnotations;

namespace DrivingChargesApi.Charges.Data.CongestionData
{
    public class Vehicle
    {
        public int VehicleId { get; set; }

        [MaxLength(30)]
        public string Type { get; set; }

        public double Rate { get; set; }

        public int PeriodId { get; set; }
    }
}
