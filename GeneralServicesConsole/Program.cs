// See https://aka.ms/new-console-template for more information

using GeneralService;
using GeneralServices.Calendars;
using MD.PersianDateTime;

Console.WriteLine("Hello, World!");

var calendar = new PersianCalendar();
var holidays = new List<CalendarHolidayViewModel>()
{
    new (new PersianDateTime(1404,1,1)
    ,new PersianDateTime(1404,1,4)),
    new (new PersianDateTime(1404,1,11)
    ,new PersianDateTime(1404,1,13)),
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
Console.WriteLine("Enter finish date :");
var finish = Console.ReadLine();
Console.WriteLine("Duration By Minutes : " + calendarCore.Duration(start, finish));
var progress = Convert.ToDecimal(Console.ReadLine());
Console.WriteLine("Earn Date = " + calendarCore.EarnDate(progress,start,finish).FullDateTime());


goto EnterValues;
//Console.WriteLine(calendarCore.Duration("1404/01/02 8:30","1404/01/05 16:30"));