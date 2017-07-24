using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intranet.API.UnitTests.TestHelpers
{
    public static class ActionResultHelpers
    {
        public static int CountItems(this IActionResult actionResult)
        {
            return GetResponsesAs<object>(actionResult).Count();
        }

        public static IEnumerable<T> GetResponsesAs<T>(this IActionResult actionResult)
            where T : class
        {
            var okObjectResult = actionResult as OkObjectResult;
            return okObjectResult.Value as IEnumerable<T>;
        }

        public static T GetResponseAs<T>(this IActionResult actionResult)
            where T : class
        {
            var okObjectResult = actionResult as OkObjectResult;
            return okObjectResult.Value as T;
        }
    }
}
