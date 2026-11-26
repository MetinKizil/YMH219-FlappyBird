using System.Drawing.Drawing2D;
using FlappyBird.Game;

namespace FlappyBird.Rendering;

/// <summary>Oyunun tüm çizimlerini GDI+ ile yapar. Oyun durumunu değiştirmez.</summary>
public sealed class GameRenderer : IDisposable
{
    private const int CapHeight = 24; // boru ağzının yüksekliği
    private const int PipeInset = 3;  // gövde, ağızdan bu kadar dardır

    private readonly FontFamily _fontFamily = new("Segoe UI");
    private int _frame; // animasyon sayacı
    private float _groundDistance; // hız değişince zemin sıçramasın diye biriktirilir

    /// <summary>Her zamanlayıcı adımında çağrılır; çarpışınca sahne donar.</summary>
    public void Advance(FlappyGame game)
    {
        if (game.State == GameState.GameOver) return;

        _frame++;
        _groundDistance += game.Speed;
    }

    public void Draw(Graphics g, FlappyGame game)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        DrawSky(g);
        foreach (Pipe pipe in game.Pipes)
            DrawPipe(g, pipe);
        DrawGround(g);
        DrawBird(g, game);

        DrawText(g, game.Score.ToString(), 48, 25);
        if (game.State == GameState.Playing)
            DrawText(g, $"Seviye {game.Level}", 14, 85);

        if (game.State == GameState.Ready)
        {
            DrawText(g, "BOŞLUK tuşu ile başla", 22, 180);
            DrawText(g, "M: sesi aç / kapat", 13, 220);
            DrawText(g, $"En iyi skor: {game.BestScore}", 18, 90);
        }
        else if (game.State == GameState.GameOver)
        {
            DrawText(g, "GAME OVER", 40, 230);
            DrawText(g, $"En iyi skor: {game.BestScore}", 18, 290);
        }
    }

    private void DrawSky(Graphics g)
    {
        var area = new Rectangle(0, 0, FlappyGame.Width, FlappyGame.GroundY);
        using var sky = new LinearGradientBrush(area, Color.FromArgb(80, 170, 230),
            Color.FromArgb(200, 235, 250), LinearGradientMode.Vertical);
        g.FillRectangle(sky, area);

        // bulutlar yavaşça sola kayar ve sağdan tekrar girer
        int loop = FlappyGame.Width + 150;
        for (int i = 0; i < 3; i++)
        {
            int x = loop - (_frame / 2 + i * 210) % loop - 110;
            int y = 90 + i * 70 % 130;
            g.FillEllipse(Brushes.White, x, y + 12, 60, 30);
            g.FillEllipse(Brushes.White, x + 20, y, 50, 42);
            g.FillEllipse(Brushes.White, x + 45, y + 12, 60, 30);
        }
    }

    private static void DrawPipe(Graphics g, Pipe pipe)
    {
        if (pipe.X >= FlappyGame.Width) return; // henüz ekrana girmedi

        RectangleF top = pipe.TopBounds;
        RectangleF bottom = pipe.BottomBounds(FlappyGame.GroundY);

        DrawPipePart(g, new RectangleF(top.X + PipeInset, top.Y, top.Width - 2 * PipeInset, top.Height));
        DrawPipePart(g, new RectangleF(top.X, top.Bottom - CapHeight, top.Width, CapHeight));
        DrawPipePart(g, new RectangleF(bottom.X + PipeInset, bottom.Y, bottom.Width - 2 * PipeInset, bottom.Height));
        DrawPipePart(g, new RectangleF(bottom.X, bottom.Y, bottom.Width, CapHeight));
    }

    private static void DrawPipePart(Graphics g, RectangleF r)
    {
        // koyu - açık - koyu geçişi boruya yuvarlak görünüm verir
        using var brush = new LinearGradientBrush(r, Color.Green, Color.Green, LinearGradientMode.Horizontal);
        brush.InterpolationColors = new ColorBlend
        {
            Colors = new[] { Color.FromArgb(50, 125, 40), Color.FromArgb(150, 220, 80), Color.FromArgb(35, 100, 35) },
            Positions = new[] { 0f, 0.35f, 1f }
        };
        g.FillRectangle(brush, r);
        using var outline = new Pen(Color.FromArgb(30, 70, 30), 2);
        g.DrawRectangle(outline, r.X, r.Y, r.Width, r.Height);
    }

    private void DrawGround(Graphics g)
    {
        const int stripe = 32;
        int offset = (int)_groundDistance % stripe;

        using var soil = new SolidBrush(Color.FromArgb(222, 190, 130));
        using var grass = new SolidBrush(Color.FromArgb(120, 195, 60));
        using var darkGrass = new SolidBrush(Color.FromArgb(90, 165, 45));

        g.FillRectangle(soil, 0, FlappyGame.GroundY, FlappyGame.Width, FlappyGame.Height - FlappyGame.GroundY);
        g.FillRectangle(grass, 0, FlappyGame.GroundY, FlappyGame.Width, 18);

        for (int x = -offset; x < FlappyGame.Width + stripe; x += stripe)
        {
            g.FillPolygon(darkGrass, new[]
            {
                new Point(x, FlappyGame.GroundY), new Point(x + 16, FlappyGame.GroundY),
                new Point(x + 6, FlappyGame.GroundY + 18), new Point(x - 10, FlappyGame.GroundY + 18)
            });
        }
    }

    private void DrawBird(Graphics g, FlappyGame game)
    {
        Bird bird = game.Bird;
        float half = Bird.Size / 2f;

        // başlamadan önce kuş havada hafifçe süzülür (yalnızca görsel)
        float bob = game.State == GameState.Ready ? MathF.Sin(_frame * 0.12f) * 6 : 0;
        float angle = Math.Clamp(bird.Velocity * 4, -25, 70);
        float wing = (_frame / 5 % 2) * 6; // kanat çırpma

        GraphicsState saved = g.Save();
        g.TranslateTransform(Bird.X + half, bird.Y + half + bob);
        g.RotateTransform(angle);

        g.FillEllipse(Brushes.Gold, -half, -half, Bird.Size, Bird.Size);
        g.DrawEllipse(Pens.SaddleBrown, -half, -half, Bird.Size, Bird.Size);
        g.FillEllipse(Brushes.LightYellow, -half - 2, -4 + wing, 18, 11);    // kanat
        g.DrawEllipse(Pens.SaddleBrown, -half - 2, -4 + wing, 18, 11);
        g.FillEllipse(Brushes.White, 2, -12, 13, 13);                        // göz
        g.FillEllipse(Brushes.Black, 8, -8, 5, 5);
        g.FillPolygon(Brushes.OrangeRed, new[]                               // gaga
        {
            new PointF(10, 1), new PointF(half + 8, 5), new PointF(10, 10)
        });

        g.Restore(saved);
    }

    // Beyaz yazı + koyu kenarlık: her arka planda okunur.
    private void DrawText(Graphics g, string text, float size, float y)
    {
        using var path = new GraphicsPath();
        using var format = new StringFormat { Alignment = StringAlignment.Center };
        path.AddString(text, _fontFamily, (int)FontStyle.Bold, size,
                       new PointF(FlappyGame.Width / 2f, y), format);

        using var outline = new Pen(Color.FromArgb(60, 40, 30), 5) { LineJoin = LineJoin.Round };
        g.DrawPath(outline, path);
        g.FillPath(Brushes.White, path);
    }

    public void Dispose() => _fontFamily.Dispose();
}
