using System;
using System.Collections.Generic;
using System.Linq;
using GeneralService;
using GeneralServices.Calendars;
using MD.PersianDateTime;

namespace GeneralServices.Models
{
    public static class DurationGeneralFunctions
    {
        public static PersianDateTime GetStartDateTime(this DurationRangeModel model)
        {
            var d = model.PlanStartDate.ToPersianDateTime();
            if (string.IsNullOrEmpty(model.PlanStartDate)) model.PlanStartTime = "0:0";
            var ts = model.PlanStartTime.Split(":").ToList();

            return d.AddHours(int.Parse(ts[0])).AddMinutes(int.Parse(ts[1]));
        }

        public static PersianDateTime GetFinishDateTime(this DurationRangeModel model)
        {
            var d = model.PlanFinishDate.ToPersianDateTime();
            if (string.IsNullOrEmpty(model.PlanFinishTime)) model.PlanFinishTime = "0:0";
            var ts = model.PlanFinishTime.Split(":").ToList();

            return d.AddHours(int.Parse(ts[0])).AddMinutes(int.Parse(ts[1]));
        }

        public static void SetStartFromDateTime(this DurationRangeModel model, PersianDateTime start)
        {
            model.PlanStartDate = start.ToShortDateString();
            model.PlanStartTime = start.ToString("HH:mm");
        }

        public static void SetFinishFromDateTime(this DurationRangeModel model, PersianDateTime finish)
        {
            model.PlanFinishDate = finish.ToShortDateString();
            model.PlanFinishTime = finish.ToString("HH:mm");
        }

        public static void CalculateChangeTimes(this DurationRangeModel model, string changeMode, PersianCalendarCore calendarCore)
        {
            changeMode = changeMode.ToUpper();

            PersianDateTime doStart= calendarCore.ExactWorkStartTime(model.GetStartDateTime());
            PersianDateTime doFinish, auditStart, auditFinish;

            if (changeMode == "FINISHDATE")
            {
                auditFinish = calendarCore.ExactWorkFinishTime(model.GetFinishDateTime());

                auditStart = calendarCore.ExactWorkStartTime(calendarCore.AddHours(auditFinish, -model.AuditDurationHours));
                doFinish = auditStart;
                doStart = calendarCore.ExactWorkStartTime(calendarCore.AddHours(doFinish, -model.DoDurationHours));

                model.DoDurationDays = calendarCore.DurationDays(doStart, doFinish);
                model.AuditDurationDays = calendarCore.DurationDays(auditStart, auditFinish);
            }
            else
            {
                if (changeMode.EndsWith("DAYS"))
                {
                    if (changeMode == "DODURATIONDAYS")
                    {
                        doFinish = calendarCore.AddDays(doStart, model.DoDurationDays);
                        model.DoDurationHours = calendarCore.DurationByHour(doStart, doFinish);
                    }
                    else
                    {
                        doFinish = calendarCore.AddHours(doStart, model.DoDurationHours);
                        model.DoDurationDays = calendarCore.DurationDays(doStart, doFinish);
                    }


                    auditStart = calendarCore.ExactWorkStartTime(doFinish);

                    if (changeMode == "DODURATIONDAYS")
                    {
                        auditFinish = calendarCore.AddHours(auditStart, model.AuditDurationHours);
                        model.AuditDurationDays = calendarCore.DurationDays(auditStart, auditFinish);
                    }
                    else
                    {
                        auditFinish = calendarCore.AddDays(auditStart, model.AuditDurationDays);
                        model.AuditDurationHours = calendarCore.DurationByHour(auditStart, auditFinish);
                    }
                    
                }
                else
                {
                    doFinish = calendarCore.AddHours(doStart, model.DoDurationHours);
                    model.DoDurationDays = calendarCore.DurationDays(doStart, doFinish);

                    auditStart = calendarCore.ExactWorkStartTime(doFinish);

                    auditFinish = calendarCore.AddHours(auditStart, model.AuditDurationHours);
                    model.AuditDurationDays = calendarCore.DurationDays(auditStart, auditFinish);
                }
            }

            model.SetStartFromDateTime(doStart);
            model.SetFinishFromDateTime(auditFinish);
        }
    }

    public class DurationRangeModel
    {
        public int CalendarId { get; set; }

        public string PlanStartDate { get; set; }
        public string PlanStartTime { get; set; }

        public string PlanFinishDate { get; set; }
        public string PlanFinishTime { get; set; }

        public double DoDurationHours { get; set; }
        public int DoDurationDays { get; set; }

        public double AuditDurationHours { get; set; }
        public int AuditDurationDays { get; set; }
    }
}
