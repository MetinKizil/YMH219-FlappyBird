namespace FlappyBird.Game;

/// <summary>En yüksek skoru bir metin dosyasında saklar.</summary>
public class HighScoreStore
{
    private readonly string _path;

    public HighScoreStore(string path) => _path = path;

    /// <summary>Dosya yoksa veya bozuksa 0 döner.</summary>
    public int Load()
    {
        try
        {
            return int.TryParse(File.ReadAllText(_path), out int score) && score > 0 ? score : 0;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    public void Save(int score)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            File.WriteAllText(_path, score.ToString());
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // skor yazılamazsa oyun yine de devam eder
        }
    }
}
