using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralServices.Models;

namespace GeneralServices.Calendars
{
    public class PersianCalendarHoliday:ICacheModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CalendarId")] public PersianCalendar Calendar { get; set; }
        public int CalendarId { get; set; }

        public string Notes { get; set; }

        [MaxLength(10)]
        public string StartHolidayDate { get; set; }

        [MaxLength(10)]
        public string FinishHolidayDate { get; set; }

        public string[] DefaultCacheNames() => new[] { ICacheModel.CreateCacheName(nameof(PersianCalendarHoliday),CalendarId)};
    }
}
