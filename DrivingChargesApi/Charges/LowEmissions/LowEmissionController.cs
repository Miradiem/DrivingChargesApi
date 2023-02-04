﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DrivingChargesApi.Charges.LowEmissions
{
    [Route("api/[controller]")]
    [ApiController]
    public class LowEmissionController : ControllerBase
    {
        // GET: api/<LowEmissionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LowEmissionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LowEmissionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LowEmissionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LowEmissionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}