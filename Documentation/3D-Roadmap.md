# KOZA 3D - Poppy Playtime Tarzı Yol Haritası

## Hedef
Poppy Playtime gibi: birinci şahıs (veya omuz kamerası), karanlık fabrika, el feneri, bulmaca + kovalama + boss fight. Kapakta **kırmızı kelebek** logo.

## Neden Unity?
Flutter ile gerçek 3D korku (ışık, gölge, jumpscare, GrabPack eli) yapılamaz. Unity URP + Android Build ile Poppy benzeri atmosfer kurulur.

## Unity Kurulum (tek seferlik)
1. Unity Hub + Unity 2022.3 LTS + Android Build Support (SDK/NDK/JDK dahil) kur
2. URP template ile proje aç, bu repo içindeki `Assets/_Koza/` klasörünü kopyala
3. Player Settings: package `com.koza.horror`, orientation Portrait/Landscape, min API 24

## Sahne Planı (Poppy mantığı)
- Chapter1-5: dar koridor + fener + VHS kaydı + bulmaca (şalter/kablo tak)
- Final: Prof Ekrem arenası (elektrik telleri), Lanetli Ali scripted kurtarma
- Her kat sonu: Çay+Simit masası (trigger -> can full)

## Kontroller (mobil)
- Sol joystick: yürü, sağ swipe: kamera
- Butonlar: Fener / Etkileşim (GrabPack eli gibi kablo tut) / Dodge / Saldır
- Poppy'deki gibi: Eline elektrik kablosu alıp kapıya takma bulmacası = Prof'un elektriğini kesme sahnesine hazırlık

## Kırmızı Kelebek Logo Spec
- Renk: #DC2626 ana, #7F1D1D gölge, siyah gövde, beyaz göz deseni
- Kullanım: splash screen, app icon, fabrika duvar grafitisi, yükleme ekranı
- Flutter prototipinde `CustomPainter _RedButterflyPainter` ile canlı örnek var (lib/main.dart)
- Unity'ye taşırken: logo SVG/PNG export al, Sprite olarak splash + billboard yap

## Boss'ların 3D Karşılığı
1. Selim Bey: seni taklit eden animasyon (Animator Mirror), hareketsiz kalınca stun
2. Kerem: elinde havya (point light + duman), fener tutunca kör (Light intensity check)
3. Burhan: NavMesh robot sürüsü, sandalye arkası şalter (Interact)
4. Lanetli Ali: hızlı samuray, %30 HP altında 3-7 klon (Instantiate), sırt collider 2x damage
5. Emir: dev cüsseli, klavye ile trap trigger, ESP32 varsa dialog ile geç
6. Prof Ekrem: elektrik particle + dodge dash, şalter kesilince slow + vulnerable

## Sıradaki İş
- Unity'de Chapter1 sahnesini aç: fabrika koridor (ProBuilder), Ali Kayra FPS controller, fener (SpotLight), Selim Bey AI
- Bu Flutter APK v1.1 kapak + mekanik prototipi olarak kalır, gerçek 3D Unity'de büyür
