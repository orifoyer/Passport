using Passport.Interfaces;
using Passport.Models;
using System;

namespace Passport.Policies
{
    public class TravelPolicy : IPolicy
    {
        private readonly ILogger _logger;

        public TravelPolicy(ILogger logger)
        {
            _logger = logger;
        }

        public decimal CalculateRate(PolicyModel model)
        {
            // Validate model properties directly
            if (!model.Days.HasValue || model.Days.Value <= 0)
            {
                _logger.Log("Travel policy must specify Days and be greater than 0.");
                return 0;
            }
            if (model.Days.Value > 180)
            {
                _logger.Log("Travel policy cannot be more than 180 Days.");
                return 0;
            }
            if (string.IsNullOrEmpty(model.Country))
            {
                _logger.Log("Travel policy must specify country.");
                return 0;
            }

            decimal rating = model.Days.Value * 2.5m;
            if (model.Country.Equals("Italy", StringComparison.OrdinalIgnoreCase))
            {
                rating *= 3;
            }

            return rating;
        }
    }
}
