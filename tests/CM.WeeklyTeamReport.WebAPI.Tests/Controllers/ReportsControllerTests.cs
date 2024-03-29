﻿using CM.WeeklyTeamReport.Domain;
using CM.WeeklyTeamReport.Domain.Commands;
using CM.WeeklyTeamReport.Domain.Dto;
using CM.WeeklyTeamReport.Domain.Dto.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Managers.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CM.WeeklyTeamReport.WebAPI.Controllers.Tests
{
    public class ReportsControllerTests
    {
        [Fact]
        public async void ShouldReturnAllReports()
        {
            var fixture = new ReportsControllerFixture();
            fixture.WeeklyReportManager
                .Setup(x => x.readAll(1, 1))
                .Returns(async () => {
                    return new List<ReportsDto>() {
                    GetReportDto(1),
                    GetReportDto(2)
                };
                });
            var controller = fixture.GetReportsController();
            var teamMembers = (ICollection<ReportsDto>)((OkObjectResult)await controller.Get(false, 1, 1)).Value;

            teamMembers.Should().NotBeNull();
            teamMembers.Should().HaveCount(2);

            fixture
                .WeeklyReportManager
                .Verify(x => x.readAll(1,1), Times.Once);
        }

        [Fact]
        public async void ShouldReturnAllReportsFormatted()
        {
            var fixture = new ReportsControllerFixture();
            var report1 = GetReportDto(1);
            var report2 = GetReportDto(2);
            fixture.WeeklyReportManager
                .Setup(x => x.readAll(1, 1))
                .Returns(async () => {
                    return new List<ReportsDto>() {
                    report1,
                    report2
                };
                });
            var controller = fixture.GetReportsController();
            var teamMembers = (List<PersonalReportDto>)((OkObjectResult)await controller.Get(true, 1, 1)).Value;

            teamMembers.Should().NotBeNull();
            teamMembers.Should().HaveCount(2);

            fixture
                .WeeklyReportManager
                .Verify(x => x.readAll(1, 1), Times.Once);
        }

        [Fact]
        public async void ShouldReturnSingleReport()
        {
            var fixture = new ReportsControllerFixture();
            fixture.WeeklyReportManager
                .Setup(x => x.read(1,1,56))
                .Returns(async () => { return GetReportDto(56); });
            var controller = fixture.GetReportsController();
            var teamMembers = (ReportsDto)((OkObjectResult)await controller.Get(1,1,56)).Value;

            teamMembers.Should().NotBeNull();

            fixture
                .WeeklyReportManager
                .Verify(x => x.read(1,1,56), Times.Once);
        }

        [Fact]
        public async void ShouldCreateReport()
        {
            var fixture = new ReportsControllerFixture();
            var report = GetReport();
            var reportDto = GetReportDto();
            fixture.WeeklyReportManager
                .Setup(x => x.create(reportDto))
                .Returns(async () => { return report; });
            var controller = fixture.GetReportsController();
            var returnedTM = (WeeklyReport)((CreatedResult)await controller .Post(reportDto, 1, 1)).Value;

            returnedTM.Should().NotBeNull();
            returnedTM.ID.Should().NotBe(0);

            fixture
                .WeeklyReportManager
                .Verify(x => x.create(reportDto), Times.Once);
        }

        [Fact]
        public async void ShouldUpdateReport()
        {
            var fixture = new ReportsControllerFixture();
            var report = GetReport();
            var reportDto = GetReportDto();
            fixture.WeeklyReportManager
                .Setup(x => x.read(1,1,report.ID))
                .Returns(async () => { return reportDto; });
            fixture.WeeklyReportManager
                .Setup(x => x.update(reportDto, reportDto));
            report.HighThisWeek = "High 2";
            var controller = fixture.GetReportsController();
            var actionResult = await controller.Put(reportDto, 1,1, report.ID);
            actionResult.Should().BeOfType<NoContentResult>();

            fixture
                .WeeklyReportManager
                .Verify(x => x.read(1,1,report.ID), Times.Once);
            fixture
                .WeeklyReportManager
                .Verify(x => x.update(reportDto, reportDto), Times.Once);
        }

        [Fact]
        public async void ShouldDeleteReport()
        {
            var fixture = new ReportsControllerFixture();
            var report = GetReport();
            var reportDto = GetReportDto();
            fixture.WeeklyReportManager
                .Setup(x => x.delete(reportDto));
            fixture.WeeklyReportManager
                .Setup(x => x.read(1,1,report.ID))
                .Returns(async () => { return reportDto; });

            var controller = fixture.GetReportsController();
            var actionResult = await controller.Delete(1,1, report.ID);
            actionResult.Should().BeOfType<NoContentResult>();

            fixture
                .WeeklyReportManager
                .Verify(x => x.delete(reportDto), Times.Once);
        }
        [Fact]
        public async void ShouldReturnNotFoundOnReadAll()
        {
            var fixture = new ReportsControllerFixture();
            fixture.WeeklyReportManager
                .Setup(x => x.readAll(1,1))
                .Returns(async () => { return (List<ReportsDto>)null; });
            var controller = fixture.GetReportsController();
            var reports = await controller.Get(false, 1,1);

            reports.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void ShouldReturnNotFoundOnReadAllFormatted()
        {
            var fixture = new ReportsControllerFixture();
            fixture.WeeklyReportManager
                .Setup(x => x.readAll(1, 1))
                .Returns(async () => { return (List<ReportsDto>)null; });
            var controller = fixture.GetReportsController();
            var reports = await controller.Get(true, 1, 1);

            reports.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void ShouldReturnNotFoundOnRead()
        {
            var fixture = new ReportsControllerFixture();
            fixture.WeeklyReportManager
                .Setup(x => x.read(1, 1,1))
                .Returns(async () => { return (ReportsDto)null; });
            var controller = fixture.GetReportsController();
            var report = await controller.Get(1, 1, 1);

            report.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async void ShouldReturnNoContentOnPost()
        {
            var fixture = new ReportsControllerFixture();
            var reportDto = GetReportDto();
            fixture.WeeklyReportManager
                .Setup(x => x.create(reportDto))
                .Returns(async () => { return (WeeklyReport)null; });
            var controller = fixture.GetReportsController();
            var report = await controller.Post(reportDto, 1, 1);

            report.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async void ShouldReturnNotFoundOnPut()
        {
            var fixture = new ReportsControllerFixture();
            var reportDto = GetReportDto();
            fixture.WeeklyReportManager
                .Setup(x => x.read(1, 1,1))
                .Returns(async () => { return (ReportsDto)null; });
            var controller = fixture.GetReportsController();
            var report = await controller.Put(reportDto, 1, 1, 1);

            report.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async void ShouldReturnNotFoundOnDelete()
        {
            var fixture = new ReportsControllerFixture();
            var reportDto = GetReportDto();
            fixture.WeeklyReportManager
                .Setup(x => x.read(1, 1,1))
                .Returns(async () => { return (ReportsDto)null; });
            var controller = fixture.GetReportsController();
            var report = await controller.Delete(1, 1,1);

            report.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData("current", 0)]
        [InlineData("previous", -7)]
        [InlineData("", 0)]
        public async void ShouldGetTeamReports(string week, int dayShift)
        {
            var day = DateTime.Now;
            var fixture = new ReportsControllerFixture();
            var fullReport = GetFullWeeklyReport(1);
            var fullReportList = new List<IFullWeeklyReport>() { fullReport };
            var reportDto = new HistoryReportDto() { };
            var dtoReportList = new List<HistoryReportDto>() { reportDto };
            fixture.WeeklyReportManager
                .Setup(x => x.ReadReportHistory(1, 1, day, day, ""))
                .Returns(async ()=> { return dtoReportList; });
            fixture.DateTimeManager.Setup(x => x.TakeDateTime(dayShift)).Returns(day);
            fixture.DateTimeManager.Setup(x => x.TakeMonday(day)).Returns(day);
            fixture.DateTimeManager.Setup(x => x.TakeSunday(day)).Returns(day);

            var controller = fixture.GetReportsController();
            var report = await controller.GetTeamReports("", week, 1, 1);

            fixture
                .WeeklyReportManager.Verify(x => x.ReadReportHistory(1, 1, day, day, ""), Times.Once);
            fixture
                .DateTimeManager.Verify(x => x.TakeMonday(day), Times.Once);
            fixture
                .DateTimeManager.Verify(x => x.TakeSunday(day), Times.Once);
            fixture
                .DateTimeManager.Verify(x => x.TakeDateTime(dayShift), Times.Once);
        }
        [Fact]
        public async void ShouldReadOldReports()
        {
            var day = DateTime.Now;
            var fixture = new ReportsControllerFixture();
            var averageOldReportDto = new AverageOldReportDto() {FilterName="q",StatusLevel=new int[] { 1 } };
            var overviewReportDto = new OverviewReportDto() { 
                AuthorId=5, 
                FirstName = "a", 
                LastName="z", 
                StatusLevel = new int[] { 1 } 
            };
            var overviewList = new List<OverviewReportDto>() { overviewReportDto };

            fixture.DateTimeManager.Setup(x => x.TakeMonday()).Returns(day);
            fixture.DateTimeManager.Setup(x => x.TakeMonday(-70)).Returns(day);
            fixture.WeeklyReportManager
                .Setup(x => x.ReadAverageOldReports(1, 1, day, day, "", ""))
                .Returns(async () => { return averageOldReportDto; });
            fixture.WeeklyReportManager
                .Setup(x => x.ReadIndividualOldReports(1, 1, day, day, "", ""))
                .Returns(async () => { return overviewList; });

            var controller = fixture.GetReportsController();
            var report = await controller.GetOldReports("", "", 1, 1);

            fixture
                .WeeklyReportManager.Verify(x => x.ReadAverageOldReports(1, 1, day, day, "", ""), Times.Once);
            fixture
                .WeeklyReportManager.Verify(x => x.ReadIndividualOldReports(1, 1, day, day, "", ""), Times.Once);
            fixture
                .DateTimeManager.Verify(x => x.TakeMonday(), Times.Once);
            fixture
                .DateTimeManager.Verify(x => x.TakeMonday(-70), Times.Once);
        }
        [Fact]
        public async void ShouldBeNotFoundOnReadAverageReports()
        {
            var day = DateTime.Now;
            var fixture = new ReportsControllerFixture();
            var overviewReportDto = new OverviewReportDto()
            {
                AuthorId = 5,
                FirstName = "a",
                LastName = "z",
                StatusLevel = new int[] { 1 }
            };
            var overviewList = new List<OverviewReportDto>() { overviewReportDto };

            fixture.DateTimeManager.Setup(x => x.TakeMonday()).Returns(day);
            fixture.DateTimeManager.Setup(x => x.TakeMonday(-70)).Returns(day);
            fixture.WeeklyReportManager
                .Setup(x => x.ReadAverageOldReports(1, 1, day, day, "", ""))
                .Returns(async () => { return null; });
            fixture.WeeklyReportManager
                .Setup(x => x.ReadIndividualOldReports(1, 1, day, day, "", ""))
                .Returns(async () => { return overviewList; });

            var controller = fixture.GetReportsController();
            var report = await controller.GetOldReports("", "", 1, 1);

            report.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async void ShouldBeNotFoundOnIndividualOldReports()
        {
            var day = DateTime.Now;
            var fixture = new ReportsControllerFixture();
            var averageOldReportDto = new AverageOldReportDto() { FilterName = "q", StatusLevel = new int[] { 1 } };

            fixture.DateTimeManager.Setup(x => x.TakeMonday()).Returns(day);
            fixture.DateTimeManager.Setup(x => x.TakeMonday(-70)).Returns(day);
            fixture.WeeklyReportManager
                .Setup(x => x.ReadAverageOldReports(1, 1, day, day, "", ""))
                .Returns(async () => { return averageOldReportDto; });
            fixture.WeeklyReportManager
                .Setup(x => x.ReadIndividualOldReports(1, 1, day, day, "", ""))
                .Returns(async () => { return null; });

            var controller = fixture.GetReportsController();
            var report = await controller.GetOldReports("", "", 1, 1);

            report.Should().BeOfType<NotFoundResult>();
        }

        private WeeklyReport GetReport(int id = 1)
        {
            return new WeeklyReport
            {
                ID = id,
                AuthorId = id,
                MoraleGradeId = id * 3,
                StressGradeId = id * 3 + 1,
                WorkloadGradeId = id * 3 + 2
            };
        }
        private ReportsDto GetReportDto(int id = 1)
        {
            return new ReportsDto
            {
                ID = id,
                AuthorId = id,
                MoraleGradeId = id * 3,
                MoraleGrade = new Grade
                {
                    ID = id * 3,
                    Level = Level.Average,
                    Commentary = "lalala" + id,
                },
                StressGradeId = id * 3 + 1,
                StressGrade = new Grade
                {
                    ID = id * 3 + 1,
                    Level = Level.Average,
                    Commentary = "lalala" + id + 1,
                },
                WorkloadGradeId = id * 3 + 2,
                WorkloadGrade = new Grade
                {
                    ID = id * 3 + 2,
                    Level = Level.Average,
                    Commentary = "lalala" + id + 2,
                },
            };
        }

        private HistoryReportDto GetHistoryReportDto(int id = 1)
        {
            return new HistoryReportDto
            {
                ID = id,
                FirstName = id.ToString(),
                LastName = id.ToString(),
                WeeklyInformation = new List<IWeeklyInformation>(),
                WeeklyNotations = new List<IWeeklyNotations>(),
                AvatarPath = ""
            };
        }

        private IFullWeeklyReport GetFullWeeklyReport(int id =1)
        {
            const string HighThisWeek = "My high this week";
            const string LowThisWeek = "My low this week";
            var reportDate = new DateTime(2021, 11, 10);
            var anythingElse = "Anything else";
            var commentary = "nope";

            return new FullWeeklyReport
            {
                ID = id,
                AuthorId = 1,
                MoraleGradeId = 1,
                StressGradeId = 1,
                WorkloadGradeId = 1,
                MoraleLevel = 1,
                StressLevel = 1,
                WorkloadLevel = 1,
                MoraleCommentary = commentary,
                StressCommentary = commentary,
                WorkloadCommentary = commentary,
                HighThisWeek = HighThisWeek,
                LowThisWeek = LowThisWeek,
                AnythingElse = anythingElse,
                Date = reportDate
            };
        }
        public class ReportsControllerFixture
        {
            public ReportsControllerFixture()
            {
                WeeklyReportManager = new Mock<IWeeklyReportManager>();
                DateTimeManager= new Mock<IDateTimeManager>();
                ReportCommands = new Mock<ReportCommands>();
            }

            public Mock<IWeeklyReportManager> WeeklyReportManager { get; private set; }
            public Mock<IDateTimeManager> DateTimeManager { get; private set; }
            public Mock<ReportCommands> ReportCommands { get; private set; }

            public WeeklyReportController GetReportsController()
            {
                return new WeeklyReportController(WeeklyReportManager.Object, DateTimeManager.Object);
            }
        }
    }
}
