﻿using CM.WeeklyTeamReport.Domain.Entities.Interfaces;
using System;

namespace CM.WeeklyTeamReport.Domain
{
    public class WeeklyReport : IWeeklyReport
    {
        public int WorkDuration { get; set; }

        public int ID { get; set; }

        public int AuthorId { get; set; }

        public int MoraleGradeId { get; set; }
        public IGrade MoraleGrade { get; set; }

        public int StressGradeId { get; set; }
        public IGrade StressGrade { get; set; }

        public int WorkloadGradeId { get; set; }
        public IGrade WorkloadGrade { get; set; }
        public string HighThisWeek { get; set; }
        public string LowThisWeek { get; set; }
        public string AnythingElse { get; set; }
        public DateTime Date { get; set; }

        public DateTime WeekStartDate {
            get {
                return Date.FirstDateInWeek(IWeeklyReport.StartOfWeek);
            }
        }

        public DateTime WeekEndDate {
            get {
                return WeekStartDate.AddDays(6);
            }
        }

        public WeeklyReport() { }
    }
}
