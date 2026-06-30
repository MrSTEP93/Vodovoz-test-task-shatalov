# Cистема учёта заказов компании Веселый Водовоз
Десктопное приложение на WPF для управления контрагентами, номенклатурой и заказами. Архитектура разделена на слои с использованием паттернов Repository и MVVM.

## Скриншоты
<details>
  <summary>Нажмите, чтобы увидеть </summary>
  Сотрудники:
<img width="988" height="641" alt="image" src="https://github.com/user-attachments/assets/cd628d42-9931-47a1-abaa-9bb6d521c661" />
<img width="986" height="636" alt="image" src="https://github.com/user-attachments/assets/3175c7bd-2462-45e3-9409-2a23dd541a53" />
  Контрагенты:
<img width="983" height="646" alt="image" src="https://github.com/user-attachments/assets/aaaf70ad-d319-49bd-84bc-9566e910b938" />
<img width="983" height="639" alt="image" src="https://github.com/user-attachments/assets/6ead7b8d-f41b-436b-b586-a9429654a4fd" />
  Заказы:
<img width="988" height="642" alt="image" src="https://github.com/user-attachments/assets/45e32acb-24d6-4efe-b97d-146bf55ef98e" />
<img width="984" height="639" alt="image" src="https://github.com/user-attachments/assets/cdb90ace-0d87-4c23-8df8-71caf7243fc9" />
</details>

## Стек
C# / .NET 10, WPF, MVVM, NHibernate (FluentNHibernate), MySQL, DI, xUnit.

## Как запустить
1. __База данных__: Установить MySQL и создать БД:
```sql
CREATE DATABASE <имя_вашей_базы> CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

2. __Конфигурация__: Открыть appsettings.Local.json (добавлен в gitignore, копия appsettings.json) и прописать параметры подключения к БД.
3. __Запуск__: Открыть решение в IDE и запустить Vododoz.UI. При первом старте будут созданы таблицы в БД.
4. __Тест__: Запустить в Обозревателе тестов или выполнить dotnet test в корне решения.

## Архитектура
* __Разделение слоёв__: Domain изолирован от Data и Services. UI максимально тонкий, вся логика инкапсулирована во ViewModel.
* __Бизнес-правила__: Сервисы валидируют операции и выбрасывают понятное исключеие BusinessRuleException.
* __Внедрение зависимостей__: Использование DependencyInjection для регистрации сервисов, репозиториев и ViewModel.
