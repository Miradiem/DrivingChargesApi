using DrivingChargesApi.Data;
using Microsoft.EntityFrameworkCore;
using System;


namespace DrivingChargesApi.Tests.Data
{
    public class TestChargeContext
    {
        public ChargeContext InMemory() =>
            new ChargeContext(
                new DbContextOptionsBuilder<ChargeContext>().
                    UseInMemoryDatabase(Guid.NewGuid().ToString()).
                    Options);
    }
}
