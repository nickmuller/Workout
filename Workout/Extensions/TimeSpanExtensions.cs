namespace Workout.Extensions;

public static class TimeSpanExtensions
{
    public static string ToDum(this TimeSpan t)
    {
        if (t.TotalHours < 1)
        {
            return $"{t.Minutes}m";
        }
        if (t.TotalDays < 1)
        {
            return t.Minutes == 0
                ? $"{t.Hours}u"
                : $"{t.Hours}u {t.Minutes}m";
        }

        return $"{t.Days}d";
    }
}