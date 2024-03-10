using Passport.Interfaces;
using Passport.Models;
using System;

namespace Passport.Policies
{
    public class HealthPolicy : IPolicy
    {
        private readonly ILogger _logger;

        public HealthPolicy(ILogger logger)
        {
            _logger = logger;
        }

        public decimal CalculateRate(PolicyModel model)
        {
            if (string.IsNullOrEmpty(model.Gender))
            {
                _logger.Log("Health policy must specify Gender.");
                return 0;
            }
            if (!model.Deductible.HasValue)
            {
                _logger.Log("Health policy must specify Deductible.");
                return 0;
            }

            string genderLower = model.Gender.ToLower();
            decimal deductible = model.Deductible.Value;

            if (genderLower == "male" && deductible < 500)
            {
                return 1000m;
            }
            if (genderLower != "male" && deductible < 800)
            {
                return 1100m;
            }
            return genderLower == "male" ? 900m : 1000m;
        }
    }
}
