namespace MoonCalendar
{
    public class MoonPhase
    {
        private readonly int _currentDay;
        private readonly int _currentMonth;
        private readonly int _currentYear;

        private int _julianYear;
        private int _julianMonth;
        private int _julianDay;

        private int _factor1;
        private int _factor2;
        private int _factor3;

        public double MoonAge { get; }
        public double MoonDistance { get; private set; } // Moon's distance in earth radius
        public double MoonLatitude { get; private set; } // Moon's ecliptic latitude
        public double MoonLongitude { get; private set; } // Moon's ecliptic longitude
        public string Phase { get; private set; }

        public MoonPhase() : this (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) { }

        private MoonPhase(int year, int month, int day)
        {
            _currentYear = year;
            _currentMonth = month;
            _currentDay = day;
            Calculate();
            MoonAge = CalculateMoonAge();
            Phase = MoonPhaseCalc(MoonAge);
        }

        public static MoonPhase Create(int year, int month, int day)
        {
            if (!DateTime.TryParse($"{day}.{month}.{year}", out _)) throw new ArgumentException("Неверная дата");

            return new MoonPhase(year, month, day);
        }


        private void Calculate()
        {
            CalculateJulianDate();

            var synodicMonthRadian = Normalize((_julianDay - 2451550.1) / 29.530588853) * 2 * Math.PI;
            var anomalyMonthRadian = 2 * Math.PI * Normalize((_julianDay - 2451562.2) / 27.55454988);
            var draconicMonthRadian = 2 * Math.PI * Normalize((_julianDay - 2451565.2) / 27.212220817);
            var tropicalMonthRadian = Normalize((_julianDay - 2451555.8) / 27.321582241);
            
            MoonDistance = 60.4 - 3.3 * Math.Cos(anomalyMonthRadian) - 0.6 * Math.Cos(2 * synodicMonthRadian - anomalyMonthRadian) - 0.5 * Math.Cos(2 * synodicMonthRadian);
            MoonLatitude = 5.1 * Math.Sin(draconicMonthRadian);
            MoonLongitude = 360 * tropicalMonthRadian + 6.3 * Math.Sin(anomalyMonthRadian) + 1.3 * Math.Sin(2 * synodicMonthRadian - anomalyMonthRadian) + 0.7 * Math.Sin(2 * synodicMonthRadian);
           
        }

        private double CalculateMoonAge() => Normalize((_julianDay - 2451550.1) / 29.530588853) * 29.53;
            
        private void CalculateJulianDate()
        {
            _julianYear = _currentYear - (int)Math.Floor((12.0 - _currentMonth) / 10.0);
            _julianMonth = _currentMonth + 9;
            if (_julianMonth >= 12)
                _julianMonth -= 12;

            _factor1 = (int)Math.Floor(365.25 * (_julianYear + 4712.0));
            _factor2 = (int)Math.Floor(30.6 * _julianMonth + 0.5);
            _factor3 = (int)Math.Floor(((_julianYear / 100.0) + 49.0) * 0.75) - 38;
            _julianDay = _factor1 + _factor2 + _currentDay + 59;
            if (_julianDay > 2299160)
                _julianDay -= _factor3;
        }


        private static string MoonPhaseCalc(double moonsAge)
        {
            string phase;
            if (moonsAge < 1.84566)
            {
                phase = "Новолуние";
                return phase;
            }
            if (moonsAge < 5.53699)
            {
                phase = "Растущий серп";
                return phase;
            }
            if (moonsAge < 9.22831)
            {
                phase = "Первая четверть";
                return phase;
            }
            if (moonsAge < 12.91963)
            {
                phase = "Растущая луна";
                return phase;
            }
            if (moonsAge < 16.61096)
            {
                phase = "Полнолуние";
                return phase;
            }
            if (moonsAge < 20.30228)
            {
                phase = "Убывающая луна";
                return phase;
            }
            if (moonsAge < 23.99361)
            {
                phase = "Третья четверть";
                return phase;
            }
            if (moonsAge < 27.68493)
            {
                phase = "Убывающий месяц";
                return phase;
            }
            phase = "Новолуние";
            return phase;
        }

        private static double Normalize(double v)
        {
            v -= Math.Floor(v);

            if (v < 0)
                v += 1;
            
            return v;
        }

    }


}
