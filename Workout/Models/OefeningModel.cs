using Workout.Types;

namespace Workout.Models;

public readonly record struct OefeningModel()
{
    public required string Naam { get; init; }
    public required int AantalSets { get; init; }
    public required int AantalHerhalingen { get; init; }
    public string[] Tips { get; init; } = Array.Empty<string>();
    public TimeSpan DuurSet { get; init; } = TimeSpan.FromSeconds(2);
    public TimeSpan DuurPauze { get; init; } = TimeSpan.FromSeconds(2);
    public string? AfbeeldingUrl { get; init; }
    public string? VideoUrl { get; init; }
    public InitieelTonen InitieelTonen { get; init; } = InitieelTonen.Afbeelding;
}