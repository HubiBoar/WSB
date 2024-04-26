# Dokumentacja Projektu Lista ToDo

[Link do GitHub](https://github.com/HubiBoar/WsbToDo)

## Temat

Tematem projektu jest prosta aplikacja do tworzenia listy zadań do zrobionenia (ToDo) per użytkownik.

## Opis

Aplikacja po otworzeniu wymaga stworzenia konta, lub zalogowania sie. Po udanym logowaniu powinna pojawić się lista zadań ToDo, danego użytkownika.
Użytkownik powinien móc edytować liste poprzez dodawanie nowych zadań, usuwanie istniejących oraz zmiane stanu danego zadania (skończone czy nie).

## Wymagania

Użytkownicy powinni być trzymani w Bazie danych (aktualnie Sqlite) wraz ze swoimi danymi i listami zadań.
Lista zadań powinna się na bieżąco aktualizować w bazie danych i być widoczna tylko dla aktualnie zalogowanego użytkownika.
Sesja użytkownika powinna być zapisana w formie Cookies.
Wszystkie endpointy API powinny być dostępne przy użyciu standardu OpenApi pod endpointem **/swagger**.

## Odbiorcy

Odbiorcami są ludzie potrzebujący aplikacji do tworzenia listy zadań w przeglądarce.


## Technologie

### Frontend
    - HTMX
    - CSS
    - AJAX
### Backend
    - Asp.Net
    - Sqlite
    - OpenApi
    - EntityFramework.Identity
### Infrastructure
    - Azure App Service
    - GitHub
    - GitHub Actions

## Testy

- Aplikacja posiada projekt do Testów jednostkowych, gdzie trzymane są testy.
- Aplikacja jest zaprojektowana tak aby w łatwy sposób można było dodać testy Integracyjne.
- HTMX ułatwia potencjalne dodanie testów End to End.

## Wyzwania

Wyzwaniem było stworzenie projektu z użyciem HTMXa, który nie jest jeszcze popularny wśród społeczności .Net,
aby to osiągnąć musiałem napisać sporo dodatkowych funkcjonalności.

## Uruchomienie

- Testowy login: **hubibubi@gmail.com** hasło: **Test!1**

### Azure Cloud
- Aplikacja ma wbudowany system CI/CD z użyciem Azure Cloud oraz GitHub Actions, gdzie przy każdym commicie na Main branch, aplikacja w chmurze sie aktualizuje.
 - [Link do Aplikacji postawionej na Azure](https://hubert-dzikuc-wsb-todo.azurewebsites.net/)

### Lokalnie

- Aplikacja wymaga zainstalowanego .Net Sdk 8.0 [Link](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- Komenda **dotnet run** w folderze **src/Host** uruchamia aplikacje.
- Komenda **dotnet test** w folderze **src** uruchamia testy aplikacji. 


## Schematy

![Schemat](Schemat.png)


## Screeny

### Logowanie/Rejestrowanie:

![Screen1](Screen1.png)

### Lista zadań:

![Screen2](Screen2.png)