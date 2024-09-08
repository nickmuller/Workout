using Workout.Types;

namespace Workout.Models;

public abstract class FileBaseModel
{
    public required DateTime Created { get; set; }
    public required DateTime Changed { get; set; }
}

public class WorkoutLogFile : FileBaseModel
{
    public List<WorkoutLog> WorkoutLijst { get; set; } = [];
}

public class WorkoutLog
{
    public required DateTime WorkoutStart { get; set; }
    public DateTime? WorkoutEind { get; set; }
    public required CategorieType Categorie { get; set; }
}

public class PersoonlijkeGegevensLogFile : FileBaseModel
{
    public required decimal Gewicht { get; set; }
}
