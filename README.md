
# SaveCustomerService
Customer Save Operation Restfull Api
#Proje Teknik Gereklilikleri
#Net.Core 3.1
#.Net Framework 5
#Mssql Server

# Kullanılan Teknik Özellikler

Güvenlik Katmanı: Barear Token ile authentication
İsteklerin Loglanması: Logging ile isteklerin .txt dosyalarına loglanması
Swagger Entegrasyonu: Swagger entegrasyonu sağlanmıştır.
Entity Framework

## Methodlar

**api/User/Create :** Kullanıcının kimlik, isim, soyisim, doğum tarihi inputları NVI doğrulama ile doğrulandıktan sonra eğer db de kayıtlı değilse kullanıcı kayıt edilir. Tüm kayıtlar encrypted şekilde kayıt edilir. Create sonunda kullanıcıya token döner ve diğer methodlarda bu token ile authentication olması beklenir.
**api/User/UserGetById**: Id inputu ile kullanıcının bilgilerini döndürür.
**api/User/DeleteUser:** Id bilgisi ile kullancıyı pasif hale getirir.
**api/User/SeachUser:** İsim yada soy isim yada kimlik numarası ile kullanıcıyı aratır ve bilgilerini döndürür.
 

## Api Katmanları

Presentation katmanı: Controller burada yer alır, uygulamanın başlangıç projesidir.
Business katmanı: Sorgu operasyonları T-SQL ile gerçekleştirilir, dönüş değerleri modellenir.
Data Katmanı: SQL tablosu EF contexti yer alır.
Model Katmanı: Input, output ve servis modelleri yer alır.
Encryption Katmanı: Verilerin encrypt, decrypt edilebilmesi için gerekli sınıflar burda tanımlıdır.`enter code here`


