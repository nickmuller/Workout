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
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Nek recht, niet buigen"},
            "images/arnold-press.gif",
            "https://www.youtube.com/embed/6Z15_WdXmVw?start=10",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Side raises",
            3,
            12,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            "images/side-raises.gif",
            "https://www.youtube.com/embed/3VcKaXpzqRo?start=37",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Shoulder presses",
            3,
            12,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            "images/shoulder-presses.gif",
            "https://www.youtube.com/embed/B-aVuyhvLHU?start=19",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Alternate curls",
            4,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            "images/alternate-curls.gif",
            "https://www.youtube.com/embed/sAq_ocpRh_I?start=32",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Zottman curls",
            4,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            "images/zottman-curl.gif",
            "https://www.youtube.com/embed/ZrpRBgswtHs?start=60",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Triceps extensions",
            3,
            15,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            "images/triceps-extensions.gif",
            "https://www.youtube.com/embed/-Vyt2QdsR7E?start=25",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Diamond push-ups",
            3,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/diamond-push-ups.gif",
            "https://www.youtube.com/embed/J0DnG1_S92I?start=52",
            InitieelTonen.Afbeelding
        ),
    };

    private static OefeningModel[] Benen => new[]
    {
        Warmup,
        new OefeningModel
        (
            "Goblet squats",
            3,
            15,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/goblet-squats.gif",
            "https://www.youtube.com/embed/MeIiIdhvXT4?start=60",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Dumbbell lunges",
            4,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new []{"5 herhalingen per been", "Knie bijna in 90 graden", "Andere knie net niet op de grond"},
            "images/dumbbell-lunges.gif",
            "https://www.youtube.com/embed/D7KaRcUTQeE?start=46",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Dumbbell step-up",
            4,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new []{"5 herhalingen per been"},
            "images/dumbbell-step-up.gif",
            "https://www.youtube.com/embed/WCFCdxzFBa4?start=28",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Jump squats",
            3,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/jump-squats.gif",
            "https://www.youtube.com/embed/A-cFYWvaHr0?start=144",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Dumbbell deadlifts",
            5,
            8,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new []{"Schouderbladen tegen elkaar", "Nek recht", "Dumbbells schuin voor benen"},
            "images/dumbbell-deadlifts.gif",
            "https://www.youtube.com/embed/lJ3QwaXNJfw?start=98",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Calf raises",
            4,
            15,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/calf-raises.gif",
            "https://www.youtube.com/embed/lJ3QwaXNJfw?start=98",
            InitieelTonen.Afbeelding
        ),
    };

    private static OefeningModel[] BorstEnRug => new[]
    {
        Warmup,
        new OefeningModel
        (
            "Push-ups",
            3,
            20,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/push-up.gif",
            "https://www.youtube.com/embed/IODxDxX7oi4?start=36",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Dumbbell fly",
            3,
            12,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/dumbbell-fly.gif",
            "https://www.youtube.com/embed/eozdVDA78K0?start=104",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Dumbbell pullovers",
            5,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            Array.Empty<string>(),
            "images/dumbbell-pullovers.gif",
            "https://www.youtube.com/embed/gwAN-Njz3Hg?start=93",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Floor presses",
            3,
            12,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new []{"Op de grond", "Armen comfortabel in 45 graden"},
            "images/dumbbell-press.gif",
            "https://www.youtube.com/embed/uUGDRwge4F8?start=48",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Dumbbell arm rows",
            5,
            10,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new []{"5 herhalingen per arm", "Lichaam stil houden", "Elleboog iets boven rug", "Elleboog niet volledig strekken"},
            "images/dumbbell-arm-rows.gif",
            "https://www.youtube.com/embed/pYcpY20QaE8?start=82",
            InitieelTonen.Afbeelding
        ),
        new OefeningModel
        (
            "Bent over raises",
            3,
            15,
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(1),
            new []{"Buig iets voorover", "Rug en nek recht", "Armen 90 graden tov borst", "Armen bij strekken iets boven rug"},
            "images/bent-over-raises.gif",
            "https://www.youtube.com/embed/ttvfGg9d76c?start=61",
            InitieelTonen.Afbeelding
        ),
    };

    private static OefeningModel Warmup => new
    (
        "Warmup",
        1,
        1,
        TimeSpan.Zero,
        TimeSpan.FromMinutes(1),
        Array.Empty<string>(),
        "images/jumping-jack.gif",
        "https://www.youtube.com/embed/HY7Zuo0bybw?start=52",
        InitieelTonen.Video
    );
}
