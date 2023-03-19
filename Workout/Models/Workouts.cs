using Workout.Types;

namespace Workout.Models;

public static class Workouts
{
    public static OefeningModel[] Oefeningen(CategorieType categorieType)
    {
        return categorieType switch
        {
            CategorieType.SchoudersEnArmen => SchoudersEnArmen,
            CategorieType.Benen => Benen,
            CategorieType.BorstEnRug => BorstEnRug,
            _ => throw new ArgumentOutOfRangeException(nameof(categorieType), categorieType, null)
        };
    }

    private static OefeningModel[] SchoudersEnArmen => new[]
    {
        Warmup,
        new OefeningModel
        (
            "Arnold Press",
            3,
            12,
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Nek recht houden"},
            "images/arnold-press.gif",
            "https://www.youtube.com/embed/6Z15_WdXmVw"
        )
    };

    private static OefeningModel[] Benen => new[]
    {
        Warmup,
        new OefeningModel
        (
            "todo",
            3,
            12,
            TimeSpan.FromMinutes(1),
            new[] {"Tip 1", "Tip 2"},
            "afbeeldingurl",
            "videourl"
        )
    };

    private static OefeningModel[] BorstEnRug => new[]
    {
        Warmup,
        new OefeningModel
        (
            "todo",
            3,
            12,
            TimeSpan.FromMinutes(1),
            new[] {"Tip 1", "Tip 2"},
            "afbeeldingurl",
            "videourl"
        )
    };

    private static OefeningModel Warmup => new
    (
        "Warmup",
        1,
        1,
        TimeSpan.FromMinutes(1),
        Array.Empty<string>(),
        "images/jumping-jack.gif",
        "https://www.youtube.com/embed/HY7Zuo0bybw"
    );
}
