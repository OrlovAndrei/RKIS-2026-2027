using System;

namespace TodoListApp
{
    // Пункт 4: Класс для хранения данных профиля автора проекта
    public class Profile
    {
        // Свойства (Пункт 4, дефис 1)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }
        // Пункт 2: Новые поля для многопользовательского режима
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        // Конструктор инициализирует базовые данные
        public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
            // Пункт 2: Автоматическое заполнение новых полей при создании
            Id = Guid.NewGuid();
            Login = firstName.ToLower(); // логин по умолчанию — имя маленькими буквами
            Password = "123";            // пароль по умолчанию
        }
        // Пункт 3: Конструктор для загрузки профиля с готовыми параметрами из CSV
        public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }
        // Метод GetInfo возвращает отформатированную строку (Пункт 4, дефис 2)
        public string GetInfo()
        {
            // Автоматический расчет возраста на основе текущего 2026 года
            int age = 2026 - BirthYear;
            return $"{FirstName} {LastName}, возраст {age}";
        }
    }
}
