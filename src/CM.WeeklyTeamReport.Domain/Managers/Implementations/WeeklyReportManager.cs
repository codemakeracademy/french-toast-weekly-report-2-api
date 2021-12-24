using CM.WeeklyTeamReport.Domain.Commands;
using CM.WeeklyTeamReport.Domain.Dto;
using CM.WeeklyTeamReport.Domain.Dto.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM.WeeklyTeamReport.Domain.Repositories.Managers
{
    public class WeeklyReportManager : IWeeklyReportManager
    {
        private readonly IWeeklyReportRepository _repository;
        private readonly IReportCommands _reportCommands;

        public WeeklyReportManager(IWeeklyReportRepository weeklyReportRepository, IReportCommands reportCommands)
        {
            _repository = weeklyReportRepository;
            _reportCommands = reportCommands;
        }
        public IWeeklyReport Create(ReportsDto newWeeklyReport)
        {
            var newReport = _reportCommands.DtoToReport(newWeeklyReport);
            return _repository.Create(newReport);
        }

        public void Delete(ReportsDto reportDto)
        {
            var report = _reportCommands.DtoToReport(reportDto);
            _repository.Delete(report);
        }

        public ICollection<ReportsDto> ReadAll(int companyId, int teamMemberId)
        {
            var reports = _repository.ReadAll(companyId, teamMemberId);
            if (reports.Count == 0)
            {
                return null;
            }
            var reportsDto = reports.Select(el => _reportCommands.FullReportToDto(el)).ToList();

            return reportsDto;
        }

        public ReportsDto Read(int companyId, int teamMemberId, int reportId)
        {
            var report = _repository.Read(companyId, teamMemberId, reportId);
            if (report == null)
            {
                return null;
            }
            var reportDto = _reportCommands.FullReportToDto(report);

            return reportDto;
        }

        public void Update(ReportsDto oldEntity, ReportsDto newEntity)
        {
            newEntity.ID = oldEntity.ID;
            var report = _reportCommands.DtoToReport(newEntity);
            _repository.Update(report);
        }

        public ICollection<ReportsDto> ReadReportsInInterval(int companyId, int teamMemberId, DateTime start, DateTime end)
        {
            var firstDate = start;
            var lastDate = end;
            if (start > end)
            {
                firstDate = end;
                lastDate = start;
            }
            var reports = _repository.ReadReportsInInterval(companyId, teamMemberId, firstDate, lastDate);
            if (reports.Count == 0)
            {
                return null;
            }
            var reportsDto = reports.Select(el => _reportCommands.FullReportToDto(el)).ToList();

            return reportsDto;
        }

        public ICollection<OverviewReportDto> ReadOldExtendedReports(int companyId, int teamMemberId, DateTime end)
        {
            var firstDate = end.AddDays(-70);
            var reports = _repository.ReadReportsInInterval(companyId, teamMemberId, firstDate, end);
            if (reports.Count == 0)
            {
                return null;
            }
            var dict = new Dictionary<int, OverviewReportDto>() { };
            var weekReports = reports.Select(el =>
            {
                if (!dict.ContainsKey(el.AuthorId))
                {
                    dict.Add(el.AuthorId, new OverviewReportDto()
                    {
                        AuthorId = el.AuthorId,
                        FirstName = el.FirstName,
                        LastName = el.LastName,
                    });
                }
                var monday = end.FirstDateInWeek(IWeeklyReport.StartOfWeek);
                int index = (int)((monday - el.Date).TotalDays / 7);
                dict[el.AuthorId].MoraleLevel[index] = el.MoraleLevel;
                dict[el.AuthorId].StressLevel[index] = el.StressLevel;
                dict[el.AuthorId].WorkloadLevel[index] = el.WorkloadLevel;
                return (WeekReportsDto)null;
            }).ToList();

            var result = new List<OverviewReportDto>(dict.Values) { };

            return result;
        }
    }
}
