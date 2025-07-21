using System;
using System.Collections.Generic;
using System.Linq;
using MD.PersianDateTime;

namespace GeneralServices
{
    public static class GeneralDateRangeFunctions
    {

        #region Persian
        public static bool IsInPersianRange(this string date, string start, string finish = null,
           bool startDay = true, bool finishDay = true)
        {
            return date.ToPersianDateTime().IsInPersianRange(start.ToPersianDateTime(), finish.ToPersianDateTime(), startDay, finishDay);
        }

        public static bool IsInPersianRange(this string date, PersianDateTime start, PersianDateTime finish,
            bool startDay = true, bool finishDay = true)
        {
            return date.ToPersianDateTime().IsInPersianRange(start, finish, startDay, finishDay);
        }

        public static bool IsInPersianRange(this PersianDateTime date, PersianDateTime start, PersianDateTime finish,
            bool startDay = true, bool finishDay = true)
        {
            if (startDay & date == start) return true;
            if (finishDay & date == finish) return true;

            if (date > start & date < finish) return true;

            return false;
        }

        public static bool IsInPersianRange(this PersianDateTime date, PersianDateRangeValueObject rangeObject,
                   bool startDay = true, bool finishDay = true)
        {
            return IsInPersianRange(date, rangeObject.Start, rangeObject.Finish, startDay, finishDay);
        }
        public static bool IsInPersianRange(this PersianDateTime date, IPersianDateRange rangeObject,
                   bool startDay = true, bool finishDay = true)
        {
            return IsInPersianRange(date, rangeObject.GetPersianDateRangeValue(), startDay, finishDay);
        }


        public static List<PersianDateRangeValueObject> MergeInFirstRange(this List<PersianDateRangeValueObject> list1, List<PersianDateRangeValueObject> list2)
        {
            var result = new List<PersianDateRangeValueObject>();

            if (list1.Count > 0 | list2.Count > 0)
            {
                if (list2.Count == 0)
                {
                    foreach (var m in list1) result.Add(m);
                }
                else if (list1.Count == 0)
                {
                    foreach (var m in list2) result.Add(m);
                }
                else
                {
                    var dateList = new List<PersianDateTime>();

                    foreach (var m in list1)
                    {
                        dateList.Add(m.Start);
                        dateList.Add(m.Finish);

                    }

                    foreach (var m in list2)
                    {
                        dateList.Add(m.Start);
                        dateList.Add(m.Finish);
                    }

                    dateList = dateList.DistinctBy(d => d).OrderBy(d => d).ToList();
                    PersianDateTime tStart, tFinish;


                    for (int k = 0; k < dateList.Count - 1; k++)
                    {
                        tStart = dateList[k];
                        if (result.Count > 0 && result.Last().Finish == tStart) tStart = tStart.AddDays(1);

                        tFinish = dateList[k + 1];

                        var list1Index = list1.FindIndex(r => tStart.IsInPersianRange(r) & tFinish.IsInPersianRange(r));

                        if (list1Index != -1)
                        {
                            result.Add(new PersianDateRangeValueObject(tStart, tFinish, list1[list1Index].Index, 1));
                        }
                        else
                        {
                            var list2Index = list2.FindIndex(r =>
                                tStart.IsInPersianRange(r) & tFinish.IsInPersianRange(r));

                            if (list1.Any(l => tFinish.IsInPersianRange(l)) && k < dateList.Count - 2)
                            {
                                tFinish = tFinish.AddDays(-1);
                            }

                            if (list2Index != -1)
                            {
                                result.Add(new PersianDateRangeValueObject(tStart, tFinish, list2[list2Index].Index, 2));

                            }
                        }
                    }

                    if (result.Count > 1)
                    {
                        int rIndex = 0;

                        do
                        {
                            if (result[rIndex].Equals(result[rIndex + 1]))
                            {
                                result[rIndex].Finish = result[rIndex + 1].Finish;
                                result.RemoveAt(rIndex + 1);
                            }
                            else
                            {
                                rIndex++;
                            }

                        } while (rIndex < result.Count - 1);
                    }
                }


            }



            return result;
        }
        #endregion

        #region Georgian


        public static bool IsInGeorgianRange(this string date, string start, string finish = null,
           bool startDay = true, bool finishDay = true)
        {
            return date.ToGeorgianDateTime().IsInGeorgianRange(start.ToGeorgianDateTime(), finish.ToGeorgianDateTime(), startDay, finishDay);
        }

        public static bool IsInRange(this string date, DateTime start, DateTime finish,
            bool startDay = true, bool finishDay = true)
        {
            return date.ToGeorgianDateTime().IsInGeorgianRange(start, finish, startDay, finishDay);
        }

        public static bool IsInGeorgianRange(this DateTime date, DateTime start, DateTime finish,
            bool startDay = true, bool finishDay = true)
        {
            if (startDay & date == start) return true;
            if (finishDay & date == finish) return true;

            if (date > start & date < finish) return true;

            return false;
        }

        public static bool IsInGeorgianRange(this DateTime date, DateRangeValueObject rangeObject,
                   bool startDay = true, bool finishDay = true)
        {
            return IsInGeorgianRange(date, rangeObject.Start, rangeObject.Finish, startDay, finishDay);
        }
        public static bool IsInGeorgianRange(this DateTime date, IGeorgianDateRange rangeObject,
                   bool startDay = true, bool finishDay = true)
        {
            return IsInGeorgianRange(date, rangeObject.GetDateRangeValue(), startDay, finishDay);
        }


        //merge list 2 in list 1 but list 1 is parent and have higher priority
        public static List<DateRangeValueObject> MergeInFirstRange(this List<DateRangeValueObject> list1, List<DateRangeValueObject> list2)
        {
            var result = new List<DateRangeValueObject>();

            if (list1.Count > 0 | list2.Count > 0)
            {
                if (list2.Count == 0)
                {
                    foreach (var m in list1) result.Add(m);
                }
                else if (list1.Count == 0)
                {
                    foreach (var m in list2) result.Add(m);
                }
                else
                {
                    var dateList = new List<DateTime>();

                    foreach (var m in list1)
                    {
                        dateList.Add(m.Start);
                        dateList.Add(m.Finish);

                    }

                    foreach (var m in list2)
                    {
                        dateList.Add(m.Start);
                        dateList.Add(m.Finish);
                    }

                    dateList = dateList.DistinctBy(d => d).OrderBy(d => d).ToList();
                    DateTime tStart, tFinish;


                    for (int k = 0; k < dateList.Count - 1; k++)
                    {
                        tStart = dateList[k];
                        if (result.Count > 0 && result.Last().Finish == tStart) tStart = tStart.AddDays(1);

                        tFinish = dateList[k + 1];
                        
                        var list1Index = list1.FindIndex(r => tStart.IsInGeorgianRange(r) & tFinish.IsInGeorgianRange(r));

                        if (list1Index != -1)
                        {
                            result.Add(new DateRangeValueObject(tStart, tFinish, list1[list1Index].Index, 1));
                        }
                        else
                        {
                            var list2Index = list2.FindIndex(r =>
                                tStart.IsInGeorgianRange(r) & tFinish.IsInGeorgianRange(r));

                            if(list1.Any(l => tFinish.IsInGeorgianRange(l)) && k < dateList.Count - 2)
                            {
                                tFinish = tFinish.AddDays(-1);
                            }

                            if (list2Index != -1)
                            {
                                result.Add(new DateRangeValueObject(tStart, tFinish, list2[list2Index].Index, 2));

                            }
                        }
                    }

                    if (result.Count > 1)
                    {
                        int rIndex = 0;

                        do
                        {
                            if (result[rIndex].Equals(result[rIndex + 1]))
                            {
                                result[rIndex].Finish = result[rIndex + 1].Finish;
                                result.RemoveAt(rIndex + 1);
                            }
                            else
                            {
                                rIndex++;
                            }

                        } while (rIndex < result.Count - 1);
                    }
                }


            }



            return result;
        }
        #endregion
    }
}
