using DrivingChargesApi.Charges.Data;
using DrivingChargesApi.Charges.Data.CongestionData;
using Microsoft.EntityFrameworkCore;

namespace DrivingChargesApi.Charges.Congestions
{
    public class CongestionRepository
    {
        private readonly ChargeContext _context;

        public CongestionRepository(ChargeContext context)
        {
            _context = context;
        }

        
        
    }
}
