using DrivingChargesApi.Validation;

namespace DrivingChargesApi.CongestionCharges
{
    public class CongestionTaxation
    {
        private readonly CongestionIntervals _congestionIntervals;
        private readonly CongestionQuery _congestionQuery;

        public CongestionTaxation(CongestionIntervals congestionIntervals, CongestionQuery congestionQuery)
        {
            _congestionIntervals = congestionIntervals;
            _congestionQuery = congestionQuery;
        }

        public async Task<CongestionCharge> GetCongestionCharge()
        {
            var chargedPeriods = await _congestionIntervals.ChargedPeriods();
            var totalCharge = chargedPeriods.Sum(charge => charge.ChargeAmount);

            return new CongestionCharge()
            {
                City = _congestionQuery.CityName,
                Vehicle = _congestionQuery.VehicleType,
                Entered = _congestionQuery.Entered,
                Left = _congestionQuery.Left,
                ChargedPeriods = chargedPeriods,
                TotalCharge = totalCharge
            };
        }

        
    }
}