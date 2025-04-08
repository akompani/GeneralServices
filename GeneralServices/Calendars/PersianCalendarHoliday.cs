using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralServices.Models;

namespace GeneralServices.Calendars
{
    public class PersianCalendarHoliday
    {
        public PersianCalendarHoliday()
        {
            
        }

        public PersianCalendarHoliday(int calendarId, string startHolidayDate, string finishHolidayDate,string notes = null)
        {
            CalendarId = calendarId;
            Notes = notes;
            StartHolidayDate = startHolidayDate;
            FinishHolidayDate = finishHolidayDate;
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey("CalendarId")] public PersianCalendar Calendar { get; set; }
        public int CalendarId { get; set; }

        public string? Notes { get; set; }

        [MaxLength(10)]
        public string StartHolidayDate { get; set; }

        [MaxLength(10)]
        public string FinishHolidayDate { get; set; }
    }
}
