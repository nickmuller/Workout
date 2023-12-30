using Workout.Types;

namespace Workout.Models;

public readonly record struct OefeningModel(
    string Naam,
    int AantalSets,
    int AantalHerhalingen,
    string AfbeeldingUrl,
    string VideoUrl
)
{
    public string[] Tips { get; init; } = Array.Empty<string>();
    public TimeSpan DuurSet { get; init; } = TimeSpan.FromMinutes(1);
    public TimeSpan DuurPauze { get; init; } = TimeSpan.FromMinutes(1);
    public InitieelTonen InitieelTonen { get; init; } = InitieelTonen.Afbeelding;
}