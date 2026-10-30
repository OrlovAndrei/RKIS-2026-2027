using System; // Подключаем работу с датой и временем.

public class Profile // Класс профиля пользователя.
{
    public string FirstName { get; private set; } // Имя пользователя.
    public string LastName { get; private set; } // Фамилия пользователя.
    public int BirthYear { get; private set; } // Год рождения пользователя.

    public Profile(string firstName, string lastName, int birthYear) // Конструктор получает данные пользователя.
    {
        FirstName = firstName; // Сохраняем имя.
        LastName = lastName; // Сохраняем фамилию.
        BirthYear = birthYear; // Сохраняем год рождения.
    }
    
    public string GetInfo() // Метод формирует информацию о пользователе.
    {
        int age = DateTime.Now.Year - BirthYear; // Вычисляем возраст по годам.
        return $"{FirstName} {LastName}, возраст {age}"; // Возвращаем готовую строку.
    }
}