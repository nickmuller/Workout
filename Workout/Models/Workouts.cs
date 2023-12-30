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
        {
            Naam = "Arnold Press",
            AantalSets = 3,
            AantalHerhalingen = 12,
            Tips = new[] {"Schouderbladen tegen elkaar", "Nek recht, niet buigen"},
            AfbeeldingUrl = "images/arnold-press.gif",
            VideoUrl = "https://www.youtube.com/embed/6Z15_WdXmVw?start=10"
        },
        new OefeningModel
        {
            Naam = "Side raises",
            AantalSets = 3,
            AantalHerhalingen = 12,
            Tips = new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            AfbeeldingUrl = "images/side-raises.gif",
            VideoUrl = "https://www.youtube.com/embed/3VcKaXpzqRo?start=37"
        },
        new OefeningModel
        {
            Naam = "Shoulder presses",
            AantalSets = 3,
            AantalHerhalingen = 12,
            Tips = new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            AfbeeldingUrl = "images/shoulder-presses.gif",
            VideoUrl = "https://www.youtube.com/embed/B-aVuyhvLHU?start=19"
        },
        new OefeningModel
        {
            Naam = "Alternate curls",
            AantalSets = 4,
            AantalHerhalingen = 10,
            Tips = new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            AfbeeldingUrl = "images/alternate-curls.gif",
            VideoUrl = "https://www.youtube.com/embed/sAq_ocpRh_I?start=32"
        },
        new OefeningModel
        {
            Naam = "Zottman curls",
            AantalSets = 4,
            AantalHerhalingen = 10,
            Tips = new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            AfbeeldingUrl = "images/zottman-curl.gif",
            VideoUrl = "https://www.youtube.com/embed/ZrpRBgswtHs?start=60"
        },
        new OefeningModel
        {
            Naam = "Triceps extensions",
            AantalSets = 3,
            AantalHerhalingen = 15,
            Tips = new[] {"Schouderbladen tegen elkaar", "Pols recht, niet buigen"},
            AfbeeldingUrl = "images/triceps-extensions.gif",
            VideoUrl = "https://www.youtube.com/embed/-Vyt2QdsR7E?start=25"
        },
        new OefeningModel
        {
            Naam = "Diamond push-ups",
            AantalSets = 3,
            AantalHerhalingen = 10,
            AfbeeldingUrl = "images/diamond-push-ups.gif",
            VideoUrl = "https://www.youtube.com/embed/J0DnG1_S92I?start=52"
        },
    };

    private static OefeningModel[] Benen => new[]
    {
        Warmup,
        new OefeningModel
        {
            Naam = "Goblet squats",
            AantalSets = 3,
            AantalHerhalingen = 15,
            AfbeeldingUrl = "images/goblet-squats.gif",
            VideoUrl = "https://www.youtube.com/embed/MeIiIdhvXT4?start=60"
        },
        new OefeningModel
        {
            Naam = "Dumbbell lunges",
            AantalSets = 4,
            AantalHerhalingen = 10,
            Tips = new []{"5 herhalingen per been", "Knie bijna in 90 graden", "Andere knie net niet op de grond"},
            AfbeeldingUrl = "images/dumbbell-lunges.gif",
            VideoUrl = "https://www.youtube.com/embed/D7KaRcUTQeE?start=46"
        },
        new OefeningModel
        {
            Naam = "Dumbbell step-up",
            AantalSets = 4,
            AantalHerhalingen = 10,
            Tips = new []{"5 herhalingen per been"},
            AfbeeldingUrl = "images/dumbbell-step-up.gif",
            VideoUrl = "https://www.youtube.com/embed/WCFCdxzFBa4?start=28"
        },
        new OefeningModel
        {
            Naam = "Jump squats",
            AantalSets = 3,
            AantalHerhalingen = 10,
            AfbeeldingUrl = "images/jump-squats.gif",
            VideoUrl = "https://www.youtube.com/embed/A-cFYWvaHr0?start=144"
        },
        new OefeningModel
        {
            Naam = "Dumbbell deadlifts",
            AantalSets = 5,
            AantalHerhalingen = 8,
            Tips = new []{"Schouderbladen tegen elkaar", "Nek recht", "Dumbbells schuin voor benen"},
            AfbeeldingUrl = "images/dumbbell-deadlifts.gif",
            VideoUrl = "https://www.youtube.com/embed/lJ3QwaXNJfw?start=98"
        },
        new OefeningModel
        {
            Naam = "Calf raises",
            AantalSets = 4,
            AantalHerhalingen = 15,
            AfbeeldingUrl = "images/calf-raises.gif",
            VideoUrl = "https://www.youtube.com/embed/lJ3QwaXNJfw?start=98"
        },
    };

    private static OefeningModel[] BorstEnRug => new[]
    {
        Warmup,
        new OefeningModel
        {
            Naam = "Push-ups",
            AantalSets = 3,
            AantalHerhalingen = 20,
            AfbeeldingUrl = "images/push-up.gif",
            VideoUrl = "https://www.youtube.com/embed/IODxDxX7oi4?start=36"
        },
        new OefeningModel
        {
            Naam = "Dumbbell fly",
            AantalSets = 3,
            AantalHerhalingen = 12,
            AfbeeldingUrl = "images/dumbbell-fly.gif",
            VideoUrl = "https://www.youtube.com/embed/eozdVDA78K0?start=104"
        },
        new OefeningModel
        {
            Naam = "Dumbbell pullovers",
            AantalSets = 5,
            AantalHerhalingen = 10,
            AfbeeldingUrl = "images/dumbbell-pullovers.gif",
            VideoUrl = "https://www.youtube.com/embed/gwAN-Njz3Hg?start=93"
        },
        new OefeningModel
        {
            Naam = "Floor presses",
            AantalSets = 3,
            AantalHerhalingen = 12,
            Tips = new []{"Op de grond", "Armen comfortabel in 45 graden"},
            AfbeeldingUrl = "images/dumbbell-press.gif",
            VideoUrl = "https://www.youtube.com/embed/uUGDRwge4F8?start=48"
        },
        new OefeningModel
        {
            Naam = "Dumbbell arm rows",
            AantalSets = 5,
            AantalHerhalingen = 10,
            Tips = new []{"5 herhalingen per arm", "Lichaam stil houden", "Elleboog iets boven rug", "Elleboog niet volledig strekken"},
            AfbeeldingUrl = "images/dumbbell-arm-rows.gif",
            VideoUrl = "https://www.youtube.com/embed/pYcpY20QaE8?start=82"
        },
        new OefeningModel
        {
            Naam = "Bent over raises",
            AantalSets = 3,
            AantalHerhalingen = 15,
            Tips = new []{"Buig iets voorover", "Rug en nek recht", "Armen 90 graden tov borst", "Armen bij strekken iets boven rug"},
            AfbeeldingUrl = "images/bent-over-raises.gif",
            VideoUrl = "https://www.youtube.com/embed/ttvfGg9d76c?start=61"
        },
    };

    private static OefeningModel Warmup => new()
    {
        Naam = "Warmup",
        AantalSets = 1,
        AantalHerhalingen = 1,
        DuurSet = TimeSpan.Zero,
        AfbeeldingUrl = "images/jumping-jack.gif",
        VideoUrl = "https://www.youtube.com/embed/HY7Zuo0bybw?start=52",
        InitieelTonen = InitieelTonen.Video
    };
}