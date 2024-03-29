﻿using CM.WeeklyTeamReport.Domain.Entities.Implementations;
using System;
using System.Collections.Generic;
using Xunit;

namespace CM.WeeklyTeamReport.Domain.Tests
{
    public class TeamLinkTests
    {
        [Fact]
        public void ShouldCreateTeamLink()
        {
            var teamLink = new TeamLink
            {
                ReportingTMId = 1,
                LeaderTMId = 2,
                Id = 3,
                FirstName = "A",
                LastName = "B"
            };
            Assert.Equal(1, teamLink.ReportingTMId);
            Assert.Equal(2, teamLink.LeaderTMId);
            Assert.Equal(3, teamLink.Id);
            Assert.Equal("A", teamLink.FirstName);
            Assert.Equal("B", teamLink.LastName);
        }
    }
}
