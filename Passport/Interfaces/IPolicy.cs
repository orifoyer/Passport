using Passport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passport.Interfaces
{
    public interface IPolicy
    {
        decimal CalculateRate(PolicyModel model);
    }

}
