namespace MoonCalendar
{
    public class MoonPhase
    {
        private readonly int _currentDay;
        private readonly int _currentMonth;
        private readonly int _currentYear;

        public double MoonAge { get; private set; }
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
        }

        public static MoonPhase Create(int year, int month, int day)
        {
            if (!DateTime.TryParse($"{day}.{month}.{year}", out _)) throw new ArgumentException("Не верная дата");

            return new MoonPhase(year, month, day);
        }


        private void Calculate()
        {
            
          
            int yy, mm, k1, k2, k3, jd;
            double ip, dp, np, rp;

            //Calculate the Julian date at 12h UT
            yy = _currentYear - (int)Math.Floor((12.0 - _currentMonth) / 10.0);
            mm = _currentMonth + 9;
            if (mm >= 12)
            {
                mm = mm - 12;
            }
            k1 = (int)Math.Floor(365.25 * (yy + 4712.0));
            k2 = (int)Math.Floor(30.6 * mm + 0.5);
            k3 = (int)Math.Floor(((yy / 100.0) + 49.0) * 0.75) - 38;
            jd = k1 + k2 + _currentDay + 59;
            if (jd > 2299160)
            {
                jd = jd - k3;
            }
            //Calculate moon's age in days
            ip = Normalize((jd - 2451550.1) / 29.530588853);
            MoonAge = ip * 29.53;
            Phase = MoonPhaseCalc(MoonAge);
            ip = ip * 2 * Math.PI;
            //Calculate moon's distance
            dp = 2 * Math.PI * Normalize((jd - 2451562.2) / 27.55454988);
            MoonDistance = 60.4 - 3.3 * Math.Cos(dp) - 0.6 * Math.Cos(2 * ip - dp) - 0.5 * Math.Cos(2 * ip);

            //Calculate moon's ecliptic latitude
            np = 2 * Math.PI * Normalize((jd - 2451565.2) / 27.212220817);
            MoonLatitude = 5.1 * Math.Sin(np);
            //calculate moon's ecliptic longitude
            rp = Normalize((jd - 2451555.8) / 27.321582241);
            MoonLongitude = 360 * rp + 6.3 * Math.Sin(dp) + 1.3 * Math.Sin(2 * ip - dp) + 0.7 * Math.Sin(2 * ip);
            //display results
            print(Phase);
            print(MoonAge);
            //Cprint("distance = {0:F2} Earth radius", di);
            //print("Ecliptic:");
            //print("Latitude = {0:F2}", la);
            //print("Longitude = {0:F2}", lo);
            Console.ReadLine();
        }


        string MoonPhaseCalc(double moonsAge)
        {
            string phase;
            if (moonsAge < 1.84566)
            {
                phase = "Новая Луна";
                return phase;
            }
            if (moonsAge < 5.53699)
            {
                phase = "Waxing cresent";
                return phase;
            }
            if (moonsAge < 9.22831)
            {
                phase = "First quarter";
                return phase;
            }
            if (moonsAge < 12.91963)
            {
                phase = "Waxing gibbous";
                return phase;
            }
            if (moonsAge < 16.61096)
            {
                phase = "Полная луна";
                return phase;
            }
            if (moonsAge < 20.30228)
            {
                phase = "Wanning gibbous";
                return phase;
            }
            if (moonsAge < 23.99361)
            {
                phase = "Last quarter";
                return phase;
            }
            if (moonsAge < 27.68493)
            {
                phase = "Wanning crescent";
                return phase;
            }
            phase = "Новая Луна";
            return phase;
        }

        static double Normalize(double v)
        {
            v = v - Math.Floor(v);
            if (v < 0)
            {
                v = v + 1;
            }
            return v;
        }

    }


}
