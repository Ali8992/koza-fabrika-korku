# KOZA - 3D Mobil Korku Oyunu

**Fabrika:** Koza Fabrikası  
**Karakter:** Ali Kayra  
**Final Boss:** Prof. Ekrem (Elektrik kontrolü)  
**Platform:** Android & iOS (Unity URP - 3D Mobile)

## Hikaye Özeti
Ali Kayra, Koza fabrikasındaki paranormal sorunları çözmek için görevlendirilir. 5 mini-boss + 1 final boss'u yenerek fabrikanın katlarını iner. Finalde Prof. Ekrem ile yüzleşir ve Lanetlenmiş Ali'nin yardımıyla fabrikayı kurtarır.

## Bölümler
1. **Chapter 1 - Selim Bey (NERF'li):** Saldırını kopyalar ama 2 vuruşta 1 counter, counter -10. Zayıf yönü: 2sn hareketsiz kal (3sn sersemler). Puzzle: 3 sigorta topla.
2. **Chapter 2 - Kerem (Selim'in İkizi):** Havya + lehim teli. Zayıf yönü: Elleri hassas, ışığa duyarlı.
3. **Chapter 3 - Roket Motoru Burhan:** Robot + büyüklü küçüklü roketleri kontrol eder. Zayıf yönü: Sandalye arkası şalter. Puzzle: 3 vanayı kapat (roketler susar).
4. **Chapter 4 - Lanetlenmiş Ali:** Samuray zırhı + klon (3-7). Zayıf yönü: Sırtı. Sonra müttefik olur, ESP32 verir.
5. **Chapter 5 - Klavye Delikanlısı Emir:** Bubi tuzakları. ESP32 ile savaşmadan geçilir.
6. **Final - Prof. Ekrem:** Tüm elektriği kontrol eder. Lanetli Ali elektriği kesince zayıflar.

## Ekran + İkon
- Yatay (landscape) ekran, uygulama simgesi: kırmızı kelebek
- Açılış: Ali Kayra'nın fabrikaya girişi hikaye sahnesi (Chapter0)

## Kontroller (Mobil)
- Sol: Joystick (Hareket)
- Sağ: Fener / Saldır / Dodge / Etkileşim
- Can Yenileme: Her kat arası Çay + Simit noktası

## Kurulum
```bash
# Unity 2022.3 LTS + Android Build Support ile aç
git clone https://github.com/KULLANICI_ADIN/koza-horror.git
# Unity Hub -> Open -> D:\projeler\test
```

## Build Alma
- Unity -> File -> Build Settings -> Android -> Switch Platform
- Player Settings -> Package Name: com.koza.horror
- Build -> .aab olarak al (Play Store)

## Proje Yapısı
```
Assets/_Koza/
├── Scripts/
│   ├── Player/AliKayraController.cs
│   ├── Bosses/ (SelimBey, Kerem, Burhan, LanetliAli, Emir, ProfEkrem)
│   └── Managers/ (GameManager, ChapterManager)
├── Scenes/ Chapter1-6 + Final
└── Prefabs/
```

## Git Push
```bash
git add .
git commit -m "initial: Koza mobil korku projesi"
git push -u origin main
```
