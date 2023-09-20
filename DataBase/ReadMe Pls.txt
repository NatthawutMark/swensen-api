ดาต้าเบสใช้ MSSM v.19

ดาต้าเบสถูกเซ็ตไว้ที่ local สามารถเซ็ตที่โปรแกรม MSSM ได้ดังนี้

Server : .\SQLEXPRESS
Athentication : SQL Server Authentication
Users : sa 
Password : 1234

หรือ 

เซ็ตที่โปรเจ็คได้ที่ appsettings.json Line 17

    "DefaultConnection": "Server={Server_name};Database=swensen;User Id={user_name};password={password};Trusted_Connection=False;trustServerCertificate=true;"