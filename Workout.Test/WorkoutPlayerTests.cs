using Workout.Models;
using Workout.Types;

namespace Workout.Test;

public class WorkoutPlayerTests
{
    [Test]
    public void VolgendeSet_InitieleOefening_GaatNaarPauze()
    {
        using var player = new Player(CategorieType.Benen, 1, 1);
        player.VolgendeSet();

        Assert.That(player.Oefeningnummer, Is.EqualTo(1));
        Assert.That(player.SetNummer, Is.EqualTo(1));
        Assert.That(player.IsPauze, Is.EqualTo(true));
    }

    [Test]
    public void VolgendeSet_LaatsteSet_GaatNaarVolgendeOefening()
    {
        using var player = new Player(CategorieType.Benen, 2, 3);
        player.VolgendeSet();
        player.VolgendeSet();

        Assert.That(player.Oefeningnummer, Is.EqualTo(3));
        Assert.That(player.SetNummer, Is.EqualTo(1));
        Assert.That(player.IsPauze, Is.EqualTo(false));
    }

    [Test]
    public void VolgendeSet_OefeningIsGestart_GaatNaarPauze()
    {
        using var player = new Player(CategorieType.Benen, 2, 1);
        player.Start();
        player.VolgendeSet();

        Assert.That(player.Oefeningnummer, Is.EqualTo(2));
        Assert.That(player.SetNummer, Is.EqualTo(1));
        Assert.That(player.IsPauze, Is.EqualTo(true));
    }

    [Test]
    public void VolgendeSet_LaatsteOefening_GaatNaarPauze()
    {
        using var player = new Player(CategorieType.Benen, 7, 1);
        player.VolgendeSet();

        Assert.That(player.Oefeningnummer, Is.EqualTo(7));
        Assert.That(player.SetNummer, Is.EqualTo(1));
        Assert.That(player.IsKlaar, Is.EqualTo(false));
        Assert.That(player.IsPauze, Is.EqualTo(true));
    }

    [Test]
    public void VolgendeSet_LaatsteOefeningLaatsteSet_GaatNaarKlaar()
    {
        using var player = new Player(CategorieType.Benen, 7, 4);
        player.VolgendeSet();

        Assert.That(player.Oefeningnummer, Is.EqualTo(7));
        Assert.That(player.SetNummer, Is.EqualTo(4));
        Assert.That(player.IsPauze, Is.EqualTo(false));
        Assert.That(player.IsKlaar, Is.EqualTo(true));
    }
}