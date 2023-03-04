using Microsoft.AspNetCore.Mvc;

namespace DrivingChargesApi.Charges.Congestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionController : ControllerBase
    {
        private readonly CongestionRepository _congestionRepository;

        public CongestionController(CongestionRepository congestionRepository)
        {
            _congestionRepository = congestionRepository;
        }

        // GET: api/<CongestionController>
        [HttpGet]
        public async Task<IActionResult> GetCongestionResult(string cityName, string vehicle, DateTime entered, DateTime left)
        {
            var result = await new CongestionCharge( _congestionRepository, new CongestionTimeData()).GetCongestionCharge(cityName, vehicle, entered, left);
            return Ok(result);
        }
    }
}
