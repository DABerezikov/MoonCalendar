namespace MoonCalendar
{
    public class MoonPhase
    {
        private int _currentDay;
        private int _currentMonth;
        private int _currentYear;

        public MoonPhase() : this (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) { }

        private MoonPhase(int year, int month, int day)
        {
            _currentYear = year;
            _currentMonth = month;
            _currentDay = day;
        }

        public static MoonPhase Create(int year, int month, int day)
        {
            if (!DateTime.TryParse($"{day}.{month}.{year}", out _)) throw new ArgumentException("Неверная дата");
            return new MoonPhase(year, month, day);
        }


        private void MoonPosit(int year, int month, int day)
        {

            double ag; // Moon's age
            double di; // Moon's distance in earth radius
            double la; // Moon's ecliptic latitude
            double lo; // Moon's ecliptic longitude
            string phase;
            int yy, mm, k1, k2, k3, jd;
            double ip, dp, np, rp;

            //Calculate the Julian date at 12h UT
            yy = year - (int)Math.Floor((12.0 - month) / 10.0);
            mm = month + 9;
            if (mm >= 12)
            {
                mm = mm - 12;
            }
            k1 = (int)Math.Floor(365.25 * (yy + 4712.0));
            k2 = (int)Math.Floor(30.6 * mm + 0.5);
            k3 = (int)Math.Floor(((yy / 100.0) + 49.0) * 0.75) - 38;
            jd = k1 + k2 + day + 59;
            if (jd > 2299160)
            {
                jd = jd - k3;
            }
            //Calculate moon's age in days
            ip = Normalize((jd - 2451550.1) / 29.530588853);
            ag = ip * 29.53;
            phase = MoonPhaseCalc(ag);
            ip = ip * 2 * Math.PI;
            //Calculate moon's distance
            dp = 2 * Math.PI * Normalize((jd - 2451562.2) / 27.55454988);
            di = 60.4 - 3.3 * Math.Cos(dp) - 0.6 * Math.Cos(2 * ip - dp) - 0.5 * Math.Cos(2 * ip);

            //Calculate moon's ecliptic latitude
            np = 2 * Math.PI * Normalize((jd - 2451565.2) / 27.212220817);
            la = 5.1 * Math.Sin(np);
            //calculate moon's ecliptic longitude
            rp = Normalize((jd - 2451555.8) / 27.321582241);
            lo = 360 * rp + 6.3 * Math.Sin(dp) + 1.3 * Math.Sin(2 * ip - dp) + 0.7 * Math.Sin(2 * ip);
            //display results
            print(phase);
            print(ag);
            //Cprint("distance = {0:F2} Earth radius", di);
            //print("Ecliptic:");
            //print("Latitude = {0:F2}", la);
            //print("Longitude = {0:F2}", lo);
            Console.ReadLine();
        }


        string MoonPhaseCalc(double ag)
        {
            string phase;
            if (ag < 1.84566)
            {
                phase = "Новая Луна";
                return phase;
            }
            if (ag < 5.53699)
            {
                phase = "Waxing cresent";
                return phase;
            }
            if (ag < 9.22831)
            {
                phase = "First quarter";
                return phase;
            }
            if (ag < 12.91963)
            {
                phase = "Waxing gibbous";
                return phase;
            }
            if (ag < 16.61096)
            {
                phase = "Полная луна";
                return phase;
            }
            if (ag < 20.30228)
            {
                phase = "Wanning gibbous";
                return phase;
            }
            if (ag < 23.99361)
            {
                phase = "Last quarter";
                return phase;
            }
            if (ag < 27.68493)
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
