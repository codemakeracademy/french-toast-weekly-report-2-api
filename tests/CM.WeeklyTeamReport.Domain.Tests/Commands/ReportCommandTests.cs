﻿using CM.WeeklyTeamReport.Domain.Commands;
using CM.WeeklyTeamReport.Domain.Dto.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Implementations;
using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using CM.WeeklyTeamReport.Domain.Repositories.Dto;
using System;
using System.Collections.Generic;
using Xunit;

namespace CM.WeeklyTeamReport.Domain.Tests
{
    public class ReportCommandTests
    {
        public ReportCommands commands = new ReportCommands()
;        [Fact]
        public void ShouldCreatReportFromDto()
        {
            var moraleGrade = new Grade { Level = Level.VeryLow };
            var stressGrade = new Grade { Level = Level.Low };
            var workloadGrade = new Grade { Level = Level.Average, Commentary = "Usual amount of stress" };
            const string HighThisWeek = "My high this week";
            const string LowThisWeek = "My low this week";
            var reportDate = new DateTime(2021, 11, 10);
            var expectedWeekStart = new DateTime(2021, 11, 8);
            var expectedWeekEnd = new DateTime(2021, 11, 14);
            var anythingElse = "Anything else";

            var reportDto = new ReportsDto
            {
                ID = 1,
                AuthorId = 1,
                MoraleGradeId = 1,
                StressGradeId = 1,
                WorkloadGradeId = 1,
                MoraleGrade = moraleGrade,
                StressGrade = stressGrade,
                WorkloadGrade = workloadGrade,
                HighThisWeek = HighThisWeek,
                LowThisWeek = LowThisWeek,
                AnythingElse = anythingElse,
                Date = reportDate
            };
            var report = commands.dtoToReport(reportDto);
            Assert.Equal(reportDto.ID, report.ID);
            Assert.Equal(reportDto.AuthorId, report.AuthorId);
            Assert.Equal(reportDto.MoraleGradeId, report.MoraleGradeId);
            Assert.Equal(reportDto.StressGradeId, report.StressGradeId);
            Assert.Equal(reportDto.WorkloadGradeId, report.WorkloadGradeId);
            Assert.Equal(reportDto.MoraleGrade, report.MoraleGrade);
            Assert.Equal(reportDto.StressGrade, report.StressGrade);
            Assert.Equal(reportDto.WorkloadGrade, report.WorkloadGrade);
            Assert.Equal(reportDto.HighThisWeek, report.HighThisWeek);
            Assert.Equal(reportDto.LowThisWeek, report.LowThisWeek);
            Assert.Equal(reportDto.Date, report.Date);
            Assert.Equal(reportDto.AnythingElse, report.AnythingElse);
        }
        [Fact]
        public void ShouldCreateDtoFromReport()
        {
            var moraleGrade = new Grade { Level = Level.VeryLow };
            var stressGrade = new Grade { Level = Level.Low };
            var workloadGrade = new Grade { Level = Level.Average, Commentary = "Usual amount of stress" };
            const string HighThisWeek = "My high this week";
            const string LowThisWeek = "My low this week";
            var reportDate = new DateTime(2021, 11, 10);
            var expectedWeekStart = new DateTime(2021, 11, 8);
            var expectedWeekEnd = new DateTime(2021, 11, 14);
            var anythingElse = "Anything else";

            var report = new WeeklyReport
            {
                ID = 1,
                AuthorId = 1,
                MoraleGradeId = 1,
                StressGradeId = 1,
                WorkloadGradeId = 1,
                MoraleGrade = moraleGrade,
                StressGrade = stressGrade,
                WorkloadGrade = workloadGrade,
                HighThisWeek = HighThisWeek,
                LowThisWeek = LowThisWeek,
                AnythingElse = anythingElse,
                Date = reportDate
            };
            var reportDto = commands.reportToDto(report);
            Assert.Equal(reportDto.ID, report.ID);
            Assert.Equal(reportDto.AuthorId, report.AuthorId);
            Assert.Equal(reportDto.MoraleGradeId, report.MoraleGradeId);
            Assert.Equal(reportDto.StressGradeId, report.StressGradeId);
            Assert.Equal(reportDto.WorkloadGradeId, report.WorkloadGradeId);
            Assert.Equal(reportDto.MoraleGrade, report.MoraleGrade);
            Assert.Equal(reportDto.StressGrade, report.StressGrade);
            Assert.Equal(reportDto.WorkloadGrade, report.WorkloadGrade);
            Assert.Equal(reportDto.HighThisWeek, report.HighThisWeek);
            Assert.Equal(reportDto.LowThisWeek, report.LowThisWeek);
            Assert.Equal(reportDto.Date, report.Date);
            Assert.Equal(reportDto.AnythingElse, report.AnythingElse);
        }
        [Fact]
        public void ShouldCreateDtoFromFullReport()
        {
            const string HighThisWeek = "My high this week";
            const string LowThisWeek = "My low this week";
            var reportDate = new DateTime(2021, 11, 10);
            var anythingElse = "Anything else";
            var commentary = "nope";

            var report = new FullWeeklyReport
            {
                ID = 1,
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
            var reportDto = commands.fullReportToDto(report);
            Assert.Equal(reportDto.ID, report.ID);
            Assert.Equal(reportDto.AuthorId, report.AuthorId);
            Assert.Equal(reportDto.MoraleGradeId, report.MoraleGradeId);
            Assert.Equal(reportDto.StressGradeId, report.StressGradeId);
            Assert.Equal(reportDto.WorkloadGradeId, report.WorkloadGradeId);
            Assert.Equal(reportDto.MoraleGrade.Commentary, report.MoraleCommentary);
            Assert.Equal(reportDto.StressGrade.Commentary, report.StressCommentary);
            Assert.Equal(reportDto.WorkloadGrade.Commentary, report.WorkloadCommentary);
            Assert.Equal(reportDto.MoraleGrade.Level, (Level)report.MoraleLevel);
            Assert.Equal(reportDto.StressGrade.Level, (Level)report.StressLevel);
            Assert.Equal(reportDto.WorkloadGrade.Level, (Level)report.WorkloadLevel);
            Assert.Equal(reportDto.HighThisWeek, report.HighThisWeek);
            Assert.Equal(reportDto.LowThisWeek, report.LowThisWeek);
            Assert.Equal(reportDto.Date, report.Date);
            Assert.Equal(reportDto.AnythingElse, report.AnythingElse);
        }
        [Fact]
        public void ShouldCreateHistoryReport()
        {
            const string HighThisWeek = "My high this week";
            const string LowThisWeek = "My low this week";
            var reportDate = new DateTime(2021, 11, 10);
            var anythingElse = "Anything else";
            var commentary = "nope";
            var name = "Polo";
            var report = new FullWeeklyReport
            {
                ID = 1,
                AuthorId = 1,
                FirstName= name,
                LastName= name,
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
            var reportDto = commands.fullToHistoryDto(report);
            var weeklyEnum = reportDto.WeeklyInformation.GetEnumerator();
            weeklyEnum.MoveNext();
            var moralelyInf = weeklyEnum.Current;
            weeklyEnum.MoveNext();
            var stressInf = weeklyEnum.Current;
            weeklyEnum.MoveNext();
            var workloadInf = weeklyEnum.Current;
            var weeklyNotEnum = reportDto.WeeklyNotations.GetEnumerator();
            weeklyNotEnum.MoveNext();
            var highNot = weeklyNotEnum.Current;
            weeklyNotEnum.MoveNext();
            var lowNot = weeklyNotEnum.Current;
            weeklyNotEnum.MoveNext();
            var anyNot = weeklyNotEnum.Current;

            Assert.Equal(reportDto.ID, report.ID);
            Assert.Equal(reportDto.ID, report.AuthorId);
            Assert.Equal(reportDto.LastName, name);
            Assert.Equal(reportDto.FirstName, name);
            Assert.Equal(moralelyInf.Comments, report.MoraleCommentary);
            Assert.Equal(stressInf.Comments, report.StressCommentary);
            Assert.Equal(workloadInf.Comments, report.WorkloadCommentary);
            Assert.Equal(moralelyInf.StateLevel, report.MoraleLevel);
            Assert.Equal(stressInf.StateLevel, report.StressLevel);
            Assert.Equal(workloadInf.StateLevel, report.WorkloadLevel);
            Assert.Equal(highNot.Text, report.HighThisWeek);
            Assert.Equal("Weekly High", highNot.Title);
            Assert.Equal("Weekly Low", lowNot.Title);
            Assert.Equal("Anything Else", anyNot.Title);
        }
    }
}
