using DrivingChargesApi.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DrivingChargesApi.CongestionCharges
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongestionController : ControllerBase
    {
        private readonly CongestionRepository _congestionRepository;
        private readonly IValidator<CongestionQuery> _congestionValidator;

        public CongestionController(CongestionRepository congestionRepository, IValidator<CongestionQuery> congestionValidator)
        {
            _congestionRepository = congestionRepository;
            _congestionValidator = congestionValidator;
        }

        // GET: api/<CongestionController>
        [HttpGet]
        public async Task<IActionResult> GetCongestionResult([FromQuery]CongestionQuery query)
        {
            var congestionValidation = await _congestionValidator.ValidateAsync(query);
            if (congestionValidation.IsValid is false)
            {
                return BadRequest(congestionValidation.Errors?.Select(error => new ValidationResult()
                {
                    Code = error.ErrorCode,
                    PropertyName = error.PropertyName,
                    Message = error.ErrorMessage
                }));
            }

            var taxation = new CongestionTaxation(
                new CongestionIntervals(_congestionRepository, query),
                query);
            var congestionCharge = await taxation.GetCongestionCharge();

            return Ok(congestionCharge);
        }
    }
}
