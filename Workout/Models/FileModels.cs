using Workout.Types;

namespace Workout.Models;

public abstract class FileBaseModel
{
    public required DateTime Created { get; set; }
    public required DateTime Changed { get; set; }
}

public class WorkoutState : FileBaseModel
{
    public string? Url { get; set; }
}

public class WorkoutLog : FileBaseModel
{
    public required DateTime WorkoutStart { get; set; }
    public DateTime? WorkoutEind { get; set; }
    public required CategorieType Categorie { get; set; }
}

public class PersoonlijkeGegevensLog : FileBaseModel
{
    public required int Gewicht { get; set; }
}
