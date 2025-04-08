// See https://aka.ms/new-console-template for more information

using GeneralService;
using GeneralServices.Calendars;
using MD.PersianDateTime;

Console.WriteLine("Hello, World!");

var calendar = new PersianCalendar();
var holidays = new List<PersianCalendarHoliday>()
{
    new (0,"1403/12/29","1403/12/30"),
    new (0, "1404/01/01","1404/01/04"),
    new (0, "1404/01/11","1404/01/13"),
};

//var availableDates = new List<(string, string, byte)>();
//availableDates.Add(("1404/01/02","1404/01/08",100));
//availableDates.Add(("1404/02/02","1404/02/08",50));

var distribution = new PersianCalendarDistribution(calendar
    ,holidays
    ,"1404/01/01"
    ,"1404/03/31");

distribution.Initialize();

var calendarCore = new PersianCalendarCore(distribution);

EnterValues:

Console.WriteLine("Enter start date :");
var start = Console.ReadLine();
Console.WriteLine("Enter Duration :");
int duration = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Add Duration Time is : " + calendarCore.AddMinutes(start.ToPersianDateTime(), duration));

goto EnterValues;
//Console.WriteLine(calendarCore.Duration("1404/01/02 8:30","1404/01/05 16:30"));