using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTITSTimetableApp
{
    internal static class Utils
    {
        public static List<Lesson[]> TT;
        public static List<TimeSpan> Calls;
        public static TimeSpan CurrentNextTime
        {
            get
            {
                var mbNow = Calls.Where(i => i.TotalDays > DateTime.Now.TimeOfDay.TotalDays);
                if (mbNow.Count() == 0)
                    return Calls.First();
                else return mbNow.First();
            }
        }
        public static Lesson[] Today;
        public static void UpdateTT()
        {
            var i = (int)DateTime.Today.DayOfWeek;
            Today = TT[i - 1];
        }
        public static TimeSpan LastTime
        {
            get
            {
                int id = Calls.IndexOf(CurrentNextTime);
                if (id == -1 || id == 0)
                    return Calls.Last();
                else
                    return Calls[id - 1];
            }
        }
        public static TimeSpan TrueTimeSub(TimeSpan t1, TimeSpan t2)
        {
            if (t2 < t1)
                return t1.Subtract(t2);
            else return TimeSpan.FromDays(1).Subtract(t2) + t1;
        }

    }
}
