using System;
using System.ComponentModel.DataAnnotations;
using GeneralServices.Models;
using MD.PersianDateTime;

namespace GeneralServices.Calendars
{
    public class PersianCalendar:ICacheModel
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool Saturday { get; set; } = true;

        [MaxLength(5)] public string SaturdayStart1 { get; set; } = "8:00";
        [MaxLength(5)] public string SaturdayStart2 { get; set; } = "13:00";
        [MaxLength(5)] public string SaturdayStart3 { get; set; }

        [MaxLength(5)] public string SaturdayFinish1 { get; set; } = "12:00";
        [MaxLength(5)] public string SaturdayFinish2 { get; set; } = "17:00";
        [MaxLength(5)] public string SaturdayFinish3 { get; set; }

        public bool Sunday { get; set; } = true;
        [MaxLength(5)] public string SundayStart1 { get; set; } = "8:00";
        [MaxLength(5)] public string SundayStart2 { get; set; } = "13:00";
        [MaxLength(5)] public string SundayStart3 { get; set; }

        [MaxLength(5)] public string SundayFinish1 { get; set; } = "12:00";
        [MaxLength(5)] public string SundayFinish2 { get; set; } = "17:00";
        [MaxLength(5)] public string SundayFinish3 { get; set; }

        public bool Monday { get; set; } = true;
        [MaxLength(5)] public string MondayStart1 { get; set; } = "8:00";
        [MaxLength(5)] public string MondayStart2 { get; set; } = "13:00";
        [MaxLength(5)] public string MondayStart3 { get; set; }

        [MaxLength(5)] public string MondayFinish1 { get; set; } = "12:00";
        [MaxLength(5)] public string MondayFinish2 { get; set; } = "17:00";
        [MaxLength(5)] public string MondayFinish3 { get; set; }


        public bool Tuesday { get; set; } = true;
        [MaxLength(5)] public string TuesdayStart1 { get; set; } = "8:00";
        [MaxLength(5)] public string TuesdayStart2 { get; set; } = "13:00";
        [MaxLength(5)] public string TuesdayStart3 { get; set; }

        [MaxLength(5)] public string TuesdayFinish1 { get; set; } = "12:00";
        [MaxLength(5)] public string TuesdayFinish2 { get; set; } = "17:00";
        [MaxLength(5)] public string TuesdayFinish3 { get; set; }

        public bool Wednesday { get; set; } = true;
        [MaxLength(5)] public string WednesdayStart1 { get; set; } = "8:00";
        [MaxLength(5)] public string WednesdayStart2 { get; set; } = "13:00";
        [MaxLength(5)] public string WednesdayStart3 { get; set; }

        [MaxLength(5)] public string WednesdayFinish1 { get; set; } = "12:00";
        [MaxLength(5)] public string WednesdayFinish2 { get; set; } = "17:00";
        [MaxLength(5)] public string WednesdayFinish3 { get; set; }

        public bool Thursday { get; set; } = true;
        [MaxLength(5)] public string ThursdayStart1 { get; set; } = "8:00";
        [MaxLength(5)] public string ThursdayStart2 { get; set; }
        [MaxLength(5)] public string ThursdayStart3 { get; set; }

        [MaxLength(5)] public string ThursdayFinish1 { get; set; } = "12:00";
        [MaxLength(5)] public string ThursdayFinish2 { get; set; }
        [MaxLength(5)] public string ThursdayFinish3 { get; set; }

        public bool Friday { get; set; }
        [MaxLength(5)] public string FridayStart1 { get; set; }
        [MaxLength(5)] public string FridayStart2 { get; set; }
        [MaxLength(5)] public string FridayStart3 { get; set; }

        [MaxLength(5)] public string FridayFinish1 { get; set; }
        [MaxLength(5)] public string FridayFinish2 { get; set; }
        [MaxLength(5)] public string FridayFinish3 { get; set; }

        public string GetDayShiftStart(PersianDayOfWeek day, byte shift)
        {

            return GetType().GetProperty($"{Enum.GetName(day)}Start{shift}").GetValue(this, null)?.ToString() ?? "00:00";
        }

        public string GetDayShiftFinish(PersianDayOfWeek day, byte shift)
        {
            return GetType().GetProperty($"{Enum.GetName(typeof(PersianDayOfWeek), day)}Finish{shift}").GetValue(this, null)?.ToString() ?? "00:00";
        }

        public string[] DefaultCacheNames() => new[] { nameof(PersianCalendar) };
    }
}
