using System;
using System.Collections.Generic;
using System.Text;

namespace Deg.DatabaseManager
{
    public class ISOTime
    {
        // The times of start and stop of daylight savings time -- at least in the Eastern
        // time zones -- the times are when the changes take place, so at 2 am on the start,
        // the start time is ST but the end time is in DT
        public static int[,] DST_DATES = {
            {2002,      4,      7,      2,      2002,       10, 27, 2},
            {2003,      4,      6,      2,      2003,       10, 26, 2},
            {2004,      4,      4,      2,      2004,       10, 31, 2},
            {2005,      4,      3,      2,      2005,       10, 30, 2},
            {2006,      4,      2,      2,      2006,       10, 29, 2},
            {2007,      3,      11, 2,      2007,       11, 4,      2},
            {2008,      3,      9,      2,      2008,       11, 2,      2},
            {2009,      3,      8,      2,      2009,       11, 1,      2},
            {2010,      3,      14, 2,      2010,       11, 7,      2},
            {2011,      3,      13, 2,      2011,       11, 6,      2},
            {2012,      3,      11, 2,      2012,       11, 4,      2},
            {2013,      3,      10, 2,      2013,       11, 3,      2},
            {2014,      3,      9,      2,      2014,       11, 2,      2},
            {2015,      3,      8,      2,      2015,       11, 1,      2},
            {2016,      3,      13, 2,      2016,       11, 6,      2},
            {2017,      3,      12, 2,      2017,       11, 5,      2},
            {2018,      3,      11, 2,      2018,       11, 4,      2},
            {2019,      3,      10, 2,      2019,       11, 3,      2},
            {2020,      3,      8, 2,        2020,      11, 1,      2}
        };
        static int N_DST_DATES = DST_DATES.GetLength(0);

        // DST transition times indexed by year
        public static SortedList<int, DateTime> DST_START_DATES_EST = new SortedList<int, DateTime>();
        public static SortedList<int, DateTime> DST_END_DATES_EST = new SortedList<int, DateTime>();

        public static SortedList<int, DateTime> DST_START_DATES_EPT = new SortedList<int, DateTime>();
        public static SortedList<int, DateTime> DST_END_DATES_EPT = new SortedList<int, DateTime>();

        // Static constructor
        static ISOTime()
        {
            // Initialize the DST transition times
            for (int d = 0; d < ISOTime.N_DST_DATES; ++d)
            {
                int year, month, day, hour;

                // DST start
                year = DST_DATES[d, 0];
                month = DST_DATES[d, 1];
                day = DST_DATES[d, 2];
                hour = DST_DATES[d, 3];
                DST_START_DATES_EST.Add(year, new DateTime(year, month, day, hour, 0, 0));
                DST_START_DATES_EPT.Add(year, new DateTime(year, month, day, hour + 1, 0, 0));

                // DST end
                year = DST_DATES[d, 4];
                month = DST_DATES[d, 5];
                day = DST_DATES[d, 6];
                hour = DST_DATES[d, 7];
                DST_END_DATES_EST.Add(year, new DateTime(year, month, day, hour - 1, 0, 0));
                DST_END_DATES_EPT.Add(year, new DateTime(year, month, day, hour, 0, 0));
            }
        }

        // Function that returns the EST and EPT time vectors, and limits according to the given filters
        public static void GetTimeVectors(
                DateTime startDate, DateTime endDate,
                bool isInputEST, List<bool[]> limitDayHours,
                out List<DateTime> TimeVectorEST, out List<DateTime> TimeVectorEPT)
        {
            DateTime start_date_est, end_date_est;
            if (isInputEST)
            {
                start_date_est = startDate;
                end_date_est = endDate;
            }
            else
            {
                // Convert the end points to EST
                start_date_est = ISOTime.EPTToEST(startDate);
                end_date_est = ISOTime.EPTToEST(endDate);
            }

            // Get the date vectors
            ISOTime.GetTimeVectorsEST(start_date_est, end_date_est, out TimeVectorEST, out TimeVectorEPT);

            // Go through and remove the unneeded dates
            List<DateTime> time_vector_compare;
            if (isInputEST)
            {
                time_vector_compare = TimeVectorEST;
            }
            else
            {
                time_vector_compare = TimeVectorEPT;
            }

            int n_dates = time_vector_compare.Count;
            for (int d = n_dates - 1; d >= 0; --d)
            {
                DateTime day = time_vector_compare[d];
                int dow = (int)day.DayOfWeek;
                int hour = day.Hour;
                if (!limitDayHours[dow][hour])
                {
                    TimeVectorEPT.RemoveAt(d);
                    TimeVectorEST.RemoveAt(d);
                }
            }
        }

        // Function that returns EST and EPT continuous time vectors for a given date range (in EST)
        public static void GetTimeVectorsEST(
                DateTime startDateEST, DateTime endDateEST,
                out List<DateTime> TimeVectorEST, out List<DateTime> TimeVectorEPT)
        {
            // Out parameters
            TimeVectorEST = new List<DateTime>();
            TimeVectorEPT = new List<DateTime>();

            // Create the EST vector
            for (DateTime run_date = startDateEST; run_date <= endDateEST; run_date = run_date.AddHours(1))
            {
                TimeVectorEST.Add(run_date);
                if (ISOTime.IsStandardTimeDST(run_date))
                {
                    TimeVectorEPT.Add(run_date.AddHours(1));
                }
                else
                {
                    TimeVectorEPT.Add(run_date);
                }
            }
        }

        // Function that returns EST and EPT continuous time vectors for a given date range (in EPT)
        public static void GetTimeVectorsEPT(
                DateTime startDateEPT, DateTime endDateEPT,
                out List<DateTime> TimeVectorEST, out List<DateTime> TimeVectorEPT)
        {
            // Convert the end points to EST
            DateTime start_date_est = ISOTime.EPTToEST(startDateEPT);
            DateTime end_date_est = ISOTime.EPTToEST(endDateEPT);

            // Get the date vectors
            ISOTime.GetTimeVectorsEST(start_date_est, end_date_est, out TimeVectorEST, out TimeVectorEPT);
        }

        public static void GetTimeVectorsEPT(
                DateTime startDateEPT, DateTime endDateEPT,
                out List<DateTime> TimeVectorEST, out List<DateTime> TimeVectorEPT,
                bool[] limitDOWs, bool[] limitHoursEPT)
        {
            // Convert the end points to EST
            DateTime start_date_est = ISOTime.EPTToEST(startDateEPT);
            DateTime end_date_est = ISOTime.EPTToEST(endDateEPT);

            // Get the date vectors
            ISOTime.GetTimeVectorsEST(start_date_est, end_date_est, out TimeVectorEST, out TimeVectorEPT);

            // Go through and remove the un-needed dates
            int n_dates = TimeVectorEPT.Count;
            for (int d = n_dates - 1; d >= 0; --d)
            {
                DateTime ept = TimeVectorEPT[d];
                int dow = (int)ept.DayOfWeek;
                int hour = ept.Hour;
                if (!limitDOWs[dow] || !limitHoursEPT[hour])
                {
                    TimeVectorEPT.RemoveAt(d);
                    TimeVectorEST.RemoveAt(d);
                }
            }
        }

        // Conversion functions
        public static DateTime CPTToCST(DateTime cpt)
        {
            return ISOTime.IsLocalTimeDST(cpt) ? cpt.AddHours(-1) : cpt;
        }

        public static DateTime MPTToEPT(DateTime mpt)
        {
            return mpt.AddHours(2);
        }

        public static DateTime MSTToCPT(DateTime mst)
        {
            return mst.AddHours(1);
        }

        public static DateTime EPTToEST(DateTime ept)
        {
            return ISOTime.IsLocalTimeDST(ept) ? ept.AddHours(-1) : ept;
        }

        public static DateTime PPTToUTC(DateTime ppt)
        {
            return ISOTime.PPToPST(ppt).AddHours(8);
        }

        public static DateTime UTCToPPT(DateTime utc)
        {
            return ISOTime.PSToPPT(utc.AddHours(-8));
        }

        public static DateTime UTCToMPT(DateTime utc)
        {
            return ISOTime.MSTToMPT(utc.AddHours(-7));
        }

        public static DateTime PSToPPT(DateTime timePst)
        {
            DateTime time_return;

            if (ISOTime.IsStandardTimeDST(timePst))
            {
                time_return = timePst.AddHours(1);
            }
            else
            {
                time_return = timePst;
            }

            return time_return;
        }


        public static DateTime PPToPST(DateTime timePpt)
        {
            DateTime time_return;

            if (ISOTime.IsLocalTimeDST(timePpt))
            {
                time_return = timePpt.AddHours(-1);
            }
            else
            {
                time_return = timePpt;
            }

            return time_return;
        }
        public static DateTime MPTToEST(DateTime timeMPT)
        {
            DateTime utcTime = MPTToUTC(timeMPT);
            return utcTime.AddHours(-5);
        }
        public static DateTime ESTToMPT(DateTime timeEST)
        {
            DateTime utcTime = timeEST.AddHours(5);
            return UTCToMPT(utcTime);
        }
        public static DateTime MPTToUTC(DateTime timeMPT)
        {
            DateTime time_return;
            DateTime timeMST = MPTToMST(timeMPT);
            time_return = MSTToUTC(timeMST);
            return time_return;
        }

        public static DateTime MSTToMPT(DateTime timeMst)
        {
            DateTime time_return;

            if (ISOTime.IsStandardTimeDST(timeMst))
            {
                time_return = timeMst.AddHours(1);
            }
            else
            {
                time_return = timeMst;
            }

            return time_return;
        }

        public static DateTime MPTToMST(DateTime mpt)
        {
            return ISOTime.IsLocalTimeDST(mpt) ? mpt.AddHours(-1) : mpt;
        }

        public static DateTime EPTToUTC(DateTime ept)
        {
            return ISOTime.EPTToEST(ept).AddHours(5);
        }

        public static DateTime CPTToUTC(DateTime cpt)
        {
            return ISOTime.CPTToCST(cpt).AddHours(6);
        }

        public static DateTime CSTToCPT(DateTime cst)
        {
            return ISOTime.IsStandardTimeDST(cst) ? cst.AddHours(1) : cst;
        }

        public static DateTime ESTToEPT(DateTime est)
        {
            return ISOTime.IsStandardTimeDST(est) ? est.AddHours(1) : est;
        }

        public static DateTime ESTToUTC(DateTime est)
        {
            return est.AddHours(5);
        }

        public static DateTime MSTToUTC(DateTime mst)
        {
            return mst.AddHours(7);
        }

        public static DateTime UTCToCPT(DateTime utc)
        {
            return ISOTime.CSTToCPT(utc.AddHours(-6));
        }

        public static DateTime UTCToEPT(DateTime utc)
        {
            return ISOTime.ESTToEPT(utc.AddHours(-5));
        }


        public static DateTime UTCToEST(DateTime utc)
        {
            return utc.AddHours(-5);
        }

        // Returns true if the given local time is in daylight savings time
        // Since there are two gray areas -- 2am-3am doesn't exist on the start day
        // and 1am-2am happens twice on the stop day -- assumes that 
        static public bool IsLocalTimeDST(DateTime time)
        {
            bool is_dst = false;

            // Get the year, month, day, and hour of the time
            int year = time.Year;
            int month = time.Month;
            int day = time.Day;
            int hour = time.Hour;

            if ((time >= ISOTime.DST_START_DATES_EPT[year]) && (time < ISOTime.DST_END_DATES_EPT[year]))
            {
                is_dst = true;
            }

            return is_dst;
        }

        static public bool IsStandardTimeDST(DateTime timeStandard)
        {
            bool is_dst = false;

            // Get the year, month, day, and hour of the time
            int year = timeStandard.Year;
            int month = timeStandard.Month;
            int day = timeStandard.Day;
            int hour = timeStandard.Hour;

            if ((timeStandard >= ISOTime.DST_START_DATES_EST[year]) && (timeStandard < ISOTime.DST_END_DATES_EST[year]))
            {
                is_dst = true;
            }

            return is_dst;
        }

        // Returns true if the given time is on a DST transition day
        static public bool IsTimeDSTDay(DateTime time)
        {
            bool is_dst = false;

            // Get the year, month, day, and hour of the time
            int year = time.Year;
            DateTime time_day = time.Date;

            if ((time_day == ISOTime.DST_START_DATES_EPT[year].Date) || (time_day == ISOTime.DST_END_DATES_EPT[year].Date))
            {
                is_dst = true;
            }

            return is_dst;
        }

        static public bool IsTimeDSTDayFall(DateTime time)
        {
            bool is_dst = false;

            // Get the year, month, day, and hour of the time
            int year = time.Year;
            DateTime time_day = time.Date;

            if (time_day == ISOTime.DST_END_DATES_EPT[year].Date)
            {
                is_dst = true;
            }

            return is_dst;
        }

        static public bool IsTimeDSTDaySpring(DateTime time)
        {
            bool is_dst = false;

            // Get the year, month, day, and hour of the time
            int year = time.Year;
            DateTime time_day = time.Date;

            if (time_day == ISOTime.DST_START_DATES_EPT[year].Date)
            {
                is_dst = true;
            }

            return is_dst;
        }

        // Returns true if the given day is a NERC holiday
        // http://www.nerc.com/~filez/offpeaks.html
        static public bool IsNERCHoliday(DateTime date)
        {
            var isHoliday = false;

            switch (date.Month)
            {
                case 1:     // New Year's day
                    isHoliday = date.Day == 1 || date.DayOfWeek == DayOfWeek.Monday && date.Day == 2;
                    break;
                case 5:     // Memorial Day
                    isHoliday = date.Day > 24 && date.DayOfWeek == DayOfWeek.Monday;
                    break;
                case 7:     // Independence Day
                    isHoliday = date.Day == 4 || date.DayOfWeek == DayOfWeek.Monday && date.Day == 5;
                    break;
                case 9:     // Labor Day
                    isHoliday = date.Day < 8 && date.DayOfWeek == DayOfWeek.Monday;
                    break;
                case 11:    // Thanksgiving Day
                    var thanksGiving = new DateTime(date.Year, 11, 30);
                    for (; thanksGiving.DayOfWeek != DayOfWeek.Thursday; thanksGiving = thanksGiving.AddDays(-1)) ;
                    isHoliday = date == thanksGiving;
                    break;
                case 12:    // Christmas
                    isHoliday = date.Day == 25 || date.DayOfWeek == DayOfWeek.Monday && date.Day == 26;
                    break;
            }

            return isHoliday;
        }

        static public void IsNERCHoliday_test()
        {
            // Define the test set -- all holidays
            int[,] test_days = new int[,] {
                {2007,      1,      1,      1},
                {2007,      5,      28, 1},
                {2007,      7,      4,      1},
                {2007,      9,      3,      1},
                {2007,      11, 22, 1},
                {2007,      12, 25, 1},
                {2008,      1,      1,      1},
                {2008,      5,      26, 1},
                {2008,      7,      4,      1},
                {2008,      9,      1,      1},
                {2008,      11, 27, 1},
                {2008,      12, 25, 1},
                {2009,      1,      1,      1},
                {2009,      5,      25, 1},
                {2009,      7,      4,      1},
                {2009,      9,      7,      1},
                {2009,      11, 26, 1},
                {2009,      12, 25, 1},
                {2010,      1,      1,      1},
                {2010,      5,      31, 1},
                {2010,      7,      5,      1},
                {2010,      9,      6,      1},
                {2010,      11, 25, 1},
                {2010,      12, 25, 1},
                {2011,      1,      1,      1},
                {2011,      5,      30, 1},
                {2011,      7,      4,      1},
                {2011,      9,      5,      1},
                {2011,      11, 24, 1},
                {2011,      12, 26, 1},
                {2012,      1,      2,      1},
                {2012,      5,      28, 1},
                {2012,      7,      4,      1},
                {2012,      9,      3,      1},
                {2012,      11, 22, 1},
                {2012,      12, 25, 1} };

            // Loop through the test vector days
            int errors = 0;
            int i_rows = test_days.GetLength(0);
            for (int i = 0; i < i_rows; ++i)
            {
                DateTime day = new DateTime(test_days[i, 0], test_days[i, 1], test_days[i, 2]);
                bool is_holiday_test = (test_days[i, 3] == 1);
                bool is_holiday = IsNERCHoliday(day);
                if (is_holiday_test == is_holiday)
                {
                }
                else
                {
                    ++errors;
                }
            }
        }

        static public bool IsNERCHolidayOrWeekend(DateTime time)
        {
            if (time.DayOfWeek == DayOfWeek.Sunday || time.DayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }

            return ISOTime.IsNERCHoliday(time);
        }
    }
}
