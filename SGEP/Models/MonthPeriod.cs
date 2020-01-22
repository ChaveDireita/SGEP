using System;
using System.Collections.Generic;

namespace SGEP.Models
{
    public struct MonthPeriod
    {
        public int Year { get; private set; }
        public int Month { get; private set; }

        public static MonthPeriod operator +(MonthPeriod m, int months) => new MonthPeriod
        {
            Year = (m.Year + m.Month + months)/12,
            Month = (m.Month + months)%12 + 1
        };

        public static MonthPeriod operator ++(MonthPeriod m) => m + 1;

        public static List<MonthPeriod> operator -(MonthPeriod m1, MonthPeriod m2)
        {
            List<MonthPeriod> months = new List<MonthPeriod>();
            if (m1 >= m2)
                for (MonthPeriod i = m2; i <= m1; i++)
                    months.Add(i);
            return months;
        }

        public static implicit operator MonthPeriod (DateTime date) => new MonthPeriod
        {
            Month = date.Month,
            Year = date.Year
        };

        public static implicit operator string (MonthPeriod m) => m.ToString();
        
        public static bool operator <(MonthPeriod m1, MonthPeriod m2) => m1.Year < m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month < m2.Month;
        public static bool operator >(MonthPeriod m1, MonthPeriod m2) => m1.Year > m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month > m2.Month;
        public static bool operator <=(MonthPeriod m1, MonthPeriod m2) => m1.Year <= m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month <= m2.Month;
        public static bool operator >=(MonthPeriod m1, MonthPeriod m2) => m1.Year >= m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month >= m2.Month;
        public static bool operator ==(MonthPeriod m1, MonthPeriod m2) => m1.Year == m2.Year && 
                                                                          m1.Month == m2.Month;
        public static bool operator !=(MonthPeriod m1, MonthPeriod m2) => m1 != m2;
        public static readonly string[] Months = new string[]
        {
            "janeiro",
            "fevereiro",
            "março",
            "abril",
            "maio",
            "junho",
            "julho",
            "agosto",
            "setembro",
            "outubro",
            "novembro",
            "dezembro"
        };
        public override string ToString() => $"{Months[Month]}/{Year}";
    }
}