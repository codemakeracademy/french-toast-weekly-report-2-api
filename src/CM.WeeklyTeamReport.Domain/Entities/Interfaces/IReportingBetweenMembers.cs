﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Entities.Interfaces
{
    public interface IReportingBetweenMembers
    {
        public int ReportingTeamMemberID { get; set; }
        public int LeaderID { get; set; }

    }
}
