using System;
using System.Collections.Generic;

namespace SGEP.Models
{
    ///<summary>
    ///Representa um mês em um ano. É usado para facilitar os cálculos em HistogramaController::GraphData
    ///</summary>
    public struct MonthPeriod
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        ///<summary>
        ///Retorna um MonthPeriod com "months" meses a mais que o MonthPeriod "m".
        ///</summary>
        public static MonthPeriod operator +(MonthPeriod m, int months)
        {
            m.Year = (m.Year*12 + m.Month + months)/12;
            m.Month = (m.Month + months)%12;
            return m;
        }

        public static MonthPeriod operator ++(MonthPeriod m) => m + 1;
        ///<summary>
        ///Retorna uma coleção de MonthPeriods situados entre m2 e m1 com a taxa de amostragem de um mês.
        ///</summary>
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
            Month = date.Month - 1,
            Year = date.Year
        };

        public static implicit operator string (MonthPeriod m) => m.ToString();
        
        public static bool operator <(MonthPeriod m1, MonthPeriod m2) => m1.Year < m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month < m2.Month;
        public static bool operator >(MonthPeriod m1, MonthPeriod m2) => m1.Year > m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month > m2.Month;
        public static bool operator <=(MonthPeriod m1, MonthPeriod m2) => m1.Year < m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month <= m2.Month;
        public static bool operator >=(MonthPeriod m1, MonthPeriod m2) => m1.Year >= m2.Year || 
                                                                         m1.Year == m2.Year && 
                                                                         m1.Month >= m2.Month;
        public static bool operator ==(MonthPeriod m1, MonthPeriod m2) => m1.Year == m2.Year && 
                                                                          m1.Month == m2.Month;
        public static bool operator !=(MonthPeriod m1, MonthPeriod m2) => !(m1 == m2);
        public static readonly string[] Months = new string[]
        {
            "jan", "fev", "mar", "abr", 
            "mai", "jun", "jul", "ago", 
            "set", "out", "nov", "dez"
        };
        public override string ToString() => $"{Months[Month]}/{Year}";

        public override bool Equals(object obj)
        {
            return obj is MonthPeriod period &&
                   this == period;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Year, Month);
        }

        public static implicit operator MonthPeriod (string s)
        {
            MonthPeriod mp = new MonthPeriod
            {
                Year = int.Parse(s.Substring(0, 4)),
                Month = int.Parse(s.Substring(4)) - 1
            };
            return mp;
        }

        
    }
}