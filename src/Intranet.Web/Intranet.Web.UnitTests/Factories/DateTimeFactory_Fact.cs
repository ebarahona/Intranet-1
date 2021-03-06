﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Intranet.Web.Models;
using Xunit;
using Intranet.Shared.Factories;

namespace Intranet.Web.UnitTests.Factories
{
    public class DateTimeFactory_Fact
    {
        [Fact]
        public void Return_Should_Be_DateTime()
        {
            // Assign
            var dateTimeFactory = new DateTimeFactory();

            // Act
            var dateTime = dateTimeFactory.DateTime;

            // Assert
            Assert.IsType<DateTime>(dateTime);
        }

        [Fact]
        public void Return_Should_Be_DateTimeOffset()
        {
            // Assign
            var dateTimeFactory = new DateTimeFactory();

            // Act
            var dateTime = dateTimeFactory.DateTimeOffset;

            // Assert
            Assert.IsType<DateTimeOffset>(dateTime);
        }

        [Fact]
        public void Return_Should_Be_DateTimeOffset_Utc()
        {
            // Assign
            var dateTimeFactory = new DateTimeFactory();

            // Act
            var dateTime = dateTimeFactory.DateTimeOffsetUtc;

            // Assert
            Assert.IsType<DateTimeOffset>(dateTime);
        }
    }
}
