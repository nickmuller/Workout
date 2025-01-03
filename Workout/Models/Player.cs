using Workout.Types;

namespace Workout.Models;

public class Player : IDisposable
{
    private Timer? timer;
    private readonly OefeningModel[] oefeningen;
    private OefeningModel oefening;
    private TimeSpan resterendeTijdSet;
    private TimeSpan resterendeTijdPauze;

    public CategorieType Categorie { get; private set; }
    public OefeningModel Oefening => oefening;
    public OefeningModel Vorige { get; private set; }
    public OefeningModel Volgende { get; private set; }
    public Modus Modus { get; private set; } = Modus.Handmatig;
    public int Oefeningnummer { get; private set; }
    public int AantalOefeningen => oefeningen.Length;
    public int AantalSetsAfgerond => oefeningen.Where((_, i) => i < Oefeningnummer - 1).Sum(o => o.AantalSets) + SetNummer - 1 + (IsKlaar ? 1 : 0);
    public int TotaalAantalSets => oefeningen.Sum(o => o.AantalSets);
    public int PercentageSetsAfgerond => (int)Math.Round((double)AantalSetsAfgerond / TotaalAantalSets * 100);
    public int SetNummer { get; private set; }
    public TimeSpan ResterendeTijdSet => resterendeTijdSet;
    public TimeSpan ResterendeTijdPauze => resterendeTijdPauze;
    public int PercentageTijd => IsPauze
        ? (int)Math.Round(resterendeTijdPauze.TotalSeconds / oefening.DuurPauze.TotalSeconds * 100)
        : (int)Math.Round(resterendeTijdSet.TotalSeconds / oefening.DuurSet.TotalSeconds * 100);
    public bool IsPauze { get; private set; }
    public bool IsKlaar { get; private set; }
    public DateTime? WorkoutStart { get; private set; }
    public DateTime? WorkoutEind { get; private set; }
    public TimeSpan? WorkoutDuur => WorkoutStart.HasValue ? (WorkoutEind ?? DateTime.Now) - WorkoutStart.Value : null;

    public event Action? OnTick;
    public event Action? OnSetChange;
    public event Action? OnStart;
    public event Action? OnEind;

    public Player(CategorieType categorie, int oefeningnummer, int setNummer)
    {
        Categorie = categorie;
        oefeningen = Workouts.Oefeningen(categorie);
        Oefeningnummer = oefeningnummer;
        SetNummer = setNummer;
        oefening = oefeningen[oefeningnummer - 1];
        Vorige = oefeningnummer > 1 ? oefeningen.Skip(oefeningnummer - 2).Take(1).Single() : default;
        Volgende = oefeningen.Skip(oefeningnummer).Take(1).SingleOrDefault();
        resterendeTijdSet = oefening.DuurSet;
        resterendeTijdPauze = oefening.DuurPauze;
        timer = new Timer(TimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
    }

    public void Start()
    {
        WorkoutStart ??= DateTime.Now;
        Modus = Modus.Automatisch;

        if (oefening.DuurSet == TimeSpan.Zero)
            IsPauze = true;

        timer?.Change(1000, 1000);
        OnStart?.Invoke();
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
            resterendeTijdPauze = resterendeTijdPauze.Subtract(TimeSpan.FromSeconds(1));
            if (resterendeTijdPauze <= TimeSpan.Zero)
                VolgendeSet();
        }
        else
        {
            resterendeTijdSet = resterendeTijdSet.Subtract(TimeSpan.FromSeconds(1));
            if (resterendeTijdSet <= TimeSpan.Zero)
                IsPauze = true;
        }

        OnTick?.Invoke();
    }

    public void HerstartSet()
    {
        resterendeTijdSet = oefening.DuurSet;
        resterendeTijdPauze = oefening.DuurPauze;
        IsPauze = false;
        IsKlaar = false;
    }

    public void VorigeSet()
    {
        timer?.Change(1000, 1000);

        if (Vorige == default)
        {
            // Dit is de eerste oefening
            HerstartSet();
        }
        else if (IsPauze)
        {
            IsPauze = false;
        }
        else if (SetNummer > 1)
        {
            if (!IsKlaar)
                SetNummer--;

            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = true;
            IsKlaar = false;
            OnSetChange?.Invoke();
        }
        else
        {
            // Vorige oefening
            Oefeningnummer--;
            oefening = oefeningen[Oefeningnummer - 1];
            Vorige = Oefeningnummer > 1 ? oefeningen.Skip(Oefeningnummer - 2).Take(1).Single() : default;
            Volgende = oefeningen.Skip(Oefeningnummer).Take(1).SingleOrDefault();
            SetNummer = oefening.AantalSets;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsKlaar = false;
            OnSetChange?.Invoke();
        }
    }

    public void VolgendeSet()
    {
        timer?.Change(1000, 1000);

        if (!IsPauze && AantalSetsAfgerond + 1 < TotaalAantalSets)
        {
            IsPauze = true;
        }
        else if (SetNummer < oefening.AantalSets && IsPauze)
        {
            // Volgende set
            SetNummer++;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = false;
            OnSetChange?.Invoke();
        }
        else if (SetNummer == oefening.AantalSets && Volgende != default)
        {
            // Volgende oefening
            Oefeningnummer++;
            oefening = oefeningen[Oefeningnummer - 1];
            Vorige = Oefeningnummer > 1 ? oefeningen.Skip(Oefeningnummer - 2).Take(1).Single() : default;
            Volgende = oefeningen.Skip(Oefeningnummer).Take(1).SingleOrDefault();
            SetNummer = 1;
            resterendeTijdSet = oefening.DuurSet;
            resterendeTijdPauze = oefening.DuurPauze;
            IsPauze = false;
            OnSetChange?.Invoke();
        }
        else if (SetNummer == oefening.AantalSets && Volgende == default)
        {
            // Dit was de laatste oefening, stoppen
            Stop();
            resterendeTijdSet = TimeSpan.Zero;
            IsPauze = false;
            IsKlaar = true;
            WorkoutEind ??= DateTime.Now;
            OnEind?.Invoke();
        }
        else
        {
            throw new InvalidOperationException("Onverwachte situatie");
        }
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
