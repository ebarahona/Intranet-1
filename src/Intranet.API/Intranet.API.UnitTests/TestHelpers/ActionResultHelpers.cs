using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Intranet.API.UnitTests.TestHelpers
{
    public static class ActionResultHelpers
    {
        public static int CountItems(this IActionResult actionResult)
        {
            var ValuePropValue = actionResult.GetType().GetProperty("Value").GetValue(actionResult);
            return (int)ValuePropValue.GetType().GetProperty("Count").GetValue(ValuePropValue);
        }
    }
}
