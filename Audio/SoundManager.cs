using System.Media;

namespace FlappyBird.Audio;

/// <summary>
/// Ses efektlerini kod ile üretir (hazır ses dosyası gerekmez) ve çalar.
/// SoundPlayer aynı anda tek ses çalar; yeni ses öncekini keser.
/// </summary>
public sealed class SoundManager : IDisposable
{
    private const int SampleRate = 22050;

    private readonly SoundPlayer _flap = Create(350, 650, 0.09);
    private readonly SoundPlayer _score = Create(880, 1320, 0.18);
    private readonly SoundPlayer _crash = Create(300, 60, 0.40);

    public bool Muted { get; set; }

    public void PlayFlap() => Play(_flap);
    public void PlayScore() => Play(_score);
    public void PlayCrash() => Play(_crash);

    private void Play(SoundPlayer player)
    {
        if (!Muted) player.Play(); // ayrı iş parçacığında çalar, oyunu bekletmez
    }

    // Frekansı başlangıçtan bitişe kayan, giderek kısılan bir WAV üretir.
    private static SoundPlayer Create(double startHz, double endHz, double seconds)
    {
        int count = (int)(SampleRate * seconds);
        var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);

        // WAV başlığı: 16 bit, tek kanal
        writer.Write("RIFF"u8);
        writer.Write(36 + count * 2);
        writer.Write("WAVEfmt "u8);
        writer.Write(16);
        writer.Write((short)1);          // PCM
        writer.Write((short)1);          // kanal sayısı
        writer.Write(SampleRate);
        writer.Write(SampleRate * 2);    // saniyedeki byte
        writer.Write((short)2);          // örnek başına byte
        writer.Write((short)16);         // örnek başına bit
        writer.Write("data"u8);
        writer.Write(count * 2);

        double phase = 0;
        for (int i = 0; i < count; i++)
        {
            double t = (double)i / count;
            phase += 2 * Math.PI * (startHz + (endHz - startHz) * t) / SampleRate;
            double volume = 0.3 * (1 - t);
            writer.Write((short)(Math.Sin(phase) * volume * short.MaxValue));
        }

        writer.Flush();
        stream.Position = 0;

        var player = new SoundPlayer(stream);
        player.Load();
        return player;
    }

    public void Dispose()
    {
        _flap.Dispose();
        _score.Dispose();
        _crash.Dispose();
    }
}
