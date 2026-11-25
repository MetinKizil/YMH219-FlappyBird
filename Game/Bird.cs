namespace FlappyBird.Game;

/// <summary>Kuşun konumu, hızı ve yer çekimi davranışı.</summary>
public class Bird
{
    private const float Gravity = 0.45f;      // her adımda hıza eklenir
    private const float FlapVelocity = -7.5f; // eksi değer yukarı demektir
    private const float MaxFallSpeed = 10f;

    public const int Size = 32;
    public const int X = 100;

    public float Y { get; private set; }
    public float Velocity { get; private set; }
    public RectangleF Bounds => new(X, Y, Size, Size);

    public Bird(float startY) => Reset(startY);

    public void Reset(float startY)
    {
        Y = startY;
        Velocity = 0;
    }

    public void Flap() => Velocity = FlapVelocity;

    public void Update()
    {
        Velocity = Math.Min(Velocity + Gravity, MaxFallSpeed);
        Y += Velocity;

        if (Y < 0) // tavandan dışarı çıkmasın
        {
            Y = 0;
            Velocity = 0;
        }
    }
}
