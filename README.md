# YMH219 - Flappy Bird

Nesne Tabanlı Programlama (YMH 219) dersi için C# ve Windows Forms ile geliştirilen, Flappy Bird benzeri masaüstü oyunu.

**Öğrenci:** _(adını soyadını yaz)_
**Öğrenci No:** _(öğrenci numaranı yaz)_

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

## Sınıf Yapısı

| Sınıf | Görevi |
|---|---|
| `Bird` | Kuşun konumu, hızı, yer çekimi ve zıplama |
| `Pipe` | Engelin konumu, boşluğu ve hareketi |
| `FlappyGame` | Oyun durumu, skor, hız ve çarpışma kontrolü |
| `GameForm` | Çizim, klavye olayları, zamanlayıcı ve buton |

## Ekran Görüntüleri

_(en az 2 ekran görüntüsü eklenecek)_

## Bilinen Sınırlamalar

- Oyun hızı sabittir, zorluk seviyesi yoktur.
- En yüksek skor saklanmaz.
- Pencere boyutu sabittir; ekran ölçeklemesine göre uyarlanmaz.
- Çarpışma, kuşun yuvarlak şekli yerine onu çevreleyen kare ile hesaplanır.
