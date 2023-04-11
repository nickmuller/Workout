using Workout.Types;

namespace Workout.Models;

public readonly record struct OefeningModel
(
    string Naam,
    int AantalSets,
    int AantalHerhalingen,
    TimeSpan DuurSet,
    TimeSpan DuurPauze,
    string[] Tips,
    string AfbeeldingUrl,
    string VideoUrl,
    InitieelTonen InitieelTonen
);
