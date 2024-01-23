namespace MoonCalendar;

public class MoonPhase
{
    private readonly int _currentDay;
    private readonly int _currentMonth;
    private readonly int _currentYear;

    private int _factor1;
    private int _factor2;
    private int _factor3;
    private int _julianDay;
    private int _julianMonth;

    private int _julianYear;

    public MoonPhase() : this(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
    {
    }

    private MoonPhase(int year, int month, int day)
    {
        _currentYear = year;
        _currentMonth = month;
        _currentDay = day;
        Calculate();
        MoonAge = CalculateMoonAge();
        Phase = MoonPhaseCalc(MoonAge);
        Zodiac = GetZodiac();
    }

    public double MoonAge { get; }
    public double MoonDistance { get; private set; } // Moon's distance in earth radius
    public double MoonLatitude { get; private set; } // Moon's ecliptic latitude
    public double MoonLongitude { get; private set; } // Moon's ecliptic longitude
    public string Phase { get; private set; }
    public string Zodiac { get; private set; }

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

        MoonDistance = 60.4 - 3.3 * Math.Cos(anomalyMonthRadian) -
                       0.6 * Math.Cos(2 * synodicMonthRadian - anomalyMonthRadian) -
                       0.5 * Math.Cos(2 * synodicMonthRadian);
        MoonLatitude = 5.1 * Math.Sin(draconicMonthRadian);
        MoonLongitude = 360 * tropicalMonthRadian + 6.3 * Math.Sin(anomalyMonthRadian) +
                        1.3 * Math.Sin(2 * synodicMonthRadian - anomalyMonthRadian) +
                        0.7 * Math.Sin(2 * synodicMonthRadian);
    }

    private string GetZodiac()
    {
        return MoonLongitude switch
        {
            < 33.18 => "Рыбы",
            < 51.16 => "Овен",
            < 93.44 => "Телец",
            < 119.48 => "Близнецы",
            < 135.3 => "Рак",
            < 173.34 => "Лев",
            < 224.17 => "Дева",
            < 242.57 => "Весы",
            < 271.26 => "Скорпион",
            < 302.49 => "Стрелец",
            < 311.72 => "Козерог",
            < 348.58 => "Водолей",
            _ => "Рыбы"
        };
    }

    private double CalculateMoonAge()
    {
        return Normalize((_julianDay - 2451550.1) / 29.530588853) * 29.53;
    }

    private void CalculateJulianDate()
    {
        _julianYear = _currentYear - (int)Math.Floor((12.0 - _currentMonth) / 10.0);
        _julianMonth = _currentMonth + 9;
        if (_julianMonth >= 12)
            _julianMonth -= 12;

        _factor1 = (int)Math.Floor(365.25 * (_julianYear + 4712.0));
        _factor2 = (int)Math.Floor(30.6 * _julianMonth + 0.5);
        _factor3 = (int)Math.Floor((_julianYear / 100.0 + 49.0) * 0.75) - 38;
        _julianDay = _factor1 + _factor2 + _currentDay + 59;
        if (_julianDay > 2299160)
            _julianDay -= _factor3;
    }


    private static string MoonPhaseCalc(double moonsAge)
    {
        return moonsAge switch
        {
            < 1.84566 => "Новолуние",
            < 5.53699 => "Растущий серп",
            < 9.22831 => "Первая четверть",
            < 12.91963 => "Растущая луна",
            < 16.61096 => "Полнолуние",
            < 20.30228 => "Убывающая луна",
            < 23.99361 => "Третья четверть",
            < 27.68493 => "Убывающий месяц",
            _ => "Новолуние"
        };
    }

    private static double Normalize(double v)
    {
        v -= Math.Floor(v);

        if (v < 0)
            v += 1;

        return v;
    }
}