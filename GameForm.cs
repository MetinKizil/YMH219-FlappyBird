using FlappyBird.Game;

namespace FlappyBird;

public class GameForm : Form
{
    private readonly Bird _bird = new(startY: 280);
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 20 };

    public GameForm()
    {
        Text = "Flappy Bird";
        ClientSize = new Size(480, 640);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.SkyBlue;
        DoubleBuffered = true; // çizimde titremeyi önler
        KeyPreview = true;     // tuş olaylarını önce form alır

        KeyDown += OnKeyDown;
        _timer.Tick += OnTick;
        _timer.Start();
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
            _bird.Flap();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        _bird.Update();
        Invalidate(); // ekranı yeniden çizdirir
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.FillEllipse(Brushes.Gold, _bird.Bounds);
    }
}
