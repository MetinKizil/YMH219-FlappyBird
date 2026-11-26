# YMH219 - Flappy Bird

Nesne Tabanlı Programlama (YMH 219) dersi için C# ve Windows Forms ile geliştirilen, Flappy Bird benzeri masaüstü oyunu.

**Öğrenci:** Muhammed Metin Kızıl
**Öğrenci No:** 250541038

## Kullanılan Teknolojiler

- C# / .NET 8
- Windows Forms (GDI+ ile çizim)
- Ek NuGet paketi kullanılmamıştır

## Kurulum ve Çalıştırma

Gereksinim: Windows ve .NET 8 SDK (veya .NET masaüstü geliştirme yükü kurulu Visual Studio 2022).

```bash
dotnet run
```

Visual Studio ile: `YMH219-FlappyBird.csproj` dosyasını açıp F5'e bas.

## Temel Özellikler

- BOŞLUK tuşu ile zıplama, yer çekimi ile düşme
- Sağdan sola hareket eden, ekrandan çıkınca yeniden konumlanan borular
- Boru veya zemine çarpınca Game Over
- Geçilen her boru için 1 puan
- Yeniden Başlat butonu
- En yüksek skoru dosyada saklama ve ekranda gösterme
- Kod ile üretilen ses efektleri (zıplama, skor, çarpışma); **M** tuşu ile sessize alma
- Hareketli bulutlar ve zemin, kanat çırpan ve hıza göre eğilen kuş

## Sınıf Yapısı

| Sınıf | Görevi |
|---|---|
| `Bird` | Kuşun konumu, hızı, yer çekimi ve zıplama |
| `Pipe` | Engelin konumu, boşluğu ve hareketi |
| `FlappyGame` | Oyun durumu, skor, hız ve çarpışma kontrolü |
| `HighScoreStore` | En yüksek skoru dosyadan okuma ve yazma |
| `GameForm` | Klavye olayları, zamanlayıcı ve buton |
| `GameRenderer` | Gökyüzü, boru, zemin, kuş ve yazı çizimi |
| `SoundManager` | Ses efektlerini üretme ve çalma |

## Ekran Görüntüleri

_(en az 2 ekran görüntüsü eklenecek)_

## Bilinen Sınırlamalar

- Oyun hızı sabittir, zorluk seviyesi yoktur.
- En yüksek skor yalnızca oyun bittiğinde kaydedilir.
- Pencere boyutu sabittir; ekran ölçeklemesine göre uyarlanmaz.
- Çarpışma, kuşun yuvarlak şekli yerine onu çevreleyen kare ile hesaplanır.
- Aynı anda tek ses çalınır; yeni ses öncekini keser.
