using FlappyBird.Game;

namespace FlappyBird;

/// <summary>Ekran çizimi ve kullanıcı girişi. Oyun kuralları FlappyGame içindedir.</summary>
public class GameForm : Form
{
    private readonly FlappyGame _game = new();
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 20 };
    private readonly Button _restartButton = new();
    private readonly Font _scoreFont = new("Segoe UI", 28, FontStyle.Bold);
    private readonly Font _infoFont = new("Segoe UI", 14, FontStyle.Bold);
    private bool _spaceHeld;

    public GameForm()
    {
        Text = "Flappy Bird";
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(FlappyGame.Width, FlappyGame.Height);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.SkyBlue;
        DoubleBuffered = true; // çizimde titremeyi önler
        KeyPreview = true;     // tuş olaylarını önce form alır

        _restartButton.Text = "Yeniden Başlat";
        _restartButton.Size = new Size(180, 45);
        _restartButton.Location = new Point((FlappyGame.Width - 180) / 2, 340);
        _restartButton.BackColor = Color.White;
        _restartButton.TabStop = false;
        _restartButton.Visible = false;
        _restartButton.Click += (_, _) => Restart();
        Controls.Add(_restartButton);

        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        _timer.Tick += OnTick;
        _timer.Start();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;

        e.SuppressKeyPress = true;
        if (_spaceHeld) return; // tuş basılı tutulunca sürekli zıplamasın
        _spaceHeld = true;
        _game.Flap();
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space) return;

        e.Handled = true;
        _spaceHeld = false;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        _game.Update();
        _restartButton.Visible = _game.State == GameState.GameOver;
        Invalidate(); // ekranı yeniden çizdirir
    }

    private void Restart()
    {
        _game.Reset();
        _restartButton.Visible = false;
        Focus();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;

        foreach (Pipe pipe in _game.Pipes)
        {
            g.FillRectangle(Brushes.ForestGreen, pipe.TopBounds);
            g.FillRectangle(Brushes.ForestGreen, pipe.BottomBounds(FlappyGame.GroundY));
        }

        g.FillRectangle(Brushes.SandyBrown, 0, FlappyGame.GroundY,
                        FlappyGame.Width, FlappyGame.Height - FlappyGame.GroundY);
        g.FillEllipse(Brushes.Gold, _game.Bird.Bounds);

        DrawCentered(g, _game.Score.ToString(), _scoreFont, 30);

        if (_game.State == GameState.Ready)
            DrawCentered(g, "Başlamak için BOŞLUK tuşuna bas", _infoFont, 200);
        else if (_game.State == GameState.GameOver)
            DrawCentered(g, "GAME OVER", _scoreFont, 250);
    }

    private static void DrawCentered(Graphics g, string text, Font font, float y)
    {
        SizeF size = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.Black, (FlappyGame.Width - size.Width) / 2, y);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
            _scoreFont.Dispose();
            _infoFont.Dispose();
        }
        base.Dispose(disposing);
    }
}
