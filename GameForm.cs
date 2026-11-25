namespace FlappyBird;

public class GameForm : Form
{
    public GameForm()
    {
        Text = "Flappy Bird";
        ClientSize = new Size(480, 640);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.SkyBlue;
        DoubleBuffered = true; // çizimde titremeyi önler
    }
}
