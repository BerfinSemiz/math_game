# 🎮 Matematik Oyunu

İlkokul öğrencilerine yönelik eğitici bir matematik oyunu. 4 işlem (toplama, çıkarma, çarpma, bölme) becerilerini geliştirmek için tasarlanmıştır.

## 🎯 Özellikler

### 📚 5 Seviye Sistemi
- **Seviye 1**: Toplama ve çıkarma (1-10 arası sayılar)
- **Seviye 2**: Toplama ve çıkarma (10-99 arası sayılar), Çarpma (1-9 arası sayılar)
- **Seviye 3**: Tüm işlemler (1-50 arası sayılar)
- **Seviye 4**: Tüm işlemler (10-100 arası sayılar)
- **Seviye 5**: Tüm işlemler (10-200 arası sayılar)

### 🎮 Oyun Mekaniği
- Her seviyede 20 soru (4 blok × 5 soru)
- Sorular 5'li bloklar halinde gösterilir
- **Cevapla**: Cevabı gir ve kontrol et
- **PAS**: Soruyu geç (2. pas yanlış kabul edilir)
- **Devam**: Blok sonunda pas geçilen soruları tekrar sor

### ⭐ Yıldız Sistemi
- **11-15 doğru**: ⭐
- **16-18 doğru**: ⭐⭐
- **19-20 doğru**: ⭐⭐⭐
- En az 11 doğru cevap gerekli seviye geçmek için

### ⏱️ Süre Sistemi
Her seviyenin belirli bir süresi vardır
- Seviye 1: 5 dakika
- Seviye 2: 6 dakika  
- Seviye 3: 7 dakika
- Seviye 4: 8 dakika
- Seviye 5: 9 dakika

### 💾 Kaydetme Sistemi
- Oyun ilerlemesi otomatik kaydedilir
- Yüksek skorlar dosyaya kaydedilir
- Oyun kapatılıp açıldığında kaldığı yerden devam edilir

### 🎯 Hile Kodları (Sadece test için!)
- `dotnet run -- open all` - Tüm seviyeleri açar
- `dotnet run -- open 2` - Seviye 2'yi açar
- `dotnet run -- open 3` - Seviye 3'ü açar
- `dotnet run -- open 4` - Seviye 4'ü açar
- `dotnet run -- open 5` - Seviye 5'i açar

## 🚀 Kurulum

### Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows işletim sistemi

### Kurulum Adımları
```bash
# Projeyi klonlayın
git clone https://github.com/BerfinSemiz/math_game.git
cd math_game

# Bağımlılıkları yükleyin
dotnet restore

# Projeyi derleyin
dotnet build --configuration Release

# Oyunu çalıştırın
dotnet run --configuration Release
```

## 🎯 Oyun Kuralları

1. **Yeni Oyun**: "Yeni Oyun" butonuna bas ve adını gir
2. **Seviye Seçimi**: Açık olan seviyelerden birini seç (ilk seviye her zaman açık)
3. **Soru Çözme**: 
   - Cevabı yazın ve "Cevapla" butonuna basın
   - Veya "PAS" butonuna basarak soruyu geçin
4. **Blok Sistemi**: 
   - 5 soru = 1 blok
   - Blok sonunda pas geçilen sorular tekrar sorulur
   - 2. pas yanlış kabul edilir
5. **Seviye Tamamlama**:
   - 20 soru tamamlandığında seviye biter
   - En az 11 doğru cevap gerekli
   - Yıldız sayısına göre sonraki seviye açılır
   - Seviye tamamlandıktan sonra seviye seçim ekranına döner

## 🔧 Teknik Detaylar

- **Framework**: .NET 8.0 Windows Forms
- **Dil**: C#
- **Mimari**: Model-Service-View pattern
- **Veri Saklama**: JSON dosyaları
- **Platform**: Windows

## 📁 Dosya Yapısı

```
math_game/
├── Models/GameModels.cs          # Veri modelleri
├── Services/GameService.cs       # Oyun mantığı
├── Form1.cs                      # Ana form
├── Program.cs                    # Giriş noktası
├── math_game.csproj             # Proje dosyası
├── README.md                    # Bu dosya
├── .gitignore                   # Git ignore dosyası
├── game_save.example.json       # Örnek kayıt dosyası
├── high_scores.example.json     # Örnek skor dosyası
└── settings.example.json        # Örnek ayar dosyası
```

## 🎓 Projeyi Yaparken Neler Öğrenilir?

Bu proje sayesinde şu konuları öğrenirsin:
- **Service Pattern**: Oyun mantığını ayrı bir servis sınıfında yönetme
- **Model-View-Service**: Temiz kod mimarisi
- **Windows Forms**: GUI uygulaması geliştirme
- **JSON Serialization**: Veri kaydetme ve yükleme
- **Event Handling**: Buton tıklama ve form olayları
- **Timer Kullanımı**: Geri sayım ve oyun süresi yönetimi
- **File I/O**: Dosya okuma ve yazma işlemleri
- **Reflection**: Hile kodları için dinamik metod çağrıları
- **LINQ**: Veri filtreleme ve sorgulama
- **Exception Handling**: Hata yönetimi
---

**İyi eğlenceler! 🎉**
