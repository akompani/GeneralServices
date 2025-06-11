// See https://aka.ms/new-console-template for more information

using GeneralService;
using GeneralServices.Calendars;
using MD.PersianDateTime;

Console.WriteLine("Hello, World!");

Console.WriteLine("Enter persian date:");
var dt = Console.ReadLine();

var dtm = dt.GetMiladyDateStringFromPersian();
Console.WriteLine(dtm);