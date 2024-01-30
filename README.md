# CopyBoard

# SORUNLAR VE ÇÖZÜMLERİ
## Sorun 1: C++ C# Entegrasyonu ve Ram Erişimi Sorunu

![Sorun 1 Görüntü](https://i.hizliresim.com/tgyf6ig.jpg)

- **Açıklama:** Windows API ile entegrasyon sırasında rame erişimde sorunlar ve boş ya da hatalı veri dönmesi.
- **Çözüm:**
  -  //Sayın helmsys buraya ekleme yapacak

## Sorun 2: C++ DLL ve C# WinForm Veri Gösterme Sorunu

![Sorun 2 Görüntü](https://i.hizliresim.com/h6aross.jpg)

- **Açıklama:** C++ DLL'den gelen veriyi C# WinForm'da gösterirken "System.AccessViolationException" hatası.
- **Çözüm:**
  - //Sayın helmsys buraya ekleme yapacak

## Sorun 3: C# Form Tasarımı Sorunu

![Sorun 4 Görüntü 2](https://i.hizliresim.com/8l9yi1n.jpg)

- **Açıklama:** Windows tasarımına birebir benzeyen bir C# form tasarımı oluşturmak.
- **Çözüm:**
  - Bunifu, DevExpress veya diğer üçüncü taraf araçları kullanarak özelleştirilebilir componentler kullanmak ya da kendi komponentlerini oluşturmak ( bir adet button özelleştirme paketi projeye dahildir)
  - Windows UI Apilerinden yardım alarak profesyonel bir tasarım ortaya çıkartmak.
  - Final tasarımı
  -  ![Görüntü 2](https://i.hizliresim.com/elqoxcb.jpg)

## Sorun 4: Tasarım Kararları ve WinForm Sınırları

![Sorun 4 Görüntü 1](https://i.hizliresim.com/jtk49mv.jpg)

- **Açıklama:** Border radius ve Glassmorphism tasarımının uygulanması.
- **Çözüm:**
  - Projenin başında biraz Glassmorphism ile çalışmanın faydası olacağını düşünnmüştüm lakin hem ana üründe bu tasarım yoktu hem de çalışma olarak sorunluydu. Bundan kaynaklı olarak Glassmorphismden vazgeçtik.
  - Border Radius için internetteki tüm çözümleri (wpf ve winform) denemenize rağmen asla iyi bir sonuç alamayacaksınız. Her daim yaptığınız border radius pikselli kalacaktır. Bunun çözümü olarak Windows UI apisinden Form border radius özelliği çekildi. 

