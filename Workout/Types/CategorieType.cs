namespace Workout.Types;

public static class CategorieTypeExtensions
{
    public static string DisplayName(this CategorieType type)
    {
        return Items[type];
    }

    public static IReadOnlyDictionary<CategorieType, string> Items =
        new Dictionary<CategorieType, string>
        {
            {CategorieType.SchoudersEnArmen, "Schouders en armen"},
            {CategorieType.Benen, "Benen"},
            {CategorieType.BorstEnRug, "Borst en rug"},
        };
}

public enum CategorieType
{
    SchoudersEnArmen,
    Benen,
    BorstEnRug,
}