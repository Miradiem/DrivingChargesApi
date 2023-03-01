using Microsoft.AspNetCore.Mvc;

namespace DrivingChargesApi.Charges.Congestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionController : ControllerBase
    {
        // GET: api/<CongestionController>
        [HttpGet]
        public CongestionResult Get()
        {

            return new();
        }
    }
}
