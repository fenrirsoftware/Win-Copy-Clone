<div align="center" >

  # 📋 CopyBoard 📋
  ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
  ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
  ![C++](https://img.shields.io/badge/c++-%2300599C.svg?style=for-the-badge&logo=c%2B%2B&logoColor=white)
</div>

# SORUNLAR
- ## Sorun 1: _Hatalı Dönüş değerleri_
  _Windows API ile entegrasyon sırasında RAM'e erişimde sorunlar ve boş ya da hatalı veri dönmesi._
  <div align="center" >
    <img src="https://github.com/fenrirsoftware/Win-Copy-Remake/assets/84701901/3449b53f-48ab-4f9a-b649-fc0d1a4f48aa" width=35% height=35% >
  </div>

- ## Sorun 2: _C++ ve C# Veri Gösterme Sorunu_
  _C++'tan gelen veriyi C# WinForm'da gösterirken `System.AccessViolationException` hatası._
  <div align="center" >
    <img src="https://github.com/fenrirsoftware/Win-Copy-Remake/assets/84701901/39fa2c31-1b92-4691-8faf-a0add2de107e" width=80% height=40% >
  </div>

- ## Sorun 3: _C# Form Tasarımı Sorunu_
  _Windows tasarımına birebir benzeyen bir C# form tasarımı oluşturmak._
    <div align="center" >
    <img src="https://github.com/fenrirsoftware/Win-Copy-Remake/assets/84701901/ab57cb99-9e33-46d0-b9b0-ac289b55a638" width=80% height=40% >
  </div>

- ## Sorun 4: _Tasarım Kararları ve WinForm Sınırları_
  _Border radius ve Glassmorphism tasarımının uygulanması._
  <div align="center" >
    <img src="https://github.com/fenrirsoftware/Win-Copy-Remake/assets/84701901/02d86062-2881-480c-a16c-952ea3986221" width=35% height=40% >
    <img src="https://github.com/fenrirsoftware/Win-Copy-Remake/assets/84701901/f26f30d2-a441-4d2d-b563-3c7ba25ca852" width=60% height=30% >
  </div>
---
# ÇÖZÜMLER
  - ### _**Çözüm-1:**_
    _Arayüzün kendisinden bir metin kopyaladığımızda verdiği bir hataydı. Bu hatayı çözmek için Arayüz içerisinde bu tür olayların yapılabilmesini kısıtlamamız yeterliydi. Oldukça spesifik bir hatayı basit bir işlem ile çözmemiz diğer sorunlara ayıracak vaktimize kazanç sağladı._
  
  - ### _**Çözüm-2:**_
    _Yapıştırma işlemini başlarda dinliyorduk, çünkü kopyaladığımız metin verisini bir şekilde kullanmamız gerekiyordu. Fakat dinlememize gerek kalmadan kullanabildiğimizi görünce bu sorunu böyle çözdük. Sorunun aslı geçersiz ram adresine ulaşmaya çalışıyor oluşumuzdu._

  - ### _**Çözüm-3:**_
    _Bunifu, DevExpress veya diğer üçüncü taraf araçları kullanarak özelleştirilebilir componentler kullanmak ya da kendi komponentlerini oluşturmak_ **_(bir adet button özelleştirme paketi projeye dahildir)_**
  
  - ### _**Çözüm-4:**_
    _Projenin başında biraz Glassmorphism ile çalışmanın faydası olacağını düşünnmüştüm lakin hem ana üründe bu tasarım yoktu hem de çalışma olarak sorunluydu. Bundan kaynaklı olarak Glassmorphismden vazgeçtik. Border Radius için internetteki tüm çözümleri_ _**(wpf ve winform)**_ _denemenize rağmen asla iyi bir sonuç alamayacaksınız. Her daim yaptığınız border radius pikselli kalacaktır. Bunun çözümü olarak Windows UI apisinden Form border radius özelliği çekildi._
---
- ## Final: _**Projenin Sonu**_

  <div align="center" >
    <img src="https://github.com/fenrirsoftware/Win-Copy-Remake/assets/84701901/f26f30d2-a441-4d2d-b563-3c7ba25ca852" width=80% height=80% >
  </div>


<h3> 

_Beğendiyseniz_ "⭐"  _verebilirsiniz!_
</h3>