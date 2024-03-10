using Newtonsoft.Json;
using Passport.Constants;
using Passport.Interfaces;
using Passport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passport.Services
{
    public class RatingEngine
    {
        public decimal Rating { get; private set; }
        private readonly ILogger _logger;

        public RatingEngine(ILogger logger)
        {
            _logger = logger;
        }

        public void Rate()
        {
            _logger.Log("Starting rate.");
            _logger.Log("Loading policy.");

            string policyJson = File.ReadAllText("policy.json");
            var settings = new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyyy",
                Culture = System.Globalization.CultureInfo.InvariantCulture
            };
            var policyData = JsonConvert.DeserializeObject<PolicyModel>(policyJson, settings);

            if (policyData == null)
            {
                _logger.Log("Failed to deserialize policy.");
                return;
            }

            IPolicy policy;

            try
            {
                PolicyType policyType = (PolicyType)Enum.Parse(typeof(PolicyType), policyData.Type, true);

                policy = CreatePolicy(policyType, _logger);

                _logger.Log($"Rating {policyData.Type.ToString().ToUpper()} policy...");
                _logger.Log("Validating policy.");

                Rating = policy.CalculateRate(policyData);
                _logger.Log("Rating completed.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Handle the specific exception for unknown policy types
                _logger.Log($"{ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                // Handle any other exception that may occur during the rating process
                _logger.Log("Unknown policy type");
                return;
            }


        }

        private IPolicy CreatePolicy(PolicyType policyType, ILogger logger)
        {
            string policyClassName = $"{Enum.GetName(typeof(PolicyType), policyType)}Policy";
            Type policyClassType = Type.GetType($"Passport.Policies.{policyClassName}");

            if (policyClassType == null)
            {
                throw new ArgumentOutOfRangeException(nameof(policyType), "Unknown policy type");
            }

            return (IPolicy)Activator.CreateInstance(policyClassType, new object[] { logger });
        }

    }

}
