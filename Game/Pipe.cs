namespace FlappyBird.Game;

/// <summary>Üst ve alt borudan oluşan tek bir engel.</summary>
public class Pipe
{
    public const int Width = 70;
    public const int Gap = 170; // iki boru arasındaki boşluk

    public float X { get; private set; }
    public int GapTop { get; private set; }
    public bool Passed { get; set; } // skor bir kez verilsin diye

    public RectangleF TopBounds => new(X, 0, Width, GapTop);

    public RectangleF BottomBounds(int groundY) =>
        new(X, GapTop + Gap, Width, groundY - GapTop - Gap);

    public bool IsOffScreen => X + Width < 0;

    public void Reset(float x, int gapTop)
    {
        X = x;
        GapTop = gapTop;
        Passed = false;
    }

    public void Move(float speed) => X -= speed;
}
