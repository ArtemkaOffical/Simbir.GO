# Simbir.GO

Для всех сущностей в качестве уникального идентификатора (Id) в базе данных используется тип Guid

В файле appsettings.json находится строка для подключения к базе данных, которую необходимо изменить.
Строка выглядит так -  "DataBase": "Host=localhost;Database=Simbir;Username=postgres;Password=123"

git clone https://github.com/Vanya-Kir/VolgaIT.git
После внесенных изменений, сохраняем и открываем папку через проводник(или cd Simbir.GO) и выполняем след. команды
1. dotnet restore
2. dotnet ef database update
3. dotnet run
## Переходим по ссылке http://localhost:5021/swagger или https://localhost:7185/swagger
