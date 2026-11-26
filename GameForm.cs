using FlappyBird.Audio;
using FlappyBird.Game;
using FlappyBird.Rendering;

namespace FlappyBird;

/// <summary>Ekran çizimi ve kullanıcı girişi. Oyun kuralları FlappyGame içindedir.</summary>
public class GameForm : Form
{
    private readonly FlappyGame _game = new(new HighScoreStore(Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "YMH219-FlappyBird", "highscore.txt")));
    private readonly SoundManager _sounds = new();
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 20 };
    private readonly Button _restartButton = new();
    private readonly GameRenderer _renderer = new();
    private bool _spaceHeld;

    public GameForm()
    {
        Text = "Flappy Bird";
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(FlappyGame.Width, FlappyGame.Height);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true; // çizimde titremeyi önler
        KeyPreview = true;     // tuş olaylarını önce form alır

        _restartButton.Text = "Yeniden Başlat";
        _restartButton.Size = new Size(180, 45);
        _restartButton.Location = new Point((FlappyGame.Width - 180) / 2, 340);
        _restartButton.BackColor = Color.Orange;
        _restartButton.ForeColor = Color.White;
        _restartButton.FlatStyle = FlatStyle.Flat;
        _restartButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        _restartButton.Cursor = Cursors.Hand;
        _restartButton.TabStop = false;
        _restartButton.Visible = false;
        _restartButton.Click += (_, _) => Restart();
        Controls.Add(_restartButton);

        _game.Flapped += _sounds.PlayFlap;
        _game.Scored += _sounds.PlayScore;
        _game.Crashed += _sounds.PlayCrash;

        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        _timer.Tick += OnTick;
        _timer.Start();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.M) // sesi aç/kapat
        {
            _sounds.Muted = !_sounds.Muted;
            return;
        }

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
        _renderer.Advance(_game);
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
        _renderer.Draw(e.Graphics, _game);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
            _sounds.Dispose();
            _renderer.Dispose();
        }
        base.Dispose(disposing);
    }
}
