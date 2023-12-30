using Workout.Types;

namespace Workout.Models;

public class Player : IDisposable
{
    private Timer? timer;
    private readonly OefeningModel[] oefeningen;
    private OefeningModel oefening;
    private TimeSpan resterendeTijdSet;
    private TimeSpan resterendeTijdPauze;

    public OefeningModel Oefening => oefening;
    public OefeningModel Vorige { get; private set; }
    public OefeningModel Volgende { get; private set; }
    public Modus Modus { get; private set; } = Modus.Handmatig;
    public int Oefeningnummer { get; private set; }
    public int AantalOefeningen => oefeningen.Length;
    public int Set { get; private set; }
    public int Herhaling { get; private set; }
    public TimeSpan ResterendeTijdSet => resterendeTijdSet;
    public TimeSpan ResterendeTijdPauze => resterendeTijdPauze;
    public bool IsPauze { get; private set; }
    public bool IsKlaar { get; private set; }

    public event Action? OnTick;
    public event Action? OnSetChange;

    public Player(CategorieType categorie, int oefeningnummer, int set)
    {
        oefeningen = Workouts.Oefeningen(categorie);
        Oefeningnummer = oefeningnummer;
        Set = set;
        oefening = oefeningen[oefeningnummer - 1];
        Vorige = oefeningnummer > 1 ? oefeningen.Skip(oefeningnummer - 2).Take(1).Single() : default;
        Volgende = oefeningen.Skip(oefeningnummer).Take(1).SingleOrDefault();
        resterendeTijdSet = oefening.DuurSet;
        resterendeTijdPauze = oefening.DuurPauze;
        timer = new Timer(TimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    public void Start()
    {
        Modus = Modus.Automatisch;
        timer?.Change(0, 1000);
        OnSetChange?.Invoke();
    }

    public void Stop()
    {
        Modus = Modus.Handmatig;
        timer?.Change(Timeout.Infinite, Timeout.Infinite); // stop timer
    }

    private void TimerElapsed(object? state)
    {
        if (IsPauze)
        {
            // Laatste set voltooid, pauze is daarna niet nodig
            if (Volgende == default && Set == oefening.AantalSets)
            {
                IsPauze = false;
                VolgendeSet();
            }
            else
            {
                resterendeTijdPauze = resterendeTijdPauze.Subtract(TimeSpan.FromSeconds(1));
                if (resterendeTijdPauze <= TimeSpan.Zero)
                {
                    IsPauze = false;
                    VolgendeSet();
                }
            }
        }
        else
        {
            resterendeTijdSet = resterendeTijdSet.Subtract(TimeSpan.FromSeconds(1));
            var percentageTijdSet = oefening.DuurSet.TotalSeconds == 0 ? 0
                : Convert.ToInt32((oefening.DuurSet.TotalSeconds - resterendeTijdSet.TotalSeconds) / oefening.DuurSet.TotalSeconds * 100);
            Herhaling = Convert.ToInt32(oefening.AantalHerhalingen / 100.0 * percentageTijdSet);
            if (resterendeTijdSet <= TimeSpan.Zero)
            {
                IsPauze = true;
            }
        }
        OnTick?.Invoke();
    }

    public void HerstartSet()
    {
        Herhaling = 0;
        resterendeTijdSet = oefening.DuurSet;
        resterendeTijdPauze = oefening.DuurPauze;
        IsPauze = false;
        IsKlaar = false;
    }

    public void VorigeSet()
    {
        if (Set > 1)
        {
            Set--;
            Herhaling = 0;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = false;
            IsKlaar = false;
            OnSetChange?.Invoke();
        }
        else if (Vorige == default)
        {
            // Dit is de eerste oefening
            HerstartSet();
        }
        else
        {
            // Vorige oefening
            Oefeningnummer--;
            oefening = oefeningen[Oefeningnummer - 1];
            Vorige = Oefeningnummer > 1 ? oefeningen.Skip(Oefeningnummer - 2).Take(1).Single() : default;
            Volgende = oefeningen.Skip(Oefeningnummer).Take(1).SingleOrDefault();
            Set = 1;
            Herhaling = 0;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = false;
            IsKlaar = false;
            OnSetChange?.Invoke();
        }
    }

    public void VolgendeSet()
    {
        if (Set < oefening.AantalSets)
        {
            Set++;
            Herhaling = 0;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = false;
            OnSetChange?.Invoke();
        }
        else if (Volgende == default)
        {
            // Dit was de laatste oefening, stoppen
            Stop();
            IsKlaar = true;
        }
        else
        {
            // Volgende oefening
            Oefeningnummer++;
            oefening = oefeningen[Oefeningnummer - 1];
            Vorige = Oefeningnummer > 1 ? oefeningen.Skip(Oefeningnummer - 2).Take(1).Single() : default;
            Volgende = oefeningen.Skip(Oefeningnummer).Take(1).SingleOrDefault();
            Set = 1;
            Herhaling = 0;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = false;
            OnSetChange?.Invoke();
        }
    }

    public void StartPauze()
    {
        IsPauze = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        timer?.Change(Timeout.Infinite, Timeout.Infinite); // stop timer
        var t = timer;
        timer = null;
        t?.Dispose();
    }
}
