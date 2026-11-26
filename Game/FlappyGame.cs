namespace FlappyBird.Game;

public enum GameState { Ready, Playing, GameOver }

/// <summary>Oyun kuralları: durum, skor, hız, engeller ve çarpışma.</summary>
public class FlappyGame
{
    public const int Width = 480;
    public const int Height = 640;
    public const int GroundY = 560;

    private const int PipeCount = 3;
    private const int PipeSpacing = 240;
    private const int BirdStartY = 280;

    private readonly Random _random = new();
    private readonly List<Pipe> _pipes = new();
    private readonly HighScoreStore _highScores;

    public Bird Bird { get; } = new(BirdStartY);
    public IReadOnlyList<Pipe> Pipes => _pipes;
    public GameState State { get; private set; }
    public int Score { get; private set; }
    public int BestScore { get; private set; }
    public float Speed { get; } = 3.5f;

    // Arayüz bu olaylara abone olarak ses çalar; oyun sınıfı sesi bilmez.
    public event Action? Flapped;
    public event Action? Scored;
    public event Action? Crashed;

    public FlappyGame(HighScoreStore highScores)
    {
        _highScores = highScores;
        BestScore = highScores.Load();

        for (int i = 0; i < PipeCount; i++)
            _pipes.Add(new Pipe());
        Reset();
    }

    public void Reset()
    {
        State = GameState.Ready;
        Score = 0;
        Bird.Reset(BirdStartY);

        for (int i = 0; i < _pipes.Count; i++)
            _pipes[i].Reset(Width + i * PipeSpacing, RandomGapTop());
    }

    public void Flap()
    {
        if (State == GameState.GameOver) return;
        State = GameState.Playing;
        Bird.Flap();
        Flapped?.Invoke();
    }

    public void Update()
    {
        if (State != GameState.Playing) return;

        Bird.Update();

        foreach (Pipe pipe in _pipes)
        {
            pipe.Move(Speed);

            if (!pipe.Passed && pipe.X + Pipe.Width < Bird.X)
            {
                pipe.Passed = true;
                Score++;
                Scored?.Invoke();
            }

            // ekrandan çıkan boru en sağdakinin arkasına taşınır
            if (pipe.IsOffScreen)
                pipe.Reset(_pipes.Max(p => p.X) + PipeSpacing, RandomGapTop());
        }

        if (HasCollision())
        {
            State = GameState.GameOver;
            SaveBestScore();
            Crashed?.Invoke();
        }
    }

    private void SaveBestScore()
    {
        if (Score <= BestScore) return;

        BestScore = Score;
        _highScores.Save(BestScore);
    }

    private bool HasCollision()
    {
        RectangleF bird = Bird.Bounds;

        return bird.Bottom >= GroundY ||
               _pipes.Any(p => bird.IntersectsWith(p.TopBounds) ||
                               bird.IntersectsWith(p.BottomBounds(GroundY)));
    }

    private int RandomGapTop() => _random.Next(60, GroundY - Pipe.Gap - 60);
}
