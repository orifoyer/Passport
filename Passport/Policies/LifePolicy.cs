using Passport.Interfaces;
using Passport.Models;
using System;

namespace Passport.Policies
{
    public class LifePolicy : IPolicy
    {
        private readonly ILogger _logger;

        public LifePolicy(ILogger logger)
        {
            _logger = logger;
        }

        public decimal CalculateRate(PolicyModel model)
        {
            if (!model.DateOfBirth.HasValue)
            {
                _logger.Log("Life policy must include Date of Birth.");
                return 0;
            }
            if (model.DateOfBirth.Value < DateTime.Today.AddYears(-100))
            {
                _logger.Log("Max eligible age for coverage is 100 years.");
                return 0;
            }
            if (!model.Amount.HasValue || model.Amount.Value <= 0)
            {
                _logger.Log("Life policy must include an Amount greater than 0.");
                return 0;
            }

            int age = DateTime.Today.Year - model.DateOfBirth.Value.Year;
            if (model.DateOfBirth.Value > DateTime.Today.AddYears(-age)) age--;

            decimal baseRate = model.Amount.Value * age / 200;

            // Default to non-smoker if IsSmoker is null
            bool isSmoker = model.IsSmoker.HasValue && model.IsSmoker.Value;
            return isSmoker ? baseRate * 2 : baseRate;
        }
    }
}
