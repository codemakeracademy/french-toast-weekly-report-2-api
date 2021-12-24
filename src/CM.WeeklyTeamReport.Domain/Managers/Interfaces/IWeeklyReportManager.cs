using CM.WeeklyTeamReport.Domain.Dto;
using CM.WeeklyTeamReport.Domain.Dto.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Repositories.Interfaces
{
    public interface IWeeklyReportManager
    {
        public IWeeklyReport Create(ReportsDto newWeeklyReport);
        public ReportsDto Read(int companyId, int teamMemberId, int WeeklyReportId);
        public void Update(ReportsDto oldEntity, ReportsDto newEntity);
        public void Delete(ReportsDto reportsDto);
        public ICollection<ReportsDto> ReadAll(int companyId, int teamMemberId);
        public ICollection<ReportsDto> ReadReportsInInterval(int companyId, int teamMemberId, DateTime start, DateTime end);
        public ICollection<OverviewReportDto> ReadOldExtendedReports(int companyId, int teamMemberId, DateTime end);
    }
}
