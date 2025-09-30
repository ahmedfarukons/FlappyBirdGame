# 🐦 Flappy Bird - Windows Forms Oyunu

Klasik Flappy Bird oyununun Windows Forms ile geliştirilmiş C# versiyonu. Türkçe değişken isimlendirmeleri ve modern kod yapısı ile temiz bir uygulama.

## 📸 Ekran Görüntüsü

Oyun 400x600 boyutunda bir pencerede çalışır ve orijinal Flappy Bird tasarımını takip eder:
- Gündüz temalı arkaplan
- Yeşil borular
- Sarı kuş karakteri
- Gerçek zamanlı skor takibi

## 🎮 Oynanış

- **Başlatma**: `SPACE` tuşuna basarak oyunu başlatın
- **Zıplama**: `SPACE` tuşu ile kuşu zıplatın
- **Yeniden Başlatma**: Oyun bittiğinde `R` tuşu ile tekrar oynayın
- **Çıkış**: `ESC` tuşu ile oyundan çıkın

### Oyun Mekaniği

- Kuş sürekli yerçekimi etkisi altında aşağı düşer
- Her SPACE tuşuna basıldığında yukarı doğru bir ivme kazanır
- Yeşil borular sağdan sola hareket eder
- Borular arasındaki boşluktan geçerek puan kazanılır
- Boruya, zemine veya tavana çarpmak oyunu bitirir

## 🛠️ Teknolojiler

- **Framework**: .NET 8.0+ (Windows Forms)
- **Dil**: C# 12
- **Grafik**: System.Drawing ile 2D rendering
- **Timer**: 60 FPS (~16ms interval)

## 📋 Gereksinimler

- Windows işletim sistemi
- .NET 8.0 SDK veya daha üstü
- Visual Studio 2022 (opsiyonel, kod editörü için)

## 🚀 Kurulum ve Çalıştırma

### 1. Repo'yu Klonlayın

```bash
git clone https://github.com/ahmedfarukons/FlappyBirdGame.git
cd FlappyBirdGame
```

### 2. Derleyin ve Çalıştırın

**PowerShell veya CMD ile:**
```bash
dotnet build
dotnet run
```

**Visual Studio ile:**
1. `FlappyBirdWin.csproj` dosyasını açın
2. F5'e basarak çalıştırın

### 3. Executable Oluşturma

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

Çıktı dosyası: `bin/Release/net8.0-windows/win-x64/publish/FlappyBirdWin.exe`

## 📁 Proje Yapısı

```
FlappyBirdWin/
├── Assets/                     # Oyun görselleri
│   ├── background-day.png      # Arkaplan
│   ├── base.png                # Zemin
│   ├── pipe-green.png          # Boru
│   ├── yellowbird-downflap.png # Kuş animasyonu 1
│   ├── yellowbird-midflap.png  # Kuş animasyonu 2
│   └── yellowbird-upflap.png   # Kuş animasyonu 3
├── Form1.cs                    # Ana oyun mantığı
├── Form1.Designer.cs           # Form tasarımı (auto-generated)
├── Program.cs                  # Giriş noktası
└── FlappyBirdWin.csproj        # Proje dosyası
```

## 🔧 Kod Mimarisi

### Temel Sınıf: `Form1`

**Önemli Değişkenler:**
- `kusY`, `kusHizi`: Kuş pozisyonu ve hızı
- `yercekimi`, `ziplamaGucu`: Fizik sabitleri
- `boruX`, `boslukY`: Boru konumları
- `skor`: Oyuncu skoru
- `oyunBasladi`, `oyunBitti`: Oyun durumu

**Ana Metodlar:**
- `VarliklariYukle()`: PNG görsellerini yükler
- `OyunuSifirla()`: Oyun değişkenlerini başlangıç durumuna getirir
- `OyunZamanlayici_Tick()`: Her frame'de fizik ve çarpışma hesaplamaları
- `OnPaint()`: Ekrana çizim işlemleri
- `BorulariCiz()`: Üst ve alt boruları render eder
- `Form1_TusaBasildi()`: Klavye girişlerini işler

### Fizik Sistemi

```csharp
yercekimi = 0.6        // Aşağı doğru ivme
ziplamaGucu = -8.5     // Yukarı doğru anlık hız
kusHizi += yercekimi   // Her frame'de hız artışı
kusY += kusHizi        // Pozisyon güncelleme
```

### Çarpışma Kontrolü

Rectangle intersection kullanılarak:
- Kuş-Üst Boru
- Kuş-Alt Boru
- Kuş-Zemin
- Kuş-Tavan

## 🎨 Özelleştirme

### Oyun Hızını Değiştirme

```csharp
oyunZamanlayici.Interval = 16;  // Düşük = Hızlı, Yüksek = Yavaş
```

### Zorluk Ayarı

```csharp
boslukYuksekligi = 110;  // Daha küçük = Daha zor
boruX -= 3;              // Daha büyük = Daha hızlı
```

### Pencere Boyutu

```csharp
Width = 400;   // Genişlik
Height = 600;  // Yükseklik
```

## 🐛 Bilinen Sorunlar ve Çözümler

### "Varlık bulunamadı" Hatası

Eğer görseller yüklenmiyorsa:
1. `Assets` klasörünün proje kök dizininde olduğundan emin olun
2. `.csproj` dosyasında `<Content Include="Assets\**\*">` satırının olduğunu kontrol edin
3. `bin/Debug/net8.0-windows/Assets/` klasörüne dosyaların kopyalandığını doğrulayın

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir. Görseller orijinal Flappy Bird oyunundan alınmıştır.

## 🤝 Katkıda Bulunma

Pull request'ler memnuniyetle karşılanır:
1. Fork yapın
2. Feature branch oluşturun (`git checkout -b yeni-ozellik`)
3. Commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Push edin (`git push origin yeni-ozellik`)
5. Pull Request açın

## 👨‍💻 Geliştirici

**Ahmed Faruk Ons**
- GitHub: [@ahmedfarukons](https://github.com/ahmedfarukons)

## 📊 Versiyon Geçmişi

- **v1.0** (2025) - İlk yayın
  - Temel oyun mekaniği
  - Türkçe değişken isimlendirmeleri
  - Sağlam varlık yükleme sistemi
  - Çarpışma algılama
  - Skor sistemi

---

⭐ Beğendiyseniz yıldız vermeyi unutmayın!
